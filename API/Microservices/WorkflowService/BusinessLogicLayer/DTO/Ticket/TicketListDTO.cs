using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO.Ticket
{
    public class TicketListDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int Order { get; set; }
        public Guid StateId { get; set; }
        public Guid? TicketLabelId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
