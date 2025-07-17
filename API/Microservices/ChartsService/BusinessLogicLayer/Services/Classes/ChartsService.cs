using AutoMapper;
using BusinessLogicLayer.DTO.Ticket.Request;
using BusinessLogicLayer.DTO.Ticket.Responce;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Classes
{
    public class ChartsService : IChartsService
    {
        private readonly IMapper _mapper;
        private readonly ITicketRepository _ticketRepository;
        public ChartsService(ITicketRepository ticketRepository, IMapper mapper)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
        }

        public async Task<List<TicketDTO>> GetTicketsByDate(SprintDTO sprintDTO)
        {
            var tickets = await _ticketRepository.GetAllByPeriod(sprintDTO.StartedAt, sprintDTO.EndedAt);
            var ticketsMapped = _mapper.Map<List<TicketDTO>>(tickets);

            return ticketsMapped;
        }

        public async Task<List<TicketDTO>> GetTicketsByDateAndBoard(SprintBoardDTO sprintBoardDTO)
        {
            var tickets = await _ticketRepository.GetAllByPeriodAndBoard(sprintBoardDTO.StartedAt, sprintBoardDTO.EndedAt, sprintBoardDTO.BoardId);
            var ticketsMapped = _mapper.Map<List<TicketDTO>>(tickets);

            return ticketsMapped;
        }
    }
}
