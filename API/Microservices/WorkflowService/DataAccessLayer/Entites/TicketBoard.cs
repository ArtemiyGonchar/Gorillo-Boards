using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entites
{
    public class TicketBoard : BaseEntity
    {
        public string Title { get; set; }

        public ICollection<State> States { get; set; }
        public ICollection<TicketLabel> Labels { get; set; }
    }
}
