namespace EDBS_server.DTOs.Responses
{
    public class AuthResultDto
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public string? VerificationToken { get; set; }
    }
}
