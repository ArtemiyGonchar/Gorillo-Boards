using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO.Ticket
{
    public class TicketUserAssignedDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
    }
}
