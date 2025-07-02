using BusinessLogicLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface IBoardAccessService
    {
        Task<List<BoardResponseDTO>> GetBoards(string role);
        Task<bool> HasAccess(Guid boardId, string role);
        Task<BoardResponseDTO> GetBoardById(Guid boardId);
    }
}
