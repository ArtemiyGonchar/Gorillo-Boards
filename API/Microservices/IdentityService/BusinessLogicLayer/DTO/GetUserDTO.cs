using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class GetUserDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
    }
}
