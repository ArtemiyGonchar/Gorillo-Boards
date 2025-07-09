using BusinessLogicLayer.DTO.Label;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface IFilteringService
    {
        Task<Guid> CreateLabel(LabelCreateDTO labelCreateDTO);
        Task<Guid> AddLabelToTicket(AddLabelToTicketDTO addLabelToTicket);
        Task<Guid> DeleteLabelFromTicket(DeleteLabelFromTicketDTO deleteLabelFromTicket);
        Task<List<LabelByBoardDTO>> GetLabelsByBoard(GetLabelsByBoardDTO getLabelsByBoardDTO);
    }
}
