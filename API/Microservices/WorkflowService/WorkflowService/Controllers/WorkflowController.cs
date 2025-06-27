using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    [Route("api/boards/{boardId}/states")]
    [ApiController]
    [Authorize]
    public class WorkflowController : ControllerBase
    {
        private readonly IStateManagementService _stateManagementService;
        private readonly ITicketManagementService _ticketManagementService;

        public WorkflowController(IStateManagementService stateManagementService, ITicketManagementService ticketManagementService)
        {
            _stateManagementService = stateManagementService;
            _ticketManagementService = ticketManagementService;
        }

        [HttpPost("create-state")]
        public async Task<IActionResult> CreateState([FromBody] StateCreateDTO dto)
        {
            var stateId = await _stateManagementService.CreateState(dto);
            return Ok(stateId);
        }

        [HttpPost("delete-state")]
        public async Task<IActionResult> DeleteState([FromBody] Guid stateId)
        {
            var deleted = await _stateManagementService.DeleteState(stateId);
            return Ok(deleted);
        }

        [HttpPost("change-state-order")]
        public async Task<IActionResult> ChangeStateOrder([FromQuery] Guid stateId, int orderTarget)
        {
            var changedOrder = await _stateManagementService.ChangeOrderState(stateId, orderTarget);
            return Ok(changedOrder);
        }

        [HttpPost("rename-state")]
        public async Task<IActionResult> RenameState([FromBody] StateRenameDTO dto)
        {
            var id = await _stateManagementService.RenameState(dto);
            return Ok(id);
        }

        [HttpPost("create-ticket")]
        public async Task<IActionResult> CreateTicket([FromBody] TicketCreateDTO dto)
        {
            var id = await _ticketManagementService.CreateTicket(dto);
            return Ok(id);
        }

        [HttpPost("change-ticket-order")]
        public async Task<IActionResult> ChangeTicketOrder([FromBody] Guid ticketId, int orderTarget)
        {
            var changedOrder = await _ticketManagementService.ChangeOrderTicket(ticketId, orderTarget);
            return Ok(changedOrder);
        }

        [HttpPost("delete-ticket")]
        public async Task<IActionResult> DeleteTicket([FromBody] Guid ticketId)
        {
            var deleted = await _ticketManagementService.DeleteTicket(ticketId);
            return Ok(deleted);
        }

        [HttpPost("rename-ticket")]
        public async Task<IActionResult> RenameTicket([FromBody] TicketRenameDTO dto)
        {
            var id = await _ticketManagementService.RenameTicket(dto);
            return Ok(id);
        }
    }
}
