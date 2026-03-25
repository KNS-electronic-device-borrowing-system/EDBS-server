using EDBS_server.Data;
using EDBS_server.Models;
using Microsoft.EntityFrameworkCore;

namespace EDBS_server.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AssetManagementDbContext _context;

        public UserRepository(AssetManagementDbContext context)
        {
            _context = context;
        }

        public async Task<bool> EmailExistsAsync(string email)
            => await _context.Users.AnyAsync(u => u.Email == email);

        public async Task<Role?> GetRoleByNameAsync(string name)
            => await _context.Roles.FirstOrDefaultAsync(r => r.Name == name);

        public async Task<User?> GetUserByVerificationTokenAsync(string token)
            => await _context.Users.FirstOrDefaultAsync(u => u.VerificationToken == token);

        public async Task<User?> GetUserByEmailAsync(string email)
            => await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}