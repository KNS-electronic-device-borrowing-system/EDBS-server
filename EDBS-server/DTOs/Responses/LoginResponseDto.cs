namespace EDBS_server.DTOs.Responses
{
    public class LoginResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FullName { get; set; }
        public int? RoleId { get; set; }
    }
}
