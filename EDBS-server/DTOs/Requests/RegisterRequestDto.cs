using System.ComponentModel.DataAnnotations;

namespace EDBS_server.DTOs.Requests
{
    /// <summary>
    /// User registration request
    /// </summary>
    public class RegisterRequestDto
    {
        /// <summary>
        /// User email address (must be valid and unique)
        /// </summary>
        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; } = null!;

        /// <summary>
        /// User's full name
        /// </summary>
        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        public string FullName { get; set; } = null!;

        /// <summary>
        /// User password (minimum 6 characters)
        /// </summary>
        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public string Password { get; set; } = null!;
    }
}
