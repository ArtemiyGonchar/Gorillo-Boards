using AutoMapper;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Entites;
using DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Classes
{
    public class FilteringService : IFilteringService
    {
        private readonly ITicketLabelRepository _labelRepository;
        private readonly ITicketBoardRepository _boardRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;

        public FilteringService(ITicketLabelRepository labelRepository, IMapper mapper, ITicketBoardRepository boardRepository, ITicketRepository ticketRepository)
        {
            _labelRepository = labelRepository;
            _mapper = mapper;
            _boardRepository = boardRepository;
            _ticketRepository = ticketRepository;
        }

        public async Task<Guid> AddLabelToTicket(AddLabelToTicketDTO addLabelToTicket)
        {
            var ticket = await _ticketRepository.GetAsync(addLabelToTicket.TicketId);
            if (ticket == null)
            {
                throw new Exception("Such ticket not exists");
            }

            var label = await _labelRepository.GetAsync(addLabelToTicket.LabelId);
            if (label == null)
            {
                throw new Exception("Such label not exists");
            }

            ticket.TicketLabelId = addLabelToTicket.LabelId;
            var ticketId = await _ticketRepository.UpdateAsync(ticket);
            return ticketId;
        }

        public async Task<Guid> CreateLabel(LabelCreateDTO labelCreateDTO)
        {
            var board = await _boardRepository.GetAsync(labelCreateDTO.BoardId);
            if (board == null)
            {
                throw new Exception($"Such board not exists{labelCreateDTO.BoardId}");
            }

            var label = _mapper.Map<TicketLabel>(labelCreateDTO);

            var labelId = await _labelRepository.CreateAsync(label);
            return labelId;
        }
    }
}
