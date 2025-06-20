using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GorilloBoards.Contracts.IntegrationEvents
{
    public class UserDeletedEvent
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
    }
}
