using BusinessLogicLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class UserRegistrationDTO
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string PasswordHash { get; set; }
        public UserRoleDTO Role { get; set; }
    }
}
