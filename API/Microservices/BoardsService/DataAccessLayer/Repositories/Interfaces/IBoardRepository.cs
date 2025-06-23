using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IBoardRepository : IRepository<Board>
    {
        Task<Board?> GetBoardByTitle(string title);

        Task<List<Board>> GetBoardsByRole(UserRoleGlobal role);
    }
}
