using EDBS_server.DTOs;
using EDBS_server.DTOs.Requests;
using EDBS_server.Services;
using Microsoft.AspNetCore.Mvc;

namespace EDBS_server.Controllers
{
    /// <summary>
    /// Authentication endpoints for user registration, login, and email verification
    /// </summary>
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

        /// <summary>
        /// Register a new user account
        /// </summary>
        /// <remarks>
        /// Creates a new user account with the provided email, password, and full name.
        /// A verification email will be sent to the provided email address.
        /// </remarks>
        /// <param name="request">User registration information</param>
        /// <returns>Registration status and verification token</returns>
        /// <response code="200">User registered successfully. Verification email has been sent.</response>
        /// <response code="400">Registration failed due to validation errors or duplicate email</response>
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

            string emailBody = $@"
                <h3>Chào mừng bạn đến với Hệ thống mượn trả thiết bị!</h3>
                <p>Vui lòng click vào đường link dưới đây để xác thực tài khoản (Link có hiệu lực trong 10 phút):</p>
                <a href='{verificationLink}' style='padding:10px 15px; background-color:#28a745; color:white; text-decoration:none; border-radius:5px;'>Xác thực Email</a>
                <p>Hoặc copy link này dán vào trình duyệt: <br> {verificationLink}</p>";

            await _emailService.SendEmailAsync(request.Email, "Xác thực tài khoản của bạn", emailBody);

            return Ok(new
            {
                message = "Đăng ký thành công. Vui lòng kiểm tra email để xác thực tài khoản."
            });
        }

        /// <summary>
        /// Verify user email address
        /// </summary>
        /// <remarks>
        /// Validates and verifies a user's email address using the verification token sent to their email.
        /// </remarks>
        /// <param name="token">Email verification token</param>
        /// <returns>Verification status</returns>
        /// <response code="200">Email verified successfully</response>
        /// <response code="400">Verification failed - invalid or expired token</response>
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

        /// <summary>
        /// Resend email verification link
        /// </summary>
        /// <remarks>
        /// Sends a new verification email link to the user if they didn't receive the initial one or if it expired.
        /// </remarks>
        /// <param name="request">Email address to resend verification to</param>
        /// <returns>Status of resend operation</returns>
        /// <response code="200">Verification email resent successfully</response>
        /// <response code="400">Resend failed - user not found or already verified</response>
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

        /// <summary>
        /// User login
        /// </summary>
        /// <remarks>
        /// Authenticates a user with their email and password. User must have verified their email before login.
        /// </remarks>
        /// <param name="request">User credentials (email and password)</param>
        /// <returns>Authentication result with user information</returns>
        /// <response code="200">Login successful. Returns user information and role</response>
        /// <response code="400">Login failed - invalid credentials or unverified account</response>
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