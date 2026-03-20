using EDBS_server.DTOs;
using EDBS_server.DTOs.Requests;
using EDBS_server.Services;
using Microsoft.AspNetCore.Mvc;

namespace MyApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var result = await _authService.RegisterAsync(request);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.ErrorMessage });
            }

            var verificationLink = Url.Action(nameof(VerifyEmail), "Auth",
                new { token = result.VerificationToken }, Request.Scheme);

            return Ok(new
            {
                message = "Đăng ký thành công. Vui lòng kiểm tra email để xác thực tài khoản.",
                debug_verify_link = verificationLink
            });
        }

        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromQuery] string token)
        {
            var result = await _authService.VerifyEmailAsync(token);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(new { message = "Xác thực email thành công! Bạn đã có thể đăng nhập." });
        }
        [HttpPost("resend-verification")]
        public async Task<IActionResult> ResendVerification([FromBody] ResendVerificationRequestDto request)
        {
            var result = await _authService.ResendVerificationEmailAsync(request.Email);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.ErrorMessage });
            }

            var verificationLink = Url.Action(nameof(VerifyEmail), "Auth",
                new { token = result.VerificationToken }, Request.Scheme);


            return Ok(new
            {
                message = "Link xác thực mới đã được gửi đến email của bạn (có hiệu lực trong 10 phút).",
                debug_verify_link = verificationLink
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await _authService.LoginAsync(request);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(new
            {
                message = "Đăng nhập thành công.",
                user = result.User
            });
        }
    }
}