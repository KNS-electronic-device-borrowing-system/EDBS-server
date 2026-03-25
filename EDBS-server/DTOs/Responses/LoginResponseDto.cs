namespace EDBS_server.DTOs.Responses
{
    public class LoginResponseDto
    {
        public int Id { get; set; }
        public string EmployeeCode { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? roleName { get; set; }
    }
}
