using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entites
{
    public class TicketLabel : BaseEntity
    {
        public string Title { get; set; }

        public Guid? BoardId { get; set; }
        public TicketBoard Board { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}
