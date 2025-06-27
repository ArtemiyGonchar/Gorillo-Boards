using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class TicketCreateDTO
    {
        public string Title { get; set; }
        public Guid StateId { get; set; }

        public string Description { get; set; }
    }
}
