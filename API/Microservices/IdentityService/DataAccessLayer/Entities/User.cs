using DataAccessLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string? AvatarUrl { get; set; }
        public string PasswordHash { get; set; }
        //public string Salt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public UserRoleGlobal Role { get; set; } = UserRoleGlobal.Member;
    }
}
