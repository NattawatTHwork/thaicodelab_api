using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using thaicodelab_api.Helpers;
using thaicodelab_api.Services;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RolePermissionController : ControllerBase
{
    private readonly RolePermissionService _rolePermissionService;
    private readonly UserService _userService;

    public RolePermissionController(RolePermissionService rolePermissionService, UserService userService)
    {
        _rolePermissionService = rolePermissionService;
        _userService = userService;
    }

    // [HttpPost]
    // public async Task<IActionResult> CreateMultiple([FromBody] dynamic request)
    // {
    //     var userId = JwtHelper.GetUserIdFromToken(User);
    //     int roleId = request.role_id;
    //     List<int> permissionIds = request.permission_id.ToObject<List<int>>();

    //     await _rolePermissionService.CreateMultipleRolePermissions(roleId, permissionIds, userId);

    //     return Ok(new
    //     {
    //         status = true,
    //         message = "Role-Permissions updated successfully"
    //     });
    // }
    [HttpPost]
    public async Task<IActionResult> CreateMultiple([FromBody] RolePermissionRequest request)
    {
        var userId = JwtHelper.GetUserIdFromToken(User);

        await _rolePermissionService.CreateMultipleRolePermissions(
            request.role_id,
            request.permission_ids,
            userId
        );

        return Ok(new
        {
            status = true,
            message = "Role-Permissions updated successfully"
        });
    }

    [AllowAnonymous]
    [HttpGet("permissionbytokenuser")]
    public async Task<IActionResult> GetPermissionsByUserId()
    {
        var userId = JwtHelper.GetUserIdFromToken(User);
        var roleId = await _userService.GetRoleIdByUserId(userId);

        if (roleId == null)
        {
            return NotFound(new
            {
                status = false,
                message = "Role not found for this user"
            });
        }

        var permissions = await _rolePermissionService.GetPermissionsByRoleId(roleId.Value);

        return Ok(new
        {
            status = true,
            message = "Successfully Retrieved",
            data = permissions
        });
    }

    [HttpGet("permissions/{roleId}")]
    public async Task<IActionResult> GetPermissionsByRoleId(int roleId)
    {
        var permissions = await _rolePermissionService.GetPermissionsByRoleId(roleId);
        return Ok(new
        {
            status = true,
            message = "Successfully Retrieved",
            data = permissions
        });
    }

}
