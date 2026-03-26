using EDBS_server.Models;
using System.Threading.Tasks;

namespace EDBS_server.Repositories
{
    public interface IUserRepository
    {
        Task<bool> EmailExistsAsync(string email);

        Task<Role?> GetRoleByNameAsync(string name);
        Task<User?> GetUserByVerificationTokenAsync(string token);
        Task<User?> GetUserByEmailAsync(string email);

        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task<User> GetUserByIdAsync(int userId);
    }
}