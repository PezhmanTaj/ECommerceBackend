using ECommerceSolution.BLL.DTOs;
using ECommerceSolution.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceSolution.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDTO registrationDto)
        {
            try
            {
                await _userService.RegisterUserAsync(registrationDto);
                return Ok(new { message = "Registration successful" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO loginDto)
        {
            try
            {
                TokenDTO token = await _userService.AuthenticateAsync(loginDto);
                if (token == null)
                    return Unauthorized(new { message = "Username or password is incorrect" });

                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(string userId, [FromBody] ChangePasswordDTO changePasswordDto)
        {
            try
            {
                bool result = await _userService.ChangePasswordAsync(userId, changePasswordDto.OldPassword, changePasswordDto.NewPassword);
                if (!result)
                    return BadRequest(new { message = "Password change failed" });

                return Ok(new { message = "Password changed successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                var userDto = await _userService.GetUserByIdAsync(id);
                if (userDto == null)
                    return NotFound();

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


    }
}
