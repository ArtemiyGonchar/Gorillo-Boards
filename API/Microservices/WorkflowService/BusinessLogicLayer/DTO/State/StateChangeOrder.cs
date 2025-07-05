using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO.State
{
    public class StateChangeOrder
    {
        public Guid BoardId { get; set; }
        public Guid StateId { get; set; }
        public int OrderTarget { get; set; }
    }
}
