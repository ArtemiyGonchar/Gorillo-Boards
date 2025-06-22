using DataAccessLayer.DatabaseContext;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class BoardRoleRepository : Repository<BoardRole>, IBoardRoleRepository
    {
        private readonly BoardsDbContext _ctx;
        public BoardRoleRepository(BoardsDbContext ctx) : base(ctx)
        {
            _ctx = ctx;
        }

        public async Task<bool> BoardHasSuchRole(Guid boardId, UserRoleGlobal role)
        {
            return await _ctx.Set<BoardRole>().AnyAsync(r => r.BoardId == boardId && r.Role == role);
        }

        public async Task<bool> DeleteRoleByBoardId(Guid boardId, UserRoleGlobal role)
        {
            var entity = await _ctx.Set<BoardRole>().FirstOrDefaultAsync(r => r.BoardId == boardId && r.Role == role);

            if (entity == null)
            {
                return false;
            }

            _ctx.Set<BoardRole>().Remove(entity);
            await _ctx.SaveChangesAsync();
            return true;
        }
    }
}
