using DataAccessLayer.DatabaseContext;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class BoardRepository : Repository<Board>, IBoardRepository
    {
        private readonly BoardsDbContext _ctx;
        public BoardRepository(BoardsDbContext ctx) : base(ctx)
        {
            _ctx = ctx;
        }

        public async Task<Board?> GetBoardByTitle(string title)
        {
            return await _ctx.Set<Board>()
                .SingleOrDefaultAsync(b => b.Title == title);
        }

        public async Task<List<Board>> GetBoardsByRole(UserRoleGlobal role)
        {
            return await _ctx.Set<Board>()
                .Include(b => b.AllowedRoles)
                .Where(b => b.AllowedRoles.Any(r => r.Role == role)).ToListAsync();
        }
    }
}
