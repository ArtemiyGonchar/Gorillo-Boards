﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO.Label
{
    public class AddLabelToTicketDTO
    {
        public Guid BoardId { get; set; }
        public Guid LabelId { get; set; }
        public Guid TicketId { get; set; }
    }
}
