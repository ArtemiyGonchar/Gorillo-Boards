﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entites
{
    public class Ticket : BaseEntity
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public int Order { get; set; }

        public bool IsClosed { get; set; } = false;
        public DateTime? TicketClosed { get; set; }

        public Guid? UserRequestor { get; set; }
        public Guid? UserAssigned { get; set; }

        public Guid StateId { get; set; }
        public State State { get; set; }

        public Guid? TicketLabelId { get; set; }
        public TicketLabel? TicketLabel { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<TicketTimeLog>? TimeLogs { get; set; }
    }
}
