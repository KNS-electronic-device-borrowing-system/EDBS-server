using EDBS_server.Models;

namespace EDBS_server.Repositories
{
    public interface IUserRepository
    {
        Task<bool> EmailExistsAsync(string email);
        Task<bool> UsernameExistsAsync(string username);
        Task<Role?> GetRoleByNameAsync(string roleName);
        Task<User?> GetUserByVerificationTokenAsync(string token);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task<User?> GetUserByEmailAsync(string email);
    }
}