using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO.Ticket
{
    public class TicketRenameDTO
    {
        public Guid BoardId { get; set; }
        public Guid Id { get; set; }
        public string Title { get; set; }
    }
}
