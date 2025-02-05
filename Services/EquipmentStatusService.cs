using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace thaicodelab_api.Services
{
    public class EquipmentStatusService
    {
        private readonly ApplicationDbContext _context;

        public EquipmentStatusService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<EquipmentStatus>> GetAllEquipmentStatus()
        {
            return await _context.tb_equipment_status
                .Where(es => !es.is_deleted)
                .OrderBy(es => es.equipment_status_id)
                .ToListAsync();
        }

        public async Task<EquipmentStatus?> GetEquipmentStatusById(int id)
        {
            return await _context.tb_equipment_status
                .FirstOrDefaultAsync(es => es.equipment_status_id == id && !es.is_deleted);
        }

        public async Task<string> GenerateEquipmentStatusCode()
        {
            var latestEquipmentStatus = await _context.tb_equipment_status
                .OrderByDescending(es => es.equipment_status_code)
                .FirstOrDefaultAsync();

            string newCode = "EQS0000001";
            if (latestEquipmentStatus != null && !string.IsNullOrEmpty(latestEquipmentStatus.equipment_status_code))
            {
                var match = Regex.Match(latestEquipmentStatus.equipment_status_code, @"\d+");
                if (match.Success)
                {
                    int latestNumber = int.Parse(match.Value);
                    newCode = $"EQS{(latestNumber + 1).ToString("D7")}";
                }
            }
            return newCode;
        }

        public async Task AddEquipmentStatus(EquipmentStatus equipmentStatus)
        {
            _context.tb_equipment_status.Add(equipmentStatus);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEquipmentStatus(EquipmentStatus existingEquipmentStatus, EquipmentStatus updatedEquipmentStatus, int userId)
        {
            existingEquipmentStatus.equipment_status = updatedEquipmentStatus.equipment_status;
            existingEquipmentStatus.updated_by = userId;
            existingEquipmentStatus.updated_at = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteEquipmentStatus(EquipmentStatus equipmentStatus, int userId)
        {
            equipmentStatus.is_deleted = true;
            equipmentStatus.updated_by = userId;
            equipmentStatus.updated_at = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}
