using EDBS_server.DTOs.Requests;
using EDBS_server.DTOs.Responses;
using EDBS_server.Models;
using EDBS_server.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Cryptography;

namespace EDBS_server.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
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

        private string CreateJwtToken(User user)
        {
            // 1. Tạo các Claims
            var authClaims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Name, user.FullName),
        new Claim(ClaimTypes.Role, user.Role.Name),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var secretKey = _configuration["JwtSettings:SecretKey"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("Chưa cấu hình JWT Secret Key!");
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var expiryDays = Convert.ToInt32(_configuration["JwtSettings:ExpiryDays"] ?? "7");

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:ValidIssuer"],
                audience: _configuration["JwtSettings:ValidAudience"],
                expires: DateTime.UtcNow.AddDays(expiryDays),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<AuthResultDto> LoginAsync(LoginRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return new AuthResultDto { IsSuccess = false, ErrorMessage = "Email và mật khẩu không được để trống." };
            }

            var user = await _userRepository.GetUserByEmailAsync(request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return new AuthResultDto { IsSuccess = false, ErrorMessage = "Email hoặc mật khẩu không chính xác." };
            }

            if (user.IsVerified != true)
            {
                return new AuthResultDto { IsSuccess = false, ErrorMessage = "Tài khoản chưa được xác thực. Vui lòng kiểm tra email để xác minh." };
            }

            var accessToken = CreateJwtToken(user);

            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userRepository.UpdateUserAsync(user);
            return new AuthResultDto
            {
                IsSuccess = true,
                User = new LoginResponseDto
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    User = new UserDetailDto
                    {
                        Id = user.Id,
                        EmployeeCode = user.EmployeeCode,
                        Email = user.Email,
                        FullName = user.FullName,
                        RoleName = user.Role.Name
                    }
                }
            };
        }

        public async Task<AuthResultDto> RefreshTokenAsync(TokenRequestDto request)
        {
            // 1. Giải mã cái AccessToken cũ (dù nó đã hết hạn) để lấy ra UserId
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!)),
                ValidateLifetime = false // BỎ QUA KIỂM TRA HẾT HẠN (Vì nó chắc chắn đã hết hạn rồi)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(request.AccessToken, tokenValidationParameters, out SecurityToken securityToken);

            // Kiểm tra xem token cũ có đúng chuẩn thuật toán của mình không
            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return new AuthResultDto { IsSuccess = false, ErrorMessage = "Token không hợp lệ" };
            }

            // Lấy UserId từ Token cũ
            var userIdString = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return new AuthResultDto { IsSuccess = false, ErrorMessage = "Dữ liệu Token bị hỏng" };
            }

            // 2. Tìm User trong Database và đối chiếu Refresh Token
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                // Nếu không khớp hoặc thẻ rác đã hết hạn -> Bắt buộc Frontend phải đẩy user ra màn hình Login
                return new AuthResultDto { IsSuccess = false, ErrorMessage = "Phiên đăng nhập không hợp lệ hoặc đã hết hạn. Vui lòng đăng nhập lại." };
            }

            // 3. Mọi thứ hợp lệ -> Tạo 1 cặp Access Token và Refresh Token mới tinh
            var newAccessToken = CreateJwtToken(user); // Tách đoạn tạo JWT cũ thành 1 hàm riêng cho gọn nhé
            var newRefreshToken = GenerateRefreshToken();

            // Cập nhật thẻ rác mới vào DB
            user.RefreshToken = newRefreshToken;
            await _userRepository.UpdateUserAsync(user);

            return new AuthResultDto
            {
                IsSuccess = true,
                User = new LoginResponseDto
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    // Không cần trả lại cục UserDetailDto nếu không muốn, Frontend đã có rồi
                }
            };
        }

    }

}