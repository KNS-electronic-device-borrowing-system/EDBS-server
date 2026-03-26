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
            // 1. Kiểm tra Email
            if (await _userRepository.EmailExistsAsync(request.Email))
            {
                return new AuthResultDto { IsSuccess = false, ErrorMessage = "Email này đã được sử dụng." };
            }

            // 2. Xử lý Role 
            var borrowerRole = await _userRepository.GetRoleByNameAsync("Borrower");
            if (borrowerRole == null)
            {
                return new AuthResultDto { IsSuccess = false, ErrorMessage = "Lỗi hệ thống: Không tìm thấy Role mặc định." };
            }

            // 3. Khởi tạo User mới
            var newUser = new User
            {
                Email = request.Email,
                FullName = request.FullName,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),

                // EmployeeCode là bắt buộc [Required], nếu Request không truyền lên thì ta tự sinh mã random (VD: EMP-12345)
                EmployeeCode = $"EMP-{new Random().Next(10000, 99999)}",

                RoleId = borrowerRole.Id,
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
            if (string.IsNullOrWhiteSpace(token))
            {
                return new AuthResultDto { IsSuccess = false, ErrorMessage = "Token không hợp lệ." };
            }

            var user = await _userRepository.GetUserByVerificationTokenAsync(token);
            if (user == null)
            {
                return new AuthResultDto { IsSuccess = false, ErrorMessage = "Token xác thực không tồn tại hoặc đã được sử dụng." };
            }

            if (user.VerificationTokenExpiresAt < DateTime.UtcNow)
            {
                return new AuthResultDto
                {
                    IsSuccess = false,
                    ErrorMessage = "Đường link xác thực đã hết hạn (quá 10 phút). Vui lòng yêu cầu gửi lại."
                };
            }

            // Xác minh thành công
            user.IsVerified = true;
            user.VerificationToken = null;
            user.VerificationTokenExpiresAt = null;

            await _userRepository.UpdateUserAsync(user);

            return new AuthResultDto { IsSuccess = true };
        }

        public async Task<AuthResultDto> ResendVerificationEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == null)
            {
                return new AuthResultDto { IsSuccess = false, ErrorMessage = "Tài khoản không tồn tại." };
            }

            if (user.IsVerified == true)
            {
                return new AuthResultDto { IsSuccess = false, ErrorMessage = "Tài khoản này đã được xác thực rồi. Bạn có thể đăng nhập ngay." };
            }

            user.VerificationToken = Guid.NewGuid().ToString();
            user.VerificationTokenExpiresAt = DateTime.UtcNow.AddMinutes(10);

            await _userRepository.UpdateUserAsync(user);

            return new AuthResultDto
            {
                IsSuccess = true,
                VerificationToken = user.VerificationToken
            };
        }

        public async Task<AuthResultDto> LoginAsync(LoginRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return new AuthResultDto { IsSuccess = false, ErrorMessage = "Email và mật khẩu không được để trống." };
            }

            // Dùng Email thay cho Username
            var user = await _userRepository.GetUserByEmailAsync(request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return new AuthResultDto { IsSuccess = false, ErrorMessage = "Email hoặc mật khẩu không chính xác." };
            }

            if (user.IsVerified != true)
            {
                return new AuthResultDto { IsSuccess = false, ErrorMessage = "Tài khoản chưa được xác thực. Vui lòng kiểm tra email để xác minh." };
            }

            var loginResponse = new LoginResponseDto
            {
                Id = user.Id,
                EmployeeCode = user.EmployeeCode,
                Email = user.Email,
                FullName = user.FullName,
                RoleName = user.Role.Name
            };

            return new AuthResultDto
            {
                IsSuccess = true,
                User = loginResponse
            };
        }
    }
}