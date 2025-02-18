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

        public async Task<List<Equipment>> GetAllEquipments()
        {
            return await _context.tb_equipments
                .Where(e => !e.is_deleted)
                .OrderBy(e => e.equipment_id)
                .ToListAsync();
        }

        public async Task<List<Equipment>> GetBorrowedEquipments()
        {
            return await _context.tb_equipments
                .Where(e => e.equipment_status_id == 1 && !e.is_deleted)
                .OrderBy(e => e.equipment_id)
                .ToListAsync();
        }

        public async Task<Equipment?> GetEquipmentById(int id)
        {
            return await _context.tb_equipments
                .FirstOrDefaultAsync(e => e.equipment_id == id && !e.is_deleted);
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
            equipment.equipment_code = updatedEquipment.equipment_code;
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
