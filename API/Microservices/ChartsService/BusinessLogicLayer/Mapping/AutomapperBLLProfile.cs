using AutoMapper;
using BusinessLogicLayer.DTO.Ticket.Request;
using BusinessLogicLayer.DTO.Ticket.Responce;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Mapping
{
    public class AutomapperBLLProfile : Profile
    {
        public AutomapperBLLProfile()
        {
            CreateMap<Ticket, TicketCreateDTO>().ReverseMap();
            CreateMap<Ticket, TicketCloseDTO>().ForMember(dest => dest.TicketClosed, opt => opt.MapFrom(src => src.TicketClose))
                .ReverseMap();

            CreateMap<Ticket, TicketDTO>().ReverseMap();
        }
    }
}
