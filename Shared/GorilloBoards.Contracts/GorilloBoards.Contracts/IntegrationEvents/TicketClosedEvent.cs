﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GorilloBoards.Contracts.IntegrationEvents
{
    public class TicketClosedEvent
    {
        public Guid Id { get; set; }
        public DateTime TicketClosed { get; set; }
    }
}
