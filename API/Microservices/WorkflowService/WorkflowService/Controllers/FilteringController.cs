using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    [Route("api/boards/{boardId}/filtering")]
    [ApiController]
    [Authorize]
    public class FilteringController : ControllerBase
    {
        private readonly IFilteringService _filteringService;

        public FilteringController(IFilteringService filteringService)
        {
            _filteringService = filteringService;
        }

        [HttpPost("create-label")]
        public async Task<IActionResult> CreateLabel(LabelCreateDTO dto)
        {
            var labelId = await _filteringService.CreateLabel(dto);
            return Ok(labelId);
        }

        [HttpPost("add-label-to-ticket")]
        public async Task<IActionResult> AddLabelToTicket(AddLabelToTicketDTO dto)
        {
            var ticketId = await _filteringService.AddLabelToTicket(dto);
            return Ok(ticketId);
        }
    }
}
