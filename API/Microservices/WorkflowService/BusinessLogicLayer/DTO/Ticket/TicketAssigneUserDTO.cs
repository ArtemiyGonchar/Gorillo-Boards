using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO.Ticket
{
    public class TicketAssigneUserDTO
    {
        public Guid UserId { get; set; }
        public Guid TicketId { get; set; }
    }
}
