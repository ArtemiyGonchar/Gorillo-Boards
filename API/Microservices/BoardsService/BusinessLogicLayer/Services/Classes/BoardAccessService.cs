using AutoMapper;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Enums;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Enums;
using DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Classes
{
    public class BoardAccessService : IBoardAccessService
    {
        private readonly IBoardRepository _boardRepository;
        private readonly IMapper _mapper;

        public BoardAccessService(IBoardRepository boardRepository, IMapper mapper)
        {
            _boardRepository = boardRepository;
            _mapper = mapper;
        }

        public async Task<List<BoardDTO>> GetBoards(string role)
        {
            var roleStringToEnum = Enum.Parse<UserRoleBL>(role);

            var roleMapped = _mapper.Map<UserRoleGlobal>(roleStringToEnum);
            var board = await _boardRepository.GetBoardsByRole(roleMapped);

            return _mapper.Map<List<BoardDTO>>(board);
        }
    }
}
