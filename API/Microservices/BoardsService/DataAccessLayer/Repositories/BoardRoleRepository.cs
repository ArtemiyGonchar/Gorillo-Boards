using DataAccessLayer.DatabaseContext;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class BoardRoleRepository : Repository<BoardRole>, IBoardRoleRepository
    {
        public BoardRoleRepository(BoardsDbContext ctx) : base(ctx) { }

    }
}
