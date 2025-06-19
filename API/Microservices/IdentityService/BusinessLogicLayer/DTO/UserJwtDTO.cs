using BusinessLogicLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class UserJwtDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public UserRoleDTO Role { get; set; }
    }
}
