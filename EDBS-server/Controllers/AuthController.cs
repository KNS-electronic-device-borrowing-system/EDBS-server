using Asp.Versioning;
using EDBS_server.DTOs;
using EDBS_server.DTOs.Requests;
using EDBS_server.DTOs.Responses; // Thêm dòng này
using EDBS_server.Services;
using Microsoft.AspNetCore.Mvc;

namespace EDBS_server.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthService authService, IEmailService emailService, IConfiguration configuration)
        {
            _authService = authService;
            _emailService = emailService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var result = await _authService.RegisterAsync(request);

            if (!result.IsSuccess)
            {
                return BadRequest(ApiResponse<object>.Fail(result.ErrorMessage!));
            }

            // Sinh link trỏ về Frontend thay vì Backend
            var frontendUrl = _configuration["FrontendUrl"] ?? "http://localhost:5173";
            var verificationLink = $"{frontendUrl}/verify-email?token={result.VerificationToken}";

            string emailBody = $@"
                <h3>Chào mừng bạn đến với Hệ thống mượn trả thiết bị!</h3>
                <p>Vui lòng click vào đường link dưới đây để xác thực tài khoản (Link có hiệu lực trong 10 phút):</p>
                <a href='{verificationLink}' style='padding:10px 15px; background-color:#28a745; color:white; text-decoration:none; border-radius:5px;'>Xác thực Email</a>
                <p>Hoặc copy link này dán vào trình duyệt: <br> {verificationLink}</p>";

            await _emailService.SendEmailAsync(request.Email, "Xác thực tài khoản của bạn", emailBody);

            return Ok(ApiResponse<object>.Succeed(
                null,
                "Đăng ký thành công. Vui lòng kiểm tra email để xác thực tài khoản."
            ));
        }

        // ĐỔI THÀNH POST VÀ NHẬN QUA BODY CHO BẢO MẬT
        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequestDto request)
        {
            var result = await _authService.VerifyEmailAsync(request.Token);

            if (!result.IsSuccess)
            {
                return BadRequest(ApiResponse<object>.Fail(result.ErrorMessage!));
            }

            return Ok(ApiResponse<object>.Succeed(null, "Xác thực email thành công!"));
        }

        [HttpPost("resend-verification")]
        public async Task<IActionResult> ResendVerification([FromBody] ResendVerificationRequestDto request)
        {
            var result = await _authService.ResendVerificationEmailAsync(request.Email);

            if (!result.IsSuccess)
            {
                return BadRequest(ApiResponse<object>.Fail(result.ErrorMessage!));
            }

            var frontendUrl = _configuration["FrontendUrl"] ?? "http://localhost:5173";
            var verificationLink = $"{frontendUrl}/verify-email?token={result.VerificationToken}";

            string emailBody = $@"
                <h3>Yêu cầu cấp lại link xác thực!</h3>
                <p>Bạn vừa yêu cầu cấp lại link xác thực. Vui lòng click vào nút bên dưới (Link có hiệu lực trong 10 phút):</p>
                <a href='{verificationLink}' style='padding:10px 15px; background-color:#007bff; color:white; text-decoration:none; border-radius:5px;'>Xác thực Email ngay</a>";

            await _emailService.SendEmailAsync(request.Email, "Gửi lại link xác thực tài khoản", emailBody);

            return Ok(ApiResponse<object>.Succeed(
                null,
                "Link xác thực mới đã được gửi đến email của bạn."
            ));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await _authService.LoginAsync(request);

            if (!result.IsSuccess)
            {
                return BadRequest(ApiResponse<object>.Fail(result.ErrorMessage!));
            }

            // TODO: Nơi này sẽ được update để sinh accessToken (JWT) 
            return Ok(ApiResponse<object>.Succeed(
                new { user = result.User },
                "Đăng nhập thành công."
            ));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequestDto request)
        {
            var result = await _authService.RefreshTokenAsync(request);

            if (!result.IsSuccess)
            {
                return BadRequest(ApiResponse<object>.Fail(result.ErrorMessage!));
            }

            return Ok(ApiResponse<LoginResponseDto>.Succeed(
                result.User!,
                "Làm mới Token thành công."
            ));
        }
    }
}