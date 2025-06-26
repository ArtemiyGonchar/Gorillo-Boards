using BusinessLogicLayer.DTO;
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

        Task<bool> ChangeOrderState(Guid stateId, int targetOrder);
    }
}
