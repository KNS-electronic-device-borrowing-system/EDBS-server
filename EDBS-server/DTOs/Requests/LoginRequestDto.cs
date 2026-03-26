namespace EDBS_server.DTOs.Requests
{
    /// <summary>
    /// User login request
    /// </summary>
    public class LoginRequestDto
    {
        /// <summary>
        /// User email address
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// User password
        /// </summary>
        public string Password { get; set; } = null!;
    }
}
