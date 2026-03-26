namespace EDBS_server.DTOs.Requests
{
    public class TokenRequestDto
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
