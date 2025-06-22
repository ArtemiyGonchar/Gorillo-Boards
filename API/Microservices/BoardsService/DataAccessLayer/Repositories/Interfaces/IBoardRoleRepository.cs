using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IBoardRoleRepository : IRepository<BoardRole>
    {
        Task<bool> BoardHasSuchRole(Guid boardId, UserRoleGlobal role);
    }
}
