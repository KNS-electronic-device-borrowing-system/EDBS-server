using EDBS_server.DTOs.Requests;
using EDBS_server.DTOs.Responses;
using EDBS_server.Repositories;

namespace EDBS_server.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IFileService _fileService;

        public UserService(IUserRepository userRepository, IFileService fileService)
        {
            _userRepository = userRepository;
            _fileService = fileService;
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
        public async Task<UserProfileDto> UpdateProfileAsync(int userId, UpdateProfileRequestDto request)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) throw new Exception("Không tìm thấy người dùng.");
            if (!string.IsNullOrWhiteSpace(request.FullName))
            {
                user.FullName = request.FullName;
            }

            if (!string.IsNullOrWhiteSpace(request.Phone))
            {
                user.Phone = request.Phone;
            }

            if (request.Avatar != null && request.Avatar.Length > 0)
            {
                var avatarUrl = await _fileService.UploadImageAsync(request.Avatar, "avatars");
                user.AvatarUrl = avatarUrl; // Đảm bảo tên thuộc tính trong Model User của bạn khớp với chữ này
            }

      
            if (request.IdCard != null && request.IdCard.Length > 0)
            {
                var idCardUrl = await _fileService.UploadImageAsync(request.IdCard, "idcards");
                user.IdCardImageUrl = idCardUrl; // Map đúng tên cột IdCardImageUrl mà bạn đang dùng
            }

            await _userRepository.UpdateUserAsync(user);

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
