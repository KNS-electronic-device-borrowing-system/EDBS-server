using Asp.Versioning;
using EDBS_server.DTOs.Responses;
using EDBS_server.Repositories;
using EDBS_server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EDBS_server.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        // Tiêm Service vào Controller thay vì Repository
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return Unauthorized(ApiResponse<object>.Fail("Token không hợp lệ."));
            }

            var profile = await _userService.GetUserProfileAsync(userId);

            if (profile == null)
            {
                return NotFound(ApiResponse<object>.Fail("Không tìm thấy thông tin người dùng trong hệ thống."));
            }

            return Ok(ApiResponse<UserProfileDto>.Succeed(
                profile,
                "Lấy thông tin cá nhân thành công."
            ));
        }
    }
}