
namespace EDBS_server.DTOs.Requests
{
    public class UpdateProfileRequestDto
    {
        // Thông tin văn bản (có thể sửa hoặc không)
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public IFormFile? Avatar { get; set; }
        public IFormFile? IdCard { get; set; }
    }
}