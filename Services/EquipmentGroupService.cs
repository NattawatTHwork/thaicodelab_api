using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace thaicodelab_api.Services
{
    public class EquipmentGroupService
    {
        private readonly ApplicationDbContext _context;

        public EquipmentGroupService(ApplicationDbContext context)
        {
            _context = context;
        }

        // public async Task<List<EquipmentGroup>> GetAllEquipmentGroups()
        // {
        //     return await _context.tb_equipment_groups
        //         .Where(eg => !eg.is_deleted)
        //         .OrderBy(eg => eg.equipment_group_id)
        //         .ToListAsync();
        // }

        public async Task<List<EquipmentGroupRequest>> GetAllEquipmentGroups()
        {
            var result = await _context.tb_equipment_groups
                .Where(eg => !eg.is_deleted)
                .Join(
                    _context.tb_departments,
                    eg => eg.department_id, // คีย์เชื่อมจาก tb_equipment_groups
                    d => d.department_id,   // คีย์เชื่อมจาก tb_departments
                    (eg, d) => new { eg, d } // จับคู่ข้อมูลทั้งสองตาราง
                )
                .OrderBy(egd => egd.eg.equipment_group_id) // เรียงตาม equipment_group_id
                .Select(egd => new EquipmentGroupRequest
                {
                    equipment_group_id = egd.eg.equipment_group_id,
                    equipment_group_code = egd.eg.equipment_group_code,
                    equipment_group = egd.eg.equipment_group,
                    department_id = egd.d.department_id,
                    department_code = egd.d.department_code, // ดึงข้อมูล department_code
                    department = egd.d.department, // ดึงข้อมูล department_name
                    created_at = egd.eg.created_at,
                    updated_at = egd.eg.updated_at,
                    created_by = egd.eg.created_by,
                    updated_by = egd.eg.updated_by,
                    is_deleted = egd.eg.is_deleted
                })
                .ToListAsync();

            return result;
        }

        // public async Task<List<EquipmentGroup>> GetEquipmentGroupsByDepartmentId(int departmentId)
        // {
        //     return await _context.tb_equipment_groups
        //         .Where(eg => eg.department_id == departmentId && !eg.is_deleted)
        //         .ToListAsync();
        // }
        public async Task<List<EquipmentGroupRequest>> GetEquipmentGroupsByDepartmentId(int departmentId)
        {
            var result = await _context.tb_equipment_groups
                .Where(eg => eg.department_id == departmentId && !eg.is_deleted)
                .Join(
                    _context.tb_departments,
                    eg => eg.department_id,
                    d => d.department_id,
                    (eg, d) => new { eg, d }
                )
                .OrderBy(egd => egd.eg.equipment_group_id)
                .Select(egd => new EquipmentGroupRequest
                {
                    equipment_group_id = egd.eg.equipment_group_id,
                    equipment_group_code = egd.eg.equipment_group_code,
                    equipment_group = egd.eg.equipment_group,
                    department_id = egd.d.department_id,
                    department_code = egd.d.department_code,
                    department = egd.d.department,
                    created_at = egd.eg.created_at,
                    updated_at = egd.eg.updated_at,
                    created_by = egd.eg.created_by,
                    updated_by = egd.eg.updated_by,
                    is_deleted = egd.eg.is_deleted
                })
                .ToListAsync();

            return result;
        }

        public async Task<EquipmentGroup?> GetEquipmentGroupById(int id)
        {
            return await _context.tb_equipment_groups
                .FirstOrDefaultAsync(eg => eg.equipment_group_id == id && !eg.is_deleted);
        }

        public async Task<EquipmentGroupRequest?> GetEquipmentGroupJoinDepartmentById(int id)
        {
            var result = await _context.tb_equipment_groups
                .Where(eg => eg.equipment_group_id == id && !eg.is_deleted)
                .Join(
                    _context.tb_departments,
                    eg => eg.department_id,
                    d => d.department_id,
                    (eg, d) => new { eg, d }
                )
                .Select(egd => new EquipmentGroupRequest
                {
                    equipment_group_id = egd.eg.equipment_group_id,
                    equipment_group_code = egd.eg.equipment_group_code,
                    equipment_group = egd.eg.equipment_group,
                    department_id = egd.d.department_id,
                    department_code = egd.d.department_code,
                    department = egd.d.department,
                    created_at = egd.eg.created_at,
                    updated_at = egd.eg.updated_at,
                    created_by = egd.eg.created_by,
                    updated_by = egd.eg.updated_by,
                    is_deleted = egd.eg.is_deleted
                })
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<string> GenerateEquipmentGroupCode()
        {
            var latestEquipmentGroup = await _context.tb_equipment_groups
                .OrderByDescending(eg => eg.equipment_group_code)
                .FirstOrDefaultAsync();

            string newCode = "EQC0000001";
            if (latestEquipmentGroup != null && !string.IsNullOrEmpty(latestEquipmentGroup.equipment_group_code))
            {
                var match = Regex.Match(latestEquipmentGroup.equipment_group_code, @"\d+");
                if (match.Success)
                {
                    int latestNumber = int.Parse(match.Value);
                    newCode = $"EQC{(latestNumber + 1).ToString("D7")}";
                }
            }
            return newCode;
        }

        public async Task AddEquipmentGroup(EquipmentGroup equipmentGroup)
        {
            _context.tb_equipment_groups.Add(equipmentGroup);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEquipmentGroup(EquipmentGroup existingEquipmentGroup, EquipmentGroup updatedEquipmentGroup, int userId)
        {
            existingEquipmentGroup.equipment_group = updatedEquipmentGroup.equipment_group;
            existingEquipmentGroup.department_id = updatedEquipmentGroup.department_id;
            existingEquipmentGroup.updated_by = userId;
            existingEquipmentGroup.updated_at = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteEquipmentGroup(EquipmentGroup equipmentGroup, int userId)
        {
            equipmentGroup.is_deleted = true;
            equipmentGroup.updated_by = userId;
            equipmentGroup.updated_at = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}
