using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace thaicodelab_api.Services
{
    public class EquipmentService
    {
        private readonly ApplicationDbContext _context;

        public EquipmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<EquipmentRequest>> GetAllEquipments()
        {
            var query = from e in _context.tb_equipments
                        where !e.is_deleted
                        join g in _context.tb_equipment_groups on e.equipment_group_id equals g.equipment_group_id
                        join t in _context.tb_equipment_types on e.equipment_type_id equals t.equipment_type_id
                        join s in _context.tb_equipment_status on e.equipment_status_id equals s.equipment_status_id

                        // LEFT JOIN tb_users
                        join u in _context.tb_users on e.borrow_user_id equals u.user_id into userGroup
                        from user in userGroup.DefaultIfEmpty()

                            // LEFT JOIN tb_ranks
                        join r in _context.tb_ranks on user.rank_id equals r.rank_id into rankGroup
                        from rank in rankGroup.DefaultIfEmpty()

                        orderby e.equipment_id
                        select new EquipmentRequest
                        {
                            equipment_id = e.equipment_id,
                            equipment_code = e.equipment_code,
                            equipment_unique_code = e.equipment_unique_code,
                            equipment = e.equipment,
                            description = e.description,
                            equipment_group_id = g.equipment_group_id,
                            equipment_group_code = g.equipment_group_code,
                            equipment_group = g.equipment_group,
                            equipment_type_id = t.equipment_type_id,
                            equipment_type_code = t.equipment_type_code,
                            equipment_type = t.equipment_type,
                            equipment_status_id = s.equipment_status_id,
                            equipment_status_code = s.equipment_status_code,
                            equipment_status = s.equipment_status,
                            borrow_user_id = user != null ? user.user_id : 0,
                            firstname = user != null ? user.firstname : string.Empty,
                            lastname = user != null ? user.lastname : string.Empty,
                            full_rank = rank != null ? rank.full_rank : string.Empty,
                            short_rank = rank != null ? rank.short_rank : string.Empty
                        };

            return await query.ToListAsync();
        }

        public async Task<List<EquipmentRequest>> GetAllEquipmentByDepartments(int departmentId)
        {
            var query = from e in _context.tb_equipments
                        where !e.is_deleted
                        join g in _context.tb_equipment_groups on e.equipment_group_id equals g.equipment_group_id
                        join t in _context.tb_equipment_types on e.equipment_type_id equals t.equipment_type_id
                        join s in _context.tb_equipment_status on e.equipment_status_id equals s.equipment_status_id

                        // LEFT JOIN tb_users
                        join u in _context.tb_users on e.borrow_user_id equals u.user_id into userGroup
                        from user in userGroup.DefaultIfEmpty()

                            // LEFT JOIN tb_ranks
                        join r in _context.tb_ranks on user.rank_id equals r.rank_id into rankGroup
                        from rank in rankGroup.DefaultIfEmpty()

                        where g.department_id == departmentId

                        orderby e.equipment_id
                        select new EquipmentRequest
                        {
                            equipment_id = e.equipment_id,
                            equipment_code = e.equipment_code,
                            equipment_unique_code = e.equipment_unique_code,
                            equipment = e.equipment,
                            description = e.description,
                            equipment_group_id = g.equipment_group_id,
                            equipment_group_code = g.equipment_group_code,
                            equipment_group = g.equipment_group,
                            equipment_type_id = t.equipment_type_id,
                            equipment_type_code = t.equipment_type_code,
                            equipment_type = t.equipment_type,
                            equipment_status_id = s.equipment_status_id,
                            equipment_status_code = s.equipment_status_code,
                            equipment_status = s.equipment_status,
                            borrow_user_id = user != null ? user.user_id : 0,
                            firstname = user != null ? user.firstname : string.Empty,
                            lastname = user != null ? user.lastname : string.Empty,
                            full_rank = rank != null ? rank.full_rank : string.Empty,
                            short_rank = rank != null ? rank.short_rank : string.Empty
                        };

            return await query.ToListAsync();
        }

        public async Task<List<Equipment>> GetBorrowedEquipments()
        {
            return await _context.tb_equipments
                .Where(e => e.equipment_status_id == 2 && !e.is_deleted)
                .OrderBy(e => e.equipment_id)
                .ToListAsync();
        }

        public async Task<List<Equipment>> GetBorrowedEquipmentByDepartments(int departmentId)
        {
            return await _context.tb_equipments
                .Join(
                    _context.tb_equipment_groups,
                    e => e.equipment_group_id,   // คีย์จาก tb_equipments
                    eg => eg.equipment_group_id, // คีย์จาก tb_equipment_groups
                    (e, eg) => new { Equipment = e, EquipmentGroup = eg } // สร้าง Anonymous Type
                )
                .Where(joined =>
                    joined.Equipment.equipment_status_id == 2 &&
                    !joined.Equipment.is_deleted &&
                    joined.EquipmentGroup.department_id == departmentId
                )
                .OrderBy(joined => joined.Equipment.equipment_id)
                .Select(joined => joined.Equipment) // เลือกเฉพาะ Equipment ที่ต้องการ
                .ToListAsync();
        }

        public async Task<List<Equipment>> GetReturnedEquipments()
        {
            return await _context.tb_equipments
                .Where(e => e.equipment_status_id == 1 && !e.is_deleted)
                .OrderBy(e => e.equipment_id)
                .ToListAsync();
        }

        public async Task<List<Equipment>> GetReturnedEquipmentAll()
        {
            return await _context.tb_equipments
                .Join(
                    _context.tb_equipment_groups,
                    e => e.equipment_group_id,
                    eg => eg.equipment_group_id,
                    (e, eg) => new { Equipment = e, EquipmentGroup = eg }
                )
                .Where(joined =>
                    joined.Equipment.equipment_status_id == 1 &&
                    !joined.Equipment.is_deleted
                )
                .OrderBy(joined => joined.Equipment.equipment_id)
                .Select(joined => joined.Equipment)
                .ToListAsync();
        }

        public async Task<List<Equipment>> GetReturnedEquipmentByDepartments(int departmentId)
        {
            return await _context.tb_equipments
                .Join(
                    _context.tb_equipment_groups,
                    e => e.equipment_group_id,   // คีย์จาก tb_equipments
                    eg => eg.equipment_group_id, // คีย์จาก tb_equipment_groups
                    (e, eg) => new { Equipment = e, EquipmentGroup = eg } // สร้าง Anonymous Type
                )
                .Where(joined =>
                    joined.Equipment.equipment_status_id == 1 &&
                    !joined.Equipment.is_deleted &&
                    joined.EquipmentGroup.department_id == departmentId
                )
                .OrderBy(joined => joined.Equipment.equipment_id)
                .Select(joined => joined.Equipment) // เลือกเฉพาะ Equipment ที่ต้องการ
                .ToListAsync();
        }

        public async Task<Equipment?> GetEquipmentById(int id)
        {
            return await _context.tb_equipments
                .FirstOrDefaultAsync(e => e.equipment_id == id && !e.is_deleted);
        }

        public async Task<EquipmentRequest?> GetEquipmentDataById(int id)
        {
            return await _context.tb_equipments
                .Where(e => e.equipment_id == id && !e.is_deleted)
                .Join(_context.tb_equipment_groups, e => e.equipment_group_id, g => g.equipment_group_id, (e, g) => new { e, g })
                .Join(_context.tb_equipment_types, eg => eg.e.equipment_type_id, t => t.equipment_type_id, (eg, t) => new { eg.e, eg.g, t })
                .Join(_context.tb_equipment_status, egt => egt.e.equipment_status_id, s => s.equipment_status_id, (egt, s) => new { egt.e, egt.g, egt.t, s })
                .Select(egts => new EquipmentRequest
                {
                    equipment_id = egts.e.equipment_id,
                    equipment_code = egts.e.equipment_code,
                    equipment_unique_code = egts.e.equipment_unique_code,
                    equipment = egts.e.equipment,
                    description = egts.e.description,
                    equipment_group_id = egts.g.equipment_group_id,
                    equipment_group_code = egts.g.equipment_group_code,
                    equipment_group = egts.g.equipment_group,
                    equipment_type_id = egts.t.equipment_type_id,
                    equipment_type_code = egts.t.equipment_type_code,
                    equipment_type = egts.t.equipment_type,
                    equipment_status_id = egts.s.equipment_status_id,
                    equipment_status_code = egts.s.equipment_status_code,
                    equipment_status = egts.s.equipment_status
                })
                .FirstOrDefaultAsync();
        }

        public async Task<string> GenerateEquipmentCode()
        {
            var latestEquipment = await _context.tb_equipments
                .OrderByDescending(e => e.equipment_code)
                .FirstOrDefaultAsync();

            string newCode = "EQ0000001";
            if (latestEquipment != null && !string.IsNullOrEmpty(latestEquipment.equipment_code))
            {
                var match = Regex.Match(latestEquipment.equipment_code, @"\d+");
                if (match.Success)
                {
                    int latestNumber = int.Parse(match.Value);
                    newCode = $"EQ{(latestNumber + 1).ToString("D7")}";
                }
            }
            return newCode;
        }

        public async Task<int> AddEquipment(Equipment equipment)
        {
            _context.tb_equipments.Add(equipment);
            await _context.SaveChangesAsync();
            return equipment.equipment_id;
        }

        public async Task UpdateEquipment(Equipment equipment, Equipment updatedEquipment, int userId)
        {
            equipment.equipment_unique_code = updatedEquipment.equipment_unique_code;
            equipment.equipment = updatedEquipment.equipment;
            equipment.description = updatedEquipment.description;
            equipment.equipment_group_id = updatedEquipment.equipment_group_id;
            equipment.equipment_type_id = updatedEquipment.equipment_type_id;
            equipment.equipment_status_id = updatedEquipment.equipment_status_id;
            equipment.updated_at = DateTime.UtcNow;
            equipment.updated_by = userId;

            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteEquipment(Equipment equipment, int userId)
        {
            equipment.is_deleted = true;
            equipment.updated_by = userId;
            equipment.updated_at = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}
