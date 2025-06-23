using BusinessLogicLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface IBoardsManagmentService
    {
        Task<Guid> CreateBoardAsync(BoardCreateDTO boardCreateDTO);

        Task<bool> DeleteBoardAsync(string title);

        Task<Guid> CreateBoardRole(BoardCreateAllowedRoleDTO boardCreateAllowedRoleDTO);
        Task<bool> DeleteBoardRole(BoardDeleteAllowedRoleDTO boardDeleteAllowedRoleDTO);
    }
}
