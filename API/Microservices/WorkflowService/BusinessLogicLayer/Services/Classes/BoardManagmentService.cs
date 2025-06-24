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
    public class BoardManagmentService : IBoardManagmentService
    {
        private readonly ITicketBoardRepository _ticketBoardRepository;
        private readonly IMapper _mapper;
        public BoardManagmentService(ITicketBoardRepository ticketBoardRepository, IMapper mapper)
        {
            _ticketBoardRepository = ticketBoardRepository;
            _mapper = mapper;
        }

        public async Task<Guid> BoardCreate(BoardCreatedDTO boardCreatedDTO)
        {
            /*
            var boardInDB = _ticketBoardRepository.GetBoardByTitle(boardCreatedDTO.Title) == null;

            if (!boardInDB)
            {
                throw new Exception("Such board exists");
            }
            */
            var boardMapped = _mapper.Map<TicketBoard>(boardCreatedDTO);
            var boardId = await _ticketBoardRepository.CreateAsync(boardMapped);
            return boardId;
        }
    }
}
