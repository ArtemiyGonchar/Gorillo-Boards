using AutoMapper;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Classes
{
    public class BoardsManagmentService : IBoardsManagmentService
    {
        private readonly IBoardRepository _boardRepository;
        private readonly IMapper _mapper;

        public BoardsManagmentService(IBoardRepository boardRepository, IMapper mapper)
        {
            _boardRepository = boardRepository;
            _mapper = mapper;
        }

        public async Task<Guid> CreateBoardAsync(BoardCreateDTO boardCreateDTO)
        {
            var boardInDb = await _boardRepository.GetBoardByTitle(boardCreateDTO.Title) == null;

            if (!boardInDb)
            {
                throw new Exception("Such board exists");
            }

            var boardMapped = _mapper.Map<Board>(boardCreateDTO);
            var boardId = await _boardRepository.CreateAsync(boardMapped);

            /// here will be service bus event
            /// 
            return boardId;
        }
    }
}
