using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO.Ticket
{
    public class TicketCreateDTO
    {
        public Guid BoardId { get; set; }
        public string Title { get; set; }
        public Guid StateId { get; set; }
        public Guid? UserRequestor { get; set; }
        public string? Description { get; set; }
    }
}
