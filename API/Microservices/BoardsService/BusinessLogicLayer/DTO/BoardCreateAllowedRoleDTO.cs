using BusinessLogicLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class BoardCreateAllowedRoleDTO
    {
        public string Title { get; set; }
        public UserRoleBL AllowedRole { get; set; }
    }
}
