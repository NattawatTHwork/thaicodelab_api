using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using thaicodelab_api.Helpers;
using thaicodelab_api.Services;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EquipmentController : ControllerBase
{
    private readonly EquipmentService _equipmentService;

    public EquipmentController(EquipmentService equipmentService)
    {
        _equipmentService = equipmentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _equipmentService.GetAllEquipments();
        return Ok(new
        {
            status = true,
            message = "Successfully Retrieved",
            data = result
        });
    }

    [HttpGet("borrowed")]
    public async Task<IActionResult> GetBorrowedEquipments()
    {
        var result = await _equipmentService.GetBorrowedEquipments();
        return Ok(new
        {
            status = true,
            message = "Successfully Retrieved",
            data = result
        });
    }

    [HttpGet("borrowed-equipment-by-department")]
    public async Task<IActionResult> GetBorrowedEquipmentByDepartments()
    {
        int departmentId = JwtHelper.GetDepartmentIdFromToken(User); // ดึง department_id จาก JWT

        var result = await _equipmentService.GetBorrowedEquipmentByDepartments(departmentId);
        return Ok(new
        {
            status = true,
            message = "Successfully Retrieved",
            data = result
        });
    }

    [HttpGet("returned-equipment")]
    public async Task<IActionResult> GetReturnedEquipments()
    {
        var result = await _equipmentService.GetReturnedEquipments();
        return Ok(new
        {
            status = true,
            message = "Successfully Retrieved",
            data = result
        });
    }

    [HttpGet("returned-equipment-by-department")]
    public async Task<IActionResult> GetReturnedEquipmentByDepartments()
    {
        int departmentId = JwtHelper.GetDepartmentIdFromToken(User); // ดึง department_id จาก JWT

        var result = await _equipmentService.GetReturnedEquipmentByDepartments(departmentId);
        return Ok(new
        {
            status = true,
            message = "Successfully Retrieved",
            data = result
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var equipment = await _equipmentService.GetEquipmentDataById(id);
        if (equipment == null)
            return NotFound(new
            {
                status = false,
                message = "Equipment not found"
            });

        return Ok(new
        {
            status = true,
            message = "Successfully Retrieved",
            data = equipment
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Equipment equipment)
    {
        var userId = JwtHelper.GetUserIdFromToken(User);
        equipment.equipment_code = await _equipmentService.GenerateEquipmentCode();
        equipment.created_at = DateTime.UtcNow;
        equipment.created_by = userId;

        await _equipmentService.AddEquipment(equipment);

        return CreatedAtAction(nameof(GetById), new { id = equipment.equipment_id }, new
        {
            status = true,
            message = "Successfully Created",
            data = equipment
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Equipment updatedEquipment)
    {
        var userId = JwtHelper.GetUserIdFromToken(User);
        var equipment = await _equipmentService.GetEquipmentById(id);

        if (equipment == null)
            return NotFound(new
            {
                status = false,
                message = "Equipment not found"
            });

        await _equipmentService.UpdateEquipment(equipment, updatedEquipment, userId);

        return Ok(new
        {
            status = true,
            message = "Successfully Updated"
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = JwtHelper.GetUserIdFromToken(User);
        var equipment = await _equipmentService.GetEquipmentById(id);

        if (equipment == null)
            return NotFound(new
            {
                status = false,
                message = "Equipment not found"
            });

        await _equipmentService.SoftDeleteEquipment(equipment, userId);

        return Ok(new
        {
            status = true,
            message = "Successfully Deleted"
        });
    }
}
