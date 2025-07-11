using BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace PresentationLayer.Controllers
{
    [Route("api/boards")]
    [ApiController]
    [Authorize]
    public class BoardsAccessController : ControllerBase
    {
        private readonly IBoardAccessService _boardAccessService;

        public BoardsAccessController(IBoardAccessService boardAccessService)
        {
            _boardAccessService = boardAccessService;
        }

        [HttpGet("GetBoards")]
        public async Task<IActionResult> GetBoards()
        {
            var role = User.FindFirst(ClaimTypes.Role).Value;

            if (string.IsNullOrEmpty(role))
            {
                return Unauthorized();
            }
            var boards = await _boardAccessService.GetBoards(role);
            return Ok(boards);
        }

        [HttpGet("get-all-boards")]
        public async Task<IActionResult> GetAllBoards()
        {
            var boards = await _boardAccessService.GetAllBoards();
            return Ok(boards);
        }


        [HttpGet("{boardId}/has-access")]
        public async Task<IActionResult> HasAccess(Guid boardId)
        {
            var role = User.FindFirst(ClaimTypes.Role).Value;
            if (string.IsNullOrEmpty(role))
            {
                return Unauthorized();
            }

            var hasAccess = await _boardAccessService.HasAccess(boardId, role);
            if (!hasAccess)
            {
                return Unauthorized();
            }
            return Ok(hasAccess);
        }

        [HttpGet("{boardId}/get-board-by-id")]
        public async Task<IActionResult> GetBoardById(Guid boardId)
        {
            var board = await _boardAccessService.GetBoardById(boardId);
            return Ok(board);
        }
    }
}
