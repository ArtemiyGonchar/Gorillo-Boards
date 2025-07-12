using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO.State
{
    public class DeleteStateDTO
    {
        public Guid BoardId { get; set; }
        public Guid Id { get; set; }
    }
}
