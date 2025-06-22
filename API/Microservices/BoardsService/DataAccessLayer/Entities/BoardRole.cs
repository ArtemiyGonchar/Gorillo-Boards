using DataAccessLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class BoardRole : BaseEntity
    {
        public Guid BoardId { get; set; }
        public Board Board { get; set; }

        public UserRoleGlobal Role { get; set; }
    }
}
