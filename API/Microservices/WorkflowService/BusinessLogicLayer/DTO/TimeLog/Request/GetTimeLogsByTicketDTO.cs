using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO.TimeLog.Request
{
    public class GetTimeLogsByTicketDTO
    {
        public Guid TicketId { get; set; }
    }
}
