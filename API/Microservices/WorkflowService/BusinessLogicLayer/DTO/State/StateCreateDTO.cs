﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO.State
{
    public class StateCreateDTO
    {
        public string Title { get; set; }
        public Guid BoardId { get; set; }
    }
}
