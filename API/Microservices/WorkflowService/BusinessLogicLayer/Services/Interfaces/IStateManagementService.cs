using BusinessLogicLayer.DTO.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface IStateManagementService
    {
        Task<Guid> CreateState(StateCreateDTO stateCreateDTO);
        Task<bool> DeleteState(Guid stateId);

        Task<bool> ChangeOrderState(StateChangeOrder stateChangeOrder);
        Task<Guid> RenameState(StateRenameDTO stateRenameDTO);
        Task<List<StateListDTO>> GetStatesByBoard(Guid boardId);
    }
}
