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
        private readonly IEmailService _emailService;

        public AuthController(IAuthService authService, IEmailService emailService)
        {
            _authService = authService;
            _emailService = emailService;
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

            // 1. TẠO NỘI DUNG VÀ GỬI EMAIL TRƯỚC
            string emailBody = $@"
                <h3>Chào mừng bạn đến với Hệ thống mượn trả thiết bị!</h3>
                <p>Vui lòng click vào đường link dưới đây để xác thực tài khoản (Link có hiệu lực trong 10 phút):</p>
                <a href='{verificationLink}' style='padding:10px 15px; background-color:#28a745; color:white; text-decoration:none; border-radius:5px;'>Xác thực Email</a>
                <p>Hoặc copy link này dán vào trình duyệt: <br> {verificationLink}</p>";

            await _emailService.SendEmailAsync(request.Email, "Xác thực tài khoản của bạn", emailBody);

            // 2. SAU ĐÓ MỚI TRẢ VỀ KẾT QUẢ CHO CLIENT
            return Ok(new
            {
                message = "Đăng ký thành công. Vui lòng kiểm tra email để xác thực tài khoản."
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

            return Ok(new { message = "Xác thực email thành công!" });
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

            string emailBody = $@"
                <h3>Yêu cầu cấp lại link xác thực!</h3>
                <p>Bạn vừa yêu cầu cấp lại link xác thực. Vui lòng click vào nút bên dưới (Link có hiệu lực trong 10 phút):</p>
                <a href='{verificationLink}' style='padding:10px 15px; background-color:#007bff; color:white; text-decoration:none; border-radius:5px;'>Xác thực Email ngay</a>
                <p>Hoặc copy link này dán vào trình duyệt: <br> {verificationLink}</p>";

            await _emailService.SendEmailAsync(request.Email, "Gửi lại link xác thực tài khoản", emailBody);

            return Ok(new
            {
                message = "Link xác thực mới đã được gửi đến email của bạn (có hiệu lực trong 10 phút)."
            });
        }
    }
}