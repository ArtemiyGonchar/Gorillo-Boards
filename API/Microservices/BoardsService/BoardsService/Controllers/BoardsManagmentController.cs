using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BoardsService.Controllers
{
    [Route("api/admin/boardmanagment")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class BoardsManagmentController : Controller
    {
        private readonly IBoardsManagmentService _boardsManagmentService;

        public BoardsManagmentController(IBoardsManagmentService boardsManagmentService)
        {
            _boardsManagmentService = boardsManagmentService;
        }

        [HttpPost("CreateBoard")]
        public async Task<IActionResult> CreateBoard([FromBody] BoardCreateDTO boardCreateDTO)
        {
            var boardId = await _boardsManagmentService.CreateBoardAsync(boardCreateDTO);
            return Ok(boardId);
        }

        [HttpPost("CreateBoardAllowedRole")]
        public async Task<IActionResult> CreateBoardAllowedRole([FromBody] BoardCreateAllowedRoleDTO boardCreateAllowedRoleDTO)
        {
            var id = await _boardsManagmentService.CreateBoardRole(boardCreateAllowedRoleDTO);
            return Ok(id);
        }

        [HttpPost("DeleteRoleFromBoard")]
        public async Task<IActionResult> DeleteRoleFromBoard([FromBody] BoardDeleteAllowedRoleDTO boardDeleteAllowedRoleDTO)
        {
            var isDeleted = await _boardsManagmentService.DeleteBoardRole(boardDeleteAllowedRoleDTO);
            return Ok(isDeleted);
        }

        [HttpPost("DeleteBoard")]
        public async Task<IActionResult> DeleteBoard([FromBody] string title)
        {
            var isDeleted = await _boardsManagmentService.DeleteBoardAsync(title);
            return Ok(isDeleted);
        }
    }
}
