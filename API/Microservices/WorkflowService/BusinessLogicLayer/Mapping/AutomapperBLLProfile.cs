using AutoMapper;
using BusinessLogicLayer.DTO.Board;
using BusinessLogicLayer.DTO.Label;
using BusinessLogicLayer.DTO.State;
using BusinessLogicLayer.DTO.Ticket;
using BusinessLogicLayer.DTO.TimeLog;
using DataAccessLayer.Entites;
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
            CreateMap<TicketBoard, BoardCreatedDTO>().ReverseMap();

            CreateMap<State, StateCreateDTO>().ReverseMap();
            CreateMap<State, StateRenameDTO>().ReverseMap();
            CreateMap<State, StateListDTO>().ReverseMap();

            CreateMap<Ticket, TicketCreateDTO>().ReverseMap();
            CreateMap<Ticket, TicketRenameDTO>().ReverseMap();
            CreateMap<Ticket, TicketListDTO>().ReverseMap();

            CreateMap<TicketLabel, LabelCreateDTO>().ReverseMap();
            CreateMap<TicketLabel, LabelByBoardDTO>().ReverseMap();

            CreateMap<TicketTimeLog, TicketStartWorkDTO>().ReverseMap();
        }
    }
}
