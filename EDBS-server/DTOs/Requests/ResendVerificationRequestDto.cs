using System.ComponentModel.DataAnnotations;

namespace EDBS_server.DTOs
{
    public class ResendVerificationRequestDto
    {
        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; } = null!;
    }
}