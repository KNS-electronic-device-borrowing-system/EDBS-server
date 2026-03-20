using EDBS_server.DTOs.Requests;
using EDBS_server.DTOs.Responses;
using EDBS_server.Models;
using EDBS_server.Repositories;

namespace EDBS_server.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<AuthResultDto> RegisterAsync(RegisterRequestDto request)
        {
            if (await _userRepository.EmailExistsAsync(request.Email))
            {
                return new AuthResultDto { IsSuccess = false, ErrorMessage = "Email này đã được sử dụng." };
            }

            string generatedUsername = request.Email.Split('@')[0];
            if (generatedUsername.Length > 50) generatedUsername = generatedUsername.Substring(0, 50);

            if (await _userRepository.UsernameExistsAsync(generatedUsername))
            {
                generatedUsername += new Random().Next(1000, 9999).ToString();
            }

            var borrowerRole = await _userRepository.GetRoleByNameAsync("Borrower");

            var newUser = new User
            {
                Email = request.Email,
                Username = generatedUsername,
                FullName = request.FullName,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                RoleId = borrowerRole?.Id,
                IsVerified = false,
                VerificationToken = Guid.NewGuid().ToString(),
                VerificationTokenExpiresAt = DateTime.UtcNow.AddMinutes(10)
            };

          
            await _userRepository.AddUserAsync(newUser);

            return new AuthResultDto
            {
                IsSuccess = true,
                VerificationToken = newUser.VerificationToken
            };
        }

        public async Task<AuthResultDto> VerifyEmailAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return new AuthResultDto { IsSuccess = false, ErrorMessage = "Token không hợp lệ." };
            }

            var user = await _userRepository.GetUserByVerificationTokenAsync(token);
            if (user == null)
            {
                return new AuthResultDto { IsSuccess = false, ErrorMessage = "Token xác thực không tồn tại." };
            }

            // THÊM BLOCK NÀY: Kiểm tra xem token đã quá hạn chưa
            if (user.VerificationTokenExpiresAt < DateTime.UtcNow)
            {
                return new AuthResultDto
                {
                    IsSuccess = false,
                    ErrorMessage = "Đường link xác thực đã hết hạn (quá 10 phút). Vui lòng yêu cầu gửi lại email xác thực."
                };
            }

            // Nếu qua được ải trên thì cho phép verify thành công
            user.IsVerified = true;
            user.VerificationToken = null;
            user.VerificationTokenExpiresAt = null; // Xóa luôn thời gian hết hạn cho sạch DB

            await _userRepository.UpdateUserAsync(user);

            return new AuthResultDto { IsSuccess = true };
        }
        public async Task<AuthResultDto> ResendVerificationEmailAsync(string email)
        {
            // 1. Tìm user theo email
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == null)
            {
                return new AuthResultDto { IsSuccess = false, ErrorMessage = "Tài khoản không tồn tại." };
            }

            // 2. Kiểm tra xem user đã xác thực chưa
            if (user.IsVerified == true)
            {
                return new AuthResultDto { IsSuccess = false, ErrorMessage = "Tài khoản này đã được xác thực rồi. Bạn có thể đăng nhập ngay." };
            }

            // 3. Tạo token mới và gia hạn 10 phút tính từ thời điểm hiện tại
            user.VerificationToken = Guid.NewGuid().ToString();
            user.VerificationTokenExpiresAt = DateTime.UtcNow.AddMinutes(10);

            // 4. Lưu thay đổi xuống DB
            await _userRepository.UpdateUserAsync(user);

            return new AuthResultDto
            {
                IsSuccess = true,
                VerificationToken = user.VerificationToken
            };
        }
    }
}