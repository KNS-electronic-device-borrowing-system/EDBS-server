using EDBS_server.DTOs.Requests;
using EDBS_server.DTOs.Responses;

namespace EDBS_server.Services
{
    public interface IUserService
    {
        Task<UserProfileDto?> GetUserProfileAsync(int userId);
        Task<UserProfileDto> UpdateProfileAsync(int userId, UpdateProfileRequestDto request);
    }
}
