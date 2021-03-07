using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VideoHosting.Domain.Entities;

namespace VideoHosting.Abstractions.Repositories
{
    public interface IUserRepository
    {       
        Task<User> GetUserById(string id);

        Task<IEnumerable<User>> GetUsers();

        Task<IEnumerable<User>> GetUserBySubName(string str);
    }
}
