using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Services.Interfaces;
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

        public BoardsManagmentController(IBoardsManagmentService boardsManagmentService)
        {
            _boardsManagmentService = boardsManagmentService;
        }

        [HttpPost("create-board")]
        public async Task<IActionResult> CreateBoard([FromBody] BoardCreateDTO boardCreateDTO)
        {
            var boardId = await _boardsManagmentService.CreateBoardAsync(boardCreateDTO);
            return Ok(boardId);
        }

        [HttpPost("allow-role-for-board")]
        public async Task<IActionResult> CreateBoardAllowedRole([FromBody] BoardCreateAllowedRoleDTO boardCreateAllowedRoleDTO)
        {
            var id = await _boardsManagmentService.CreateBoardRole(boardCreateAllowedRoleDTO);
            return Ok(id);
        }

        [HttpPost("forbid-role-from-board")]
        public async Task<IActionResult> DeleteRoleFromBoard([FromBody] BoardDeleteAllowedRoleDTO boardDeleteAllowedRoleDTO)
        {
            var isDeleted = await _boardsManagmentService.DeleteBoardRole(boardDeleteAllowedRoleDTO);
            return Ok(isDeleted);
        }

        [HttpPost("delete-board")]
        public async Task<IActionResult> DeleteBoard([FromBody] DeleteBoardDTO dto)
        {
            var isDeleted = await _boardsManagmentService.DeleteBoardAsync(dto.Title);
            return Ok(isDeleted);
        }
    }
}
