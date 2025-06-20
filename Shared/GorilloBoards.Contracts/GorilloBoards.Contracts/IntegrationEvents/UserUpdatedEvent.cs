using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GorilloBoards.Contracts.IntegrationEvents
{
    public class UserUpdatedEvent
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }
}
