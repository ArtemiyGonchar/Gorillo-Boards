using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO.Ticket
{
    public class TicketChangeStateDTO
    {
        public Guid Id { get; set; }
        public Guid StateId { get; set; }
    }
}
