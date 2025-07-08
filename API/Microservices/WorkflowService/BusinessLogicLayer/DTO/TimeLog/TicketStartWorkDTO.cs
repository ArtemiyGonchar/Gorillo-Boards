using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO.TimeLog
{
    public class TicketStartWorkDTO
    {
        public Guid BoardId { get; set; }
        public Guid TicketId { get; set; }
        public Guid UserId { get; set; }
    }
}
