using DataAccessLayer.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface ITicketBoardRepository : IRepository<TicketBoard>
    {
        Task<TicketBoard?> GetBoardByTitle(string title);
    }
}
