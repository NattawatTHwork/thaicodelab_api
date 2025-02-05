using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using thaicodelab_api.Services;
using thaicodelab_api.Helpers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EquipmentStatusController : ControllerBase
{
    private readonly EquipmentStatusService _equipmentStatusService;

    public EquipmentStatusController(EquipmentStatusService equipmentStatusService)
    {
        _equipmentStatusService = equipmentStatusService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _equipmentStatusService.GetAllEquipmentStatus();
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
        var equipmentStatus = await _equipmentStatusService.GetEquipmentStatusById(id);
        if (equipmentStatus == null)
            return NotFound(new
            {
                status = false,
                message = "Equipment Status not found"
            });

        return Ok(new
        {
            status = true,
            message = "Successfully Retrieved",
            data = equipmentStatus
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EquipmentStatus equipmentStatus)
    {
        var userId = JwtHelper.GetUserIdFromToken(User);
        equipmentStatus.created_at = DateTime.UtcNow;
        equipmentStatus.created_by = userId;

        await _equipmentStatusService.AddEquipmentStatus(equipmentStatus);

        return CreatedAtAction(nameof(GetById), new { id = equipmentStatus.equipment_status_id }, new
        {
            status = true,
            message = "Successfully Created",
            data = equipmentStatus
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] EquipmentStatus updatedEquipmentStatus)
    {
        var userId = JwtHelper.GetUserIdFromToken(User);
        var equipmentStatus = await _equipmentStatusService.GetEquipmentStatusById(id);

        if (equipmentStatus == null)
            return NotFound(new
            {
                status = false,
                message = "Equipment Status not found"
            });

        await _equipmentStatusService.UpdateEquipmentStatus(equipmentStatus, updatedEquipmentStatus, userId);

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
        var equipmentStatus = await _equipmentStatusService.GetEquipmentStatusById(id);

        if (equipmentStatus == null)
            return NotFound(new
            {
                status = false,
                message = "Equipment Status not found"
            });

        await _equipmentStatusService.SoftDeleteEquipmentStatus(equipmentStatus, userId);

        return Ok(new
        {
            status = true,
            message = "Successfully Deleted"
        });
    }
}
