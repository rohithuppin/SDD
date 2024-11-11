using Microsoft.EntityFrameworkCore;
using SDDUserApi.Data.DBConfiguration;
using SDDUserApi.Data.Model;

namespace SDDUserApi.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.Where(s => s.IsActive == true).ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.Where(u => u.UserId == id && u.IsActive == true)
            .FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.IsActive == true);
        }

        public async Task AddUserAsync(User user)
        {
            if (user != null)
            {
                user.UserId = 0;
            }
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {           
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        //public async Task DeleteUserAsync(int id)
        //{
        //    var user = await _context.Users.FindAsync(id);
        //    if (user != null)
        //    {
        //        _context.Users.Remove(user);
        //        await _context.SaveChangesAsync();
        //    }
        //}
    }
}
