using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using thaicodelab_api.Helpers;
using thaicodelab_api.Services;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _userService.GetAllUsers();
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
        var user = await _userService.GetUserProfileById(id);
        if (user == null)
            return NotFound(new
            {
                status = false,
                message = "User not found"
            });

        return Ok(new
        {
            status = true,
            message = "Successfully Retrieved",
            data = user
        });
    }

    [HttpGet("Name")]
    [AllowAnonymous]
    public async Task<IActionResult> GetName()
    {
        try
        {
            // ดึง user_id จาก token
            var userId = JwtHelper.GetUserIdFromToken(User);

            // ใช้ user_id เพื่อเรียกข้อมูลผู้ใช้
            var user = await _userService.GetUserNameById(userId);

            if (user == null)
            {
                return NotFound(new
                {
                    status = false,
                    message = "User not found"
                });
            }

            return Ok(new
            {
                status = true,
                message = "Successfully Retrieved",
                data = user
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new
            {
                status = false,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                status = false,
                message = "An error occurred while retrieving the profile.",
                error = ex.Message
            });
        }
    }

    [HttpGet("Profile")]
    public async Task<IActionResult> GetProfile()
    {
        try
        {
            // ดึง user_id จาก token
            var userId = JwtHelper.GetUserIdFromToken(User);

            // ใช้ user_id เพื่อเรียกข้อมูลผู้ใช้
            var user = await _userService.GetUserProfileById(userId);

            if (user == null)
            {
                return NotFound(new
                {
                    status = false,
                    message = "User not found"
                });
            }

            return Ok(new
            {
                status = true,
                message = "Successfully Retrieved",
                data = user
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new
            {
                status = false,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                status = false,
                message = "An error occurred while retrieving the profile.",
                error = ex.Message
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserRequest UserRequest)
    {
        if (UserRequest.user_password != UserRequest.confirm_user_password)
        {
            return BadRequest(new
            {
                status = false,
                message = "Password and Confirm Password do not match"
            });
        }
        var emailExists = await _userService.CheckIfEmailExists(UserRequest.email);
        if (emailExists)
        {
            return Conflict(new
            {
                status = false,
                message = "Email already exists"
            });
        }
        var userId = JwtHelper.GetUserIdFromToken(User);
        UserRequest.user_code = await _userService.GenerateUserCode();
        // UserRequest.created_at = DateTime.UtcNow;
        // UserRequest.created_by = userId;
        // UserRequest.updated_by = userId;

        await _userService.AddUser(UserRequest, userId);

        return CreatedAtAction(nameof(GetById), new { id = UserRequest.user_id }, new
        {
            status = true,
            message = "Successfully Created",
            data = UserRequest
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UserRequest UserRequest)
    {
        var userId = JwtHelper.GetUserIdFromToken(User);
        var user = await _userService.GetUserById(id);
        if (user == null)
        {
            return NotFound(new
            {
                status = false,
                message = "User not found"
            });
        }

        var emailExists = await _userService.CheckIfEmailExistsExceptCurrent(UserRequest.email, id);
        if (emailExists)
        {
            return Conflict(new
            {
                status = false,
                message = "Email already exists"
            });
        }

        await _userService.UpdateUserProfile(user, UserRequest, userId);

        return Ok(new
        {
            status = true,
            message = "Successfully Updated"
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id == 1)
        {
            return StatusCode(403, new
            {
                status = false,
                message = "Cannot delete this user"
            });
        }

        var userId = JwtHelper.GetUserIdFromToken(User);
        var user = await _userService.GetUserById(id);

        if (user == null)
            return NotFound(new
            {
                status = false,
                message = "User not found"
            });

        await _userService.SoftDeleteUser(user, userId);

        return Ok(new
        {
            status = true,
            message = "Successfully Deleted"
        });
    }

    [HttpPut("Update-Email/{id}")]
    public async Task<IActionResult> UpdateEmail(int id, [FromBody] UserRequest UserRequest)
    {
        var userId = JwtHelper.GetUserIdFromToken(User);
        var user = await _userService.GetUserById(id);

        if (user == null)
            return NotFound(new
            {
                status = false,
                message = "User not found"
            });

        var emailExists = await _userService.CheckIfEmailExistsExceptCurrent(UserRequest.email, id);
        if (emailExists)
        {
            return Conflict(new
            {
                status = false,
                message = "Email already exists"
            });
        }


        await _userService.UpdateUserEmail(user, UserRequest.email, userId);

        return Ok(new
        {
            status = true,
            message = "Password successfully updated"
        });
    }

    [HttpPut("Update-Password/{id}")]
    public async Task<IActionResult> UpdatePassword(int id, [FromBody] UserRequest UserRequest)
    {
        var userId = JwtHelper.GetUserIdFromToken(User);
        var user = await _userService.GetUserById(id);

        if (user == null)
        {
            return NotFound(new
            {
                status = false,
                message = "User not found"
            });
        }

        if (UserRequest.user_password != UserRequest.confirm_user_password)
        {
            return BadRequest(new
            {
                status = false,
                message = "Password and Confirm Password do not match"
            });
        }

        await _userService.UpdateUserPassword(user, UserRequest.user_password, userId);

        return Ok(new
        {
            status = true,
            message = "Password successfully updated"
        });
    }

    [HttpPut("UpdateProfile/{id}")]
    public async Task<IActionResult> UpdateProfile(int id, [FromBody] UserRequest UserRequest)
    {
        var userId = JwtHelper.GetUserIdFromToken(User);
        var user = await _userService.GetUserById(id);
        if (user == null)
        {
            return NotFound(new
            {
                status = false,
                message = "User not found"
            });
        }

        var emailExists = await _userService.CheckIfEmailExistsExceptCurrent(UserRequest.email, id);
        if (emailExists)
        {
            return Conflict(new
            {
                status = false,
                message = "Email already exists"
            });
        }

        await _userService.UpdateUserProfile(user, UserRequest, userId);

        return Ok(new
        {
            status = true,
            message = "Profile successfully updated"
        });
    }
}
