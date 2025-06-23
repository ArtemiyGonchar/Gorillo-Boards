using BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            var boards = await _boardAccessService.GetBoards(role);
            return Ok(boards);
        }
    }
}
