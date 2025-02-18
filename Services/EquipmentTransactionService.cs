using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace thaicodelab_api.Services
{
    public class EquipmentTransactionService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public EquipmentTransactionService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task<List<EquipmentTransaction>> GetAllEquipmentTransactions()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                return await _context.tb_equipment_transactions
                    .Where(t => !t.is_deleted)
                    .ToListAsync();
            }
        }

        public async Task<List<EquipmentTransactionWithDetails>> GetAllEquipmentTransactionsWithDetails()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var transactions = await _context.tb_equipment_transactions
                    .Where(t => !t.is_deleted)
                    .Select(t => new EquipmentTransactionWithDetails
                    {
                        equipment_transaction_id = t.equipment_transaction_id,
                        equipment_transaction_code = t.equipment_transaction_code,
                        approve_user_id = t.approve_user_id,
                        borrow_user_id = t.borrow_user_id,
                        borrow_timestamp = t.borrow_timestamp,
                        note = t.note,
                        updated_at = t.updated_at,
                        updated_by = t.updated_by,
                        details = _context.tb_equipment_transaction_details
                            .Where(d => d.equipment_transaction_id == t.equipment_transaction_id)
                            .ToList()
                    })
                    .ToListAsync();

                return transactions;
            }
        }

        public async Task<EquipmentTransaction?> GetEquipmentTransactionById(int id)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                return await _context.tb_equipment_transactions
                    .FirstOrDefaultAsync(et => et.equipment_transaction_id == id && !et.is_deleted);
            }
        }

        public async Task<string> GenerateEquipmentTransactionCode()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var latestEquipmentTransaction = await _context.tb_equipment_transactions
                    .OrderByDescending(es => es.equipment_transaction_code)
                    .FirstOrDefaultAsync();

                string newCode = "EQTS0000001";
                if (latestEquipmentTransaction != null && !string.IsNullOrEmpty(latestEquipmentTransaction.equipment_transaction_code))
                {
                    var match = Regex.Match(latestEquipmentTransaction.equipment_transaction_code, @"\d+");
                    if (match.Success)
                    {
                        int latestNumber = int.Parse(match.Value);
                        newCode = $"EQTS{(latestNumber + 1).ToString("D7")}";
                    }
                }
                return newCode;
            }
        }

        public async Task<string> GenerateEquipmentTransactionDetailCode(int increment)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // ค้นหาข้อมูลล่าสุด
                var latestEquipmentTransactionDetail = await _context.tb_equipment_transaction_details
                    .OrderByDescending(es => es.equipment_transaction_detail_code)
                    .FirstOrDefaultAsync();

                string newCode;
                if (latestEquipmentTransactionDetail == null || string.IsNullOrEmpty(latestEquipmentTransactionDetail.equipment_transaction_detail_code))
                {
                    // ถ้ายังไม่มีข้อมูลเลย ให้เริ่มจาก 1 แล้วเพิ่ม increment
                    newCode = $"EQTD{(1 + increment - 1).ToString("D7")}";
                }
                else
                {
                    // ถ้ามีข้อมูลแล้ว ดึงตัวเลขที่มากที่สุด และบวก increment
                    var match = Regex.Match(latestEquipmentTransactionDetail.equipment_transaction_detail_code, @"\d+");
                    if (match.Success)
                    {
                        int latestNumber = int.Parse(match.Value);
                        newCode = $"EQTD{(latestNumber + increment).ToString("D7")}";
                    }
                    else
                    {
                        newCode = $"EQTD{increment.ToString("D7")}";
                    }
                }

                return newCode;
            }
        }

        public async Task<List<int>> CheckUnreturnedEquipment(int[] equipmentIds)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // ดึงรายการอุปกรณ์ที่ยังไม่ได้คืน
                var notReturnedEquipment = await _context.tb_equipment_transaction_details
                    .Where(d => equipmentIds.Contains(d.equipment_id) &&
                                (d.return_user_id == null || d.operator_return_user_id == null))
                    .Select(d => d.equipment_id)
                    .ToListAsync();

                return notReturnedEquipment;
            }
        }

        public async Task<int> BorrowEquipmentTransaction(EquipmentTransaction equipmentTransaction, List<int> equipmentIds)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // บันทึกข้อมูลการยืมลงใน tb_equipment_transactions
                _context.tb_equipment_transactions.Add(equipmentTransaction);
                await _context.SaveChangesAsync();

                // อัปเดต equipment_status_id เป็น 2 (กำลังถูกยืม) ใน tb_equipments
                var equipments = await _context.tb_equipments
                    .Where(e => equipmentIds.Contains(e.equipment_id))
                    .ToListAsync();

                foreach (var equipment in equipments)
                {
                    equipment.equipment_status_id = 2; // กำลังถูกยืม
                }

                await _context.SaveChangesAsync();

                return equipmentTransaction.equipment_transaction_id;
            }
        }

        public async Task BorrowEquipmentTransactionDetails(List<EquipmentTransactionDetail> equipmentTransactionDetails)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                _context.tb_equipment_transaction_details.AddRange(equipmentTransactionDetails);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> ReturnEquipmentTransaction(List<EquipmentReturnDetail> equipmentReturnDetails, int returnUserId, int operatorReturnUserId)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var transactionDetailIds = equipmentReturnDetails.Select(e => e.equipment_transaction_detail_id).ToList();

                var equipmentTransactions = await _context.tb_equipment_transaction_details
                    .Where(d => transactionDetailIds.Contains(d.equipment_transaction_detail_id) &&
                                d.return_user_id == null &&
                                d.operator_return_user_id == null)
                    .ToListAsync();

                if (!equipmentTransactions.Any())
                    return 0;

                var equipmentIds = equipmentTransactions.Select(e => e.equipment_id).ToList();

                foreach (var transaction in equipmentTransactions)
                {
                    var returnDetail = equipmentReturnDetails.FirstOrDefault(e => e.equipment_transaction_detail_id == transaction.equipment_transaction_detail_id);
                    if (returnDetail != null)
                    {
                        transaction.return_user_id = returnUserId;
                        transaction.operator_return_user_id = operatorReturnUserId;
                        transaction.return_timestamp = DateTime.UtcNow;
                        transaction.updated_at = DateTime.UtcNow;
                        transaction.note = returnDetail.note ?? "";
                    }
                }

                // อัปเดต equipment_status_id เป็น 2 (ถูกคืนแล้ว) ใน tb_equipments
                var equipments = await _context.tb_equipments
                    .Where(e => equipmentIds.Contains(e.equipment_id))
                    .ToListAsync();

                foreach (var equipment in equipments)
                {
                    equipment.equipment_status_id = 1; // คืนแล้ว
                }

                await _context.SaveChangesAsync();

                return equipmentTransactions.Count;
            }
        }

        public async Task<List<UnreturnedEquipmentWithGroup>> GetUnreturnedEquipment()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var unreturnedEquipment = await _context.tb_equipment_transaction_details
                    .Where(etd => etd.return_user_id == null && !etd.is_deleted) // เฉพาะที่ยังไม่ได้คืน และไม่ถูกลบ
                    .Join(_context.tb_equipments,
                        etd => etd.equipment_id,
                        e => e.equipment_id,
                        (etd, e) => new { etd, e }) // เชื่อมกับ tb_equipments
                    .Join(_context.tb_equipment_groups,
                        join1 => join1.e.equipment_group_id,
                        eg => eg.equipment_group_id,
                        (join1, eg) => new { join1.etd, join1.e, eg }) // เชื่อมกับ tb_equipment_groups
                    .Select(join2 => new UnreturnedEquipmentWithGroup
                    {
                        equipment_id = join2.e.equipment_id,
                        equipment = join2.e.equipment,
                        equipment_code = join2.e.equipment_code,
                        equipment_transaction_detail_id = join2.etd.equipment_transaction_detail_id,
                        equipment_transaction_id = join2.etd.equipment_transaction_id,
                        equipment_group_id = join2.eg.equipment_group_id,
                        equipment_group = join2.eg.equipment_group,
                        department_id = join2.eg.department_id
                    })
                    .ToListAsync();

                return unreturnedEquipment;
            }
        }

        public async Task<List<UnreturnedEquipmentWithGroup>> GetUnreturnedEquipmentByDepartment(int departmentId)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var unreturnedEquipment = await _context.tb_equipment_transaction_details
                    .Where(etd => etd.return_user_id == null && !etd.is_deleted) // เฉพาะที่ยังไม่ได้คืน และไม่ถูกลบ
                    .Join(_context.tb_equipments,
                        etd => etd.equipment_id,
                        e => e.equipment_id,
                        (etd, e) => new { etd, e }) // เชื่อมกับ tb_equipments
                    .Join(_context.tb_equipment_groups,
                        join1 => join1.e.equipment_group_id,
                        eg => eg.equipment_group_id,
                        (join1, eg) => new { join1.etd, join1.e, eg }) // เชื่อมกับ tb_equipment_groups
                    .Where(join2 => join2.eg.department_id == departmentId) // กรองตาม department_id
                    .Select(join2 => new UnreturnedEquipmentWithGroup
                    {
                        equipment_id = join2.e.equipment_id,
                        equipment = join2.e.equipment,
                        equipment_code = join2.e.equipment_code,
                        equipment_transaction_detail_id = join2.etd.equipment_transaction_detail_id,
                        equipment_transaction_id = join2.etd.equipment_transaction_id,
                        equipment_group_id = join2.eg.equipment_group_id,
                        equipment_group = join2.eg.equipment_group,
                        department_id = join2.eg.department_id
                    })
                    .ToListAsync();

                return unreturnedEquipment;
            }
        }

    }
}
