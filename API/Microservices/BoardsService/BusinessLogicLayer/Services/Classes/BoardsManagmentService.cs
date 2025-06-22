using AutoMapper;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Enums;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Classes
{
    public class BoardsManagmentService : IBoardsManagmentService
    {
        private readonly IBoardRepository _boardRepository;
        private readonly IBoardRoleRepository _boardRoleRepository;
        private readonly IMapper _mapper;

        public BoardsManagmentService(IBoardRepository boardRepository, IMapper mapper, IBoardRoleRepository boardRoleRepository)
        {
            _boardRepository = boardRepository;
            _mapper = mapper;
            _boardRoleRepository = boardRoleRepository;
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

        public async Task<Guid> CreateBoardRole(BoardCreateAllowedRoleDTO boardCreateAllowedRoleDTO)
        {
            if ((((int)boardCreateAllowedRoleDTO.AllowedRole) + 1) > Enum.GetNames(typeof(UserRoleBL)).Length)
            {
                throw new Exception("No such role");
            }

            var board = await _boardRepository.GetBoardByTitle(boardCreateAllowedRoleDTO.Title);

            if (board == null)
            {
                throw new Exception("Such board not exists");
            }

            var boardId = board.Id;

            var boardRoleMapped = _mapper.Map<BoardRole>(boardCreateAllowedRoleDTO);
            boardRoleMapped.BoardId = boardId;

            var mappedRole = _mapper.Map<UserRoleGlobal>(boardCreateAllowedRoleDTO.AllowedRole);

            if (await _boardRoleRepository.BoardHasSuchRole(boardId, mappedRole))
            {
                throw new Exception("Such role exists");
            }

            boardRoleMapped.Role = mappedRole;
            var boardRoleCreatedId = await _boardRoleRepository.CreateAsync(boardRoleMapped);
            return boardRoleCreatedId;
        }

        public async Task<bool> DeleteBoardRole(BoardDeleteAllowedRoleDTO boardDeleteAllowedRoleDTO)
        {
            if ((((int)boardDeleteAllowedRoleDTO.AllowedRole) + 1) > Enum.GetNames(typeof(UserRoleBL)).Length)
            {
                throw new Exception("No such role");
            }

            var board = await _boardRepository.GetBoardByTitle(boardDeleteAllowedRoleDTO.Title);

            if (board == null)
            {
                throw new Exception("Such board not exists");
            }

            var boardId = board.Id;

            var boardRoleMapped = _mapper.Map<BoardRole>(boardDeleteAllowedRoleDTO);
            boardRoleMapped.BoardId = boardId;

            var mappedRole = _mapper.Map<UserRoleGlobal>(boardDeleteAllowedRoleDTO.AllowedRole);

            if (!(await _boardRoleRepository.BoardHasSuchRole(boardId, mappedRole)))
            {
                throw new Exception("Such role not exists");
            }

            boardRoleMapped.Role = mappedRole;
            var boardRoleIsDeleted = await _boardRoleRepository.DeleteRoleByBoardId(boardId, mappedRole);
            return boardRoleIsDeleted;
        }
    }
}
