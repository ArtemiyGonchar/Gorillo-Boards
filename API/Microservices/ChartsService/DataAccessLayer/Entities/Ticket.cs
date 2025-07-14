using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class Ticket : BaseEntity
    {
        public Guid TicketId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? TicketClose { get; set; }
    }
}
