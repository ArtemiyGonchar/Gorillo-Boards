using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class BoardCreatedDTO
    {
        Guid Id { get; set; }
        public string Title { get; set; }
    }
}
