using EDBS_server.DTOs.Responses;
using EDBS_server.Repositories;

namespace EDBS_server.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserProfileDto?> GetUserProfileAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                return null;
            }

            return new UserProfileDto
            {
                Id = user.Id,
                EmployeeCode = user.EmployeeCode,
                Email = user.Email,
                FullName = user.FullName,
                RoleName = user.Role.Name,
                Phone = user.Phone,
                AvatarUrl = user.AvatarUrl,
                IdCardImageUrl = user.IdCardImageUrl
            };
        }
    }
}
