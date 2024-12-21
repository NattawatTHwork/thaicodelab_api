using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using thaicodelab_api.Services;

[Route("api/[controller]")]
[ApiController]
public class RegisterController : ControllerBase
{
    private readonly RegisterService _registerService;

    public RegisterController(RegisterService registerService)
    {
        _registerService = registerService;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
    {
        // ตรวจสอบ ModelState
        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                status = false,
                message = "Invalid data",
                errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
            });
        }

        try
        {
            // ตรวจสอบว่า Email ซ้ำหรือไม่
            var isEmailExists = await _registerService.CheckIfEmailExists(request.email);
            if (isEmailExists)
            {
                return BadRequest(new
                {
                    status = false,
                    message = "Email already exists"
                });
            }

            // สร้าง User Code และลงทะเบียนผู้ใช้
            request.user_code = await _registerService.GenerateUserCode();
            var newUser = await _registerService.RegisterUser(request);

            // ตอบกลับเมื่อการลงทะเบียนสำเร็จ
            return CreatedAtAction(nameof(Register), new { id = newUser.user_id }, new
            {
                status = true,
                message = "User registered successfully",
                data = new
                {
                    user_id = newUser.user_id,
                    user_code = newUser.user_code,
                    email = newUser.email
                }
            });
        }
        catch (Exception ex)
        {
            // จัดการข้อผิดพลาดทั่วไป
            return StatusCode(500, new
            {
                status = false,
                message = "An error occurred while processing your request",
                error = ex.Message
            });
        }
    }
}
