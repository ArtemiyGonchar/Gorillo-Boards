using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO.Ticket.Request
{
    public class TicketCreateDTO
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
