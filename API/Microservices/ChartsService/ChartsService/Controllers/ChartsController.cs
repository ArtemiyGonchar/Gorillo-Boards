using BusinessLogicLayer.DTO.Ticket.Request;
using BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChartsService.Controllers
{
    [Route("api/charts")]
    [ApiController]
    [Authorize]
    public class ChartsController : ControllerBase
    {
        private readonly IChartsService _chartsService;
        private readonly ILogger<ChartsController> _logger;

        public ChartsController(IChartsService chartsService)
        {
            _chartsService = chartsService;
        }

        [HttpPost("get-tickets-by-sprint")]
        public async Task<IActionResult> GetTicketsBySprint(SprintDTO dto)
        {
            // _logger.LogInformation($"Logged {}");
            var tickets = await _chartsService.GetTicketsByDate(dto);
            return Ok(tickets);
        }

        [HttpPost("get-tickets-by-sprint-and-board")]
        public async Task<IActionResult> GetTicketsBySprintAndBoard(SprintBoardDTO dto)
        {
            var tickets = await _chartsService.GetTicketsByDateAndBoard(dto);
            return Ok(tickets);
        }

        [HttpGet("test")]
        public async Task<IActionResult> test()
        {
            return Ok(true);
        }
    }
}
