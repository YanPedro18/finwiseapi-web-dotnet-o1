
using DataAccess.Context;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly MainContext _dbContext;

        //injeção de deps, do banco.
        public UserRepository(MainContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> PostCreateUser(User setting)
        {
            await _dbContext.User.AddAsync(setting);
            await _dbContext.SaveChangesAsync();
            return setting;
        }

        public async Task<List<User>> GetAllUser()
        {
            return await _dbContext.User.ToListAsync();
        }

        public async Task<User> GetUserById(Guid Id)
        {
            return await _dbContext.User.FirstOrDefaultAsync(s => s.Id == Id);
        }

        public async Task UpdateUserAsync(User setting)
        {
            _dbContext.User.Update(setting);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteUser(Guid id)
        {
            var item = await _dbContext.User.FindAsync(id);

            if (item != null)
            {
                _dbContext.User.Remove(item);

                await _dbContext.SaveChangesAsync();
            }

        }

        public async Task<User> GetUserByUsername(string userName)
        {
            return await _dbContext.User.FirstOrDefaultAsync(s => s.Username == userName);
        }

    }
}
