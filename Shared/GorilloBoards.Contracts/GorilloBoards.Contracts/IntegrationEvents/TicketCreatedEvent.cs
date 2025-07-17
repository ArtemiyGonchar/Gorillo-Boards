using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GorilloBoards.Contracts.IntegrationEvents
{
    public class TicketCreatedEvent
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid BoardId { get; set; }
    }
}
