using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO.Ticket
{
    public class TicketCloseDTO
    {
        public Guid BoardId { get; set; }
        public Guid TicketId { get; set; }
        public Guid UserRequestor { get; set; }
    }
}
