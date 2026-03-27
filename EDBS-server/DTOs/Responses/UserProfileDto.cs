namespace EDBS_server.DTOs.Responses
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string EmployeeCode { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? Phone { get; set; }

        public string? AvatarUrl { get; set; }
        public string? IdCardImageUrl { get; set; }
        public string RoleName { get; set; } = null!;
    }
}
