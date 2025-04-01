using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using thaicodelab_api.Helpers;
using thaicodelab_api.Services;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EquipmentTransactionController : ControllerBase
{
    private readonly EquipmentTransactionService _equipmentTransactionService;

    public EquipmentTransactionController(EquipmentTransactionService equipmentTransactionService)
    {
        _equipmentTransactionService = equipmentTransactionService;
    }

    [HttpGet] // เรียกดูรายการยืมทั้งหมด
    public async Task<IActionResult> GetAllTransactions()
    {
        var transactions = await _equipmentTransactionService.GetAllEquipmentTransactions();

        return Ok(new
        {
            status = true,
            message = "Successfully Retrieved",
            data = transactions
        });
    }

    [HttpGet("with-details")] // เรียกดูรายการยืมทั้งหมดพร้อมรายละเอียด
    public async Task<IActionResult> GetAllTransactionsWithDetails()
    {
        var transactions = await _equipmentTransactionService.GetAllEquipmentTransactionsWithDetails();

        return Ok(new
        {
            status = true,
            message = "Successfully Retrieved",
            data = transactions
        });
    }

    [HttpGet("details-with-transaction")]
    public async Task<IActionResult> GetEquipmentTransactionDetailsWithTransaction()
    {
        var details = await _equipmentTransactionService.GetEquipmentTransactionDetailsWithTransaction();

        return Ok(new
        {
            status = true,
            message = "Successfully Retrieved",
            data = details
        });
    }

    [HttpGet("details-with-transaction-by-equipment/{id}")]
    public async Task<IActionResult> GetDetailsWithTransactionByEquipment([FromRoute] int id)
    {
        var details = await _equipmentTransactionService
            .GetEquipmentTransactionDetailsWithTransactionByEquipmentId(id);

        return Ok(new
        {
            status = true,
            message = "Successfully Retrieved",
            data = details
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var transaction = await _equipmentTransactionService.GetEquipmentTransactionById(id);

        if (transaction == null)
        {
            return NotFound(new
            {
                status = false,
                message = "Equipment Transaction not found"
            });
        }

        return Ok(new
        {
            status = true,
            message = "Successfully Retrieved",
            data = transaction
        });
    }

    [HttpPost("borrow")] // ยืมอุปกรณ์
    public async Task<IActionResult> BorrowEquipment([FromBody] EquipmentTransactionRequest request)
    {
        if (request.equipment_ids == null || request.equipment_ids.Length == 0)
        {
            return BadRequest(new
            {
                status = false,
                message = "Equipment IDs cannot be empty"
            });
        }

        var notReturnedEquipment = await _equipmentTransactionService.CheckUnreturnedEquipment(request.equipment_ids);

        if (notReturnedEquipment.Any())
        {
            return BadRequest(new
            {
                status = false,
                message = $"Some equipment has not been returned: {string.Join(", ", notReturnedEquipment)}"
            });
        }

        var userId = JwtHelper.GetUserIdFromToken(User);

        var transaction = new EquipmentTransaction
        {
            equipment_transaction_code = await _equipmentTransactionService.GenerateEquipmentTransactionCode(),
            approve_user_id = userId,  // request.approve_user_id เป็นหลักเมื่อมีผู้อนุมัติ
            operator_borrow_user_id = userId, // request.operator_borrow_user_id เป็นหลักเมื่อมีผู้อนุมัติ
            borrow_user_id = request.borrow_user_id,
            borrow_timestamp = DateTime.UtcNow,
            note = request.note,
            updated_at = DateTime.UtcNow,
            updated_by = userId
        };

        var equipmentTransactionId = await _equipmentTransactionService.BorrowEquipmentTransaction(transaction, request.equipment_ids.ToList());

        var transactionDetails = (await Task.WhenAll(request.equipment_ids
            .Select(async (equipmentId, index) => new EquipmentTransactionDetail
            {
                equipment_transaction_id = equipmentTransactionId,
                equipment_transaction_detail_code = await _equipmentTransactionService.GenerateEquipmentTransactionDetailCode(index + 1),
                equipment_id = equipmentId
            }))).ToList();

        await _equipmentTransactionService.BorrowEquipmentTransactionDetails(transactionDetails);

        return CreatedAtAction(nameof(GetById), new { id = equipmentTransactionId }, new
        {
            status = true,
            message = "Successfully Created",
            data = transaction
        });
    }

    [HttpPut("return")] // คืนอุปกรณ์
    public async Task<IActionResult> ReturnEquipment([FromBody] EquipmentReturnRequest request)
    {
        if (request.equipment_return_details == null || request.equipment_return_details.Count == 0)
        {
            return BadRequest(new
            {
                status = false,
                message = "Equipment Transaction Detail IDs cannot be empty"
            });
        }

        var userId = JwtHelper.GetUserIdFromToken(User);

        var updatedCount = await _equipmentTransactionService.ReturnEquipmentTransaction(
            request.equipment_return_details, request.return_user_id, userId // request.operator_return_user_id เป็นหลักเมื่อมีผู้อนุมัติ
        );

        if (updatedCount == 0)
        {
            return NotFound(new
            {
                status = false,
                message = "No matching equipment transactions found for return"
            });
        }

        return Ok(new
        {
            status = true,
            message = "Successfully Returned Equipment",
            updated_count = updatedCount
        });
    }

    [HttpGet("unreturned-equipment")] // อุปกรณ์ที่ยังไม่ได้คืนทั้งหมด
    public async Task<IActionResult> GetUnreturnedEquipment()
    {
        try
        {
            var unreturnedEquipment = await _equipmentTransactionService.GetUnreturnedEquipmentAll();

            return Ok(new
            {
                status = true,
                message = "Successfully Retrieved",
                data = unreturnedEquipment
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { status = false, message = ex.Message });
        }
    }


    [HttpGet("unreturned-equipment-by-department")] // อุปกรณ์ที่ยังไม่ได้คืนแต่ละ department ถ้า role_id = 1 ดูได้ทั้งหมด
    public async Task<IActionResult> GetUnreturnedEquipmentByDepartment()
    {
        try
        {
            int roleId = JwtHelper.GetRoleIdFromToken(User);

            List<ReturnAndUnreturnedEquipmentWithGroup> unreturnedEquipment;

            if (roleId == 1)
            {
                // สำหรับ Admin (ดูได้ทั้งหมด)
                unreturnedEquipment = await _equipmentTransactionService.GetUnreturnedEquipmentAll();
            }
            else
            {
                // สำหรับผู้ใช้งานทั่วไป ดูเฉพาะแผนกตัวเอง
                int departmentId = JwtHelper.GetDepartmentIdFromToken(User);
                unreturnedEquipment = await _equipmentTransactionService.GetUnreturnedEquipmentByDepartment(departmentId);
            }

            return Ok(new
            {
                status = true,
                message = "Successfully Retrieved",
                data = unreturnedEquipment
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { status = false, message = ex.Message });
        }
    }

    [HttpGet("returned-equipment-by-department")] // อุปกรณ์คืนแล้วแต่ละ department
    public async Task<IActionResult> GetReturnedEquipmentByDepartment()
    {
        try
        {
            int departmentId = JwtHelper.GetDepartmentIdFromToken(User);

            var unreturnedEquipment = await _equipmentTransactionService.GetReturnedEquipmentByDepartment(departmentId);

            return Ok(new
            {
                status = true,
                message = "Successfully Retrieved",
                data = unreturnedEquipment
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { status = false, message = ex.Message });
        }
    }
}
