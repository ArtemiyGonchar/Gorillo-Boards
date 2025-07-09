using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO.Label
{
    public class LabelCreateDTO
    {
        public Guid BoardId { get; set; }
        public string Title { get; set; }
    }
}
