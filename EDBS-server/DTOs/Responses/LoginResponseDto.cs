namespace EDBS_server.DTOs.Responses
{
    /// <summary>
    /// User information returned after successful login
    /// </summary>
    public class LoginResponseDto
    {
        /// <summary>
        /// User unique identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Employee identification code
        /// </summary>
        public string EmployeeCode { get; set; } = null!;

        /// <summary>
        /// User email address
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// User's full name
        /// </summary>
        public string FullName { get; set; } = null!;

        /// <summary>
        /// User's role name (e.g., "Borrower", "Administrator")
        /// </summary>
        public string? RoleName { get; set; }
    }
}
