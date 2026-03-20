using EDBS_server.DTOs.Requests;
using EDBS_server.DTOs.Responses;

namespace EDBS_server.Services
{
    public interface IAuthService
    {
        Task<AuthResultDto> RegisterAsync(RegisterRequestDto request);
        Task<AuthResultDto> VerifyEmailAsync(string token);
        Task<AuthResultDto> ResendVerificationEmailAsync(string email);
        Task<AuthResultDto> LoginAsync(LoginRequestDto request);
    }
}