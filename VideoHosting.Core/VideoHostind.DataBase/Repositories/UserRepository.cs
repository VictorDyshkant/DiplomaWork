using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VideoHosting.Abstractions.Repositories;
using VideoHosting.Domain.Entities;

namespace VideoHosting.DataBase.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataBaseContext _context;

        public UserRepository(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserById(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUserBySubName(string str)
        {
            str = str.ToLower();
            return await _context.Users
                                 .Where(x => x.Name.ToLower().Contains(str) || x.Surname.ToLower().Contains(str) || (x.Name + x.Surname)
                                                                                                                    .ToLower()
                                                                                                                    .Contains(str))
                                 .ToListAsync();
        }
    }
}
