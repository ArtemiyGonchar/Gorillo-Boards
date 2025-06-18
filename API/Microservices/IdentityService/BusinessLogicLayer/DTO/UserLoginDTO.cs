using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class UserLoginDTO
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
    }
}
