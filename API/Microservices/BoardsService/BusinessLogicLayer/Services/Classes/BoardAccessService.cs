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
        private readonly IBoardRoleRepository _boardRoleRepository;
        private readonly IBoardRepository _boardRepository;
        private readonly IMapper _mapper;

        public BoardAccessService(IBoardRepository boardRepository, IMapper mapper, IBoardRoleRepository boardRoleRepository)
        {
            _boardRepository = boardRepository;
            _mapper = mapper;
            _boardRoleRepository = boardRoleRepository;
        }

        public async Task<BoardResponseDTO> GetBoardById(Guid boardId)
        {
            var board = await _boardRepository.GetAsync(boardId);
            if (board == null)
            {
                throw new Exception("Such board not exists");
            }

            var boardResponse = _mapper.Map<BoardResponseDTO>(board);
            return boardResponse;
        }

        public async Task<List<BoardResponseDTO>> GetBoards(string role)
        {
            var roleStringToEnum = Enum.Parse<UserRoleBL>(role);

            var roleMapped = _mapper.Map<UserRoleGlobal>(roleStringToEnum);
            var board = await _boardRepository.GetBoardsByRole(roleMapped);

            return _mapper.Map<List<BoardResponseDTO>>(board);
        }

        public async Task<bool> HasAccess(Guid boardId, string role)
        {
            var roleStringToEnum = Enum.Parse<UserRoleBL>(role);

            var roleMapped = _mapper.Map<UserRoleGlobal>(roleStringToEnum);
            var hasRole = await _boardRoleRepository.BoardHasSuchRole(boardId, roleMapped);

            return hasRole;
        }
    }
}
