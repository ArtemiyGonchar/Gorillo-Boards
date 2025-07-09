using BusinessLogicLayer.DTO.Label;
using BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PresentationLayer.Hubs;

namespace PresentationLayer.Controllers
{
    [Route("api/boards/{boardId}/filtering")]
    [ApiController]
    [Authorize]
    public class FilteringController : ControllerBase
    {
        private readonly IFilteringService _filteringService;
        private readonly IHubContext<WorkflowHub> _hubContext;

        public FilteringController(IFilteringService filteringService, IHubContext<WorkflowHub> hubContext)
        {
            _filteringService = filteringService;
            _hubContext = hubContext;
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
            await _hubContext.Clients.Group(dto.BoardId.ToString()).SendAsync("WorkflowUpdated", dto.BoardId);
            return Ok(ticketId);
        }

        [HttpPost("delete-label-from-ticket")]
        public async Task<IActionResult> DeleteLabelFromTicket(DeleteLabelFromTicketDTO dto)
        {
            var ticketId = await _filteringService.DeleteLabelFromTicket(dto);
            await _hubContext.Clients.Group(dto.BoardId.ToString()).SendAsync("WorkflowUpdated", dto.BoardId);
            return Ok(ticketId);
        }

        [HttpPost("get-label")]
        public async Task<IActionResult> GetLabel(LabelByIdDTO dto)
        {
            var label = await _filteringService.GetLabelById(dto);
            return Ok(label);
        }
    }
}
