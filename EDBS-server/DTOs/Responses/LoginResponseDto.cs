namespace EDBS_server.DTOs.Responses
{
    // Class này đại diện cho toàn bộ cục "data" trả về
    public class LoginResponseDto
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public UserDetailDto User { get; set; } = null!;
    }

    // Class này đại diện cho cục "user" nằm bên trong
    public class UserDetailDto
    {
        public int Id { get; set; }
        public string EmployeeCode { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string RoleName { get; set; } = null!;
    }
}