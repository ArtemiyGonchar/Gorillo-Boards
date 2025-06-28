using BusinessLogicLayer.DTO;
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
    }
}
