using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IUserRepository
    {
        Task<User> PostCreateUser(User setting);
        Task<List<User>> GetAllUser();
        Task<User> GetUserById(Guid Id);
        Task UpdateUserAsync(User setting);
        Task DeleteUser(Guid id);
        Task<User> GetUserByUsername(string userName);
    }
}
