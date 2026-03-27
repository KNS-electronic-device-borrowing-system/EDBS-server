using EDBS_server.DTOs.Responses;

namespace EDBS_server.Services
{
    public interface IUserService
    {
        Task<UserProfileDto?> GetUserProfileAsync(int userId);
    }
}
