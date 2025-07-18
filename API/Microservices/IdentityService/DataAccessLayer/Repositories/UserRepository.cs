﻿using DataAccessLayer.DatabaseContext;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly IdentityDbContext _ctx;

        public UserRepository(IdentityDbContext ctx) : base(ctx)
        {
            _ctx = ctx;
        }

        public async Task<bool> DeleteByUsername(string username)
        {
            var user = await _ctx.Set<User>().SingleOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                return false;
            }

            _ctx.Set<User>().Remove(user);
            await _ctx.SaveChangesAsync();
            return true;
        }

        public async Task<User?> GetByUsername(string username)
        {
            return await _ctx.Set<User>().SingleOrDefaultAsync(u => u.UserName == username);
        }
    }
}
