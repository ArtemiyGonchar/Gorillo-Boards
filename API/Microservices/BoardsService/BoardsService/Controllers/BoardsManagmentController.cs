using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BoardsService.Controllers
{
    [Route("api/admin/boardmanagment")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class BoardsManagmentController : ControllerBase
    {
        private readonly IBoardsManagmentService _boardsManagmentService;
        private readonly ILogger<BoardsManagmentController> _logger;

        public BoardsManagmentController(IBoardsManagmentService boardsManagmentService, ILogger<BoardsManagmentController> logger)
        {
            _boardsManagmentService = boardsManagmentService;
            _logger = logger;
        }

        [HttpPost("create-board")]
        public async Task<IActionResult> CreateBoard([FromBody] BoardCreateDTO boardCreateDTO)
        {
            var boardId = await _boardsManagmentService.CreateBoardAsync(boardCreateDTO);
            _logger.LogInformation($"Board created, id: {boardId}, title: {boardCreateDTO.Title}");
            return Ok(boardId);
        }

        [HttpPost("allow-role-for-board")]
        public async Task<IActionResult> CreateBoardAllowedRole([FromBody] BoardCreateAllowedRoleDTO boardCreateAllowedRoleDTO)
        {
            var id = await _boardsManagmentService.CreateBoardRole(boardCreateAllowedRoleDTO);
            _logger.LogInformation($"Allowed role to board, title: {boardCreateAllowedRoleDTO.Title}, role: {boardCreateAllowedRoleDTO.AllowedRole}");
            return Ok(id);
        }

        [HttpPost("forbid-role-from-board")]
        public async Task<IActionResult> DeleteRoleFromBoard([FromBody] BoardDeleteAllowedRoleDTO boardDeleteAllowedRoleDTO)
        {
            var isDeleted = await _boardsManagmentService.DeleteBoardRole(boardDeleteAllowedRoleDTO);
            _logger.LogInformation($"Forbid role to board, title: {boardDeleteAllowedRoleDTO.Title}, role: {boardDeleteAllowedRoleDTO.AllowedRole}");
            return Ok(isDeleted);
        }

        [HttpPost("delete-board")]
        public async Task<IActionResult> DeleteBoard([FromBody] DeleteBoardDTO dto)
        {
            var isDeleted = await _boardsManagmentService.DeleteBoardAsync(dto.Title);
            _logger.LogInformation($"Board deleted, title: {dto.Title}");
            return Ok(isDeleted);
        }
    }
}
