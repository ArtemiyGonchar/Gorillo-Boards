using BusinessLogicLayer.DTO.State;
using BusinessLogicLayer.DTO.Ticket;
using BusinessLogicLayer.DTO.TimeLog;
using BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PresentationLayer.Hubs;
using System.Security.Claims;

namespace PresentationLayer.Controllers
{
    [Route("api/boards/{boardId}/states")]
    [ApiController]
    [Authorize]
    public class WorkflowController : ControllerBase
    {
        private readonly IStateManagementService _stateManagementService;
        private readonly ITicketManagementService _ticketManagementService;
        private readonly ITimeLogService _timeLogService;
        private readonly IHubContext<WorkflowHub> _hubContext;

        public WorkflowController(IStateManagementService stateManagementService, ITicketManagementService ticketManagementService
            , ITimeLogService timeLogService, IHubContext<WorkflowHub> hubContext)
        {
            _stateManagementService = stateManagementService;
            _ticketManagementService = ticketManagementService;
            _timeLogService = timeLogService;
            _hubContext = hubContext;
        }

        [HttpPost("create-state")]
        public async Task<IActionResult> CreateState([FromBody] StateCreateDTO dto)
        {
            var stateId = await _stateManagementService.CreateState(dto);
            await _hubContext.Clients.Group(dto.BoardId.ToString()).SendAsync("WorkflowUpdated", dto.BoardId);
            return Ok(stateId);
        }

        [HttpPost("delete-state")]
        public async Task<IActionResult> DeleteState([FromBody] Guid stateId)
        {
            var deleted = await _stateManagementService.DeleteState(stateId);
            return Ok(deleted);
        }

        [HttpPost("change-state-order")]
        public async Task<IActionResult> ChangeStateOrder([FromBody] StateChangeOrder dto)
        {
            var changedOrder = await _stateManagementService.ChangeOrderState(dto);
            await _hubContext.Clients.Group(dto.BoardId.ToString()).SendAsync("WorkflowUpdated", dto.BoardId);
            return Ok(changedOrder);
        }

        [HttpGet("get-states-by-board")]
        public async Task<IActionResult> GetStatesByBoard([FromRoute] Guid boardId)
        {
            var states = await _stateManagementService.GetStatesByBoard(boardId);
            return Ok(states);
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
            var requestorId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            dto.UserRequestor = Guid.Parse(requestorId);
            var id = await _ticketManagementService.CreateTicket(dto);
            await _hubContext.Clients.Group(dto.BoardId.ToString()).SendAsync("WorkflowUpdated", dto.BoardId);
            return Ok(id);
        }

        [HttpPost("change-ticket-order")]
        public async Task<IActionResult> ChangeTicketOrder([FromBody] TicketChangeOrderDTO dto)
        {
            var changedOrder = await _ticketManagementService.ChangeOrderTicket(dto.Id, dto.OrderTarget);
            await _hubContext.Clients.Group(dto.BoardId.ToString()).SendAsync("WorkflowUpdated", dto.BoardId);
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
            await _hubContext.Clients.Group(dto.BoardId.ToString()).SendAsync("WorkflowUpdated", dto.BoardId);
            return Ok(id);
        }

        [HttpPost("change-ticket-description")]
        public async Task<IActionResult> ChangeTicketDescription([FromBody] TicketChangeDescription dto)
        {
            var id = await _ticketManagementService.ChangeDescriptionTicket(dto);
            return Ok(id);
        }

        [HttpPost("change-ticket-state")]
        public async Task<IActionResult> ChangeTicketState([FromBody] TicketChangeStateDTO dto)
        {
            var id = await _ticketManagementService.ChangeTicketState(dto);
            await _hubContext.Clients.Group(dto.BoardId.ToString()).SendAsync("WorkflowUpdated", dto.BoardId);
            return Ok(id);
        }

        [HttpPost("close-ticket")]
        public async Task<IActionResult> CloseTicket([FromBody] TicketCloseDTO dto)
        {
            var requestorId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            dto.UserRequestor = Guid.Parse(requestorId);
            var id = await _ticketManagementService.CloseTicket(dto);
            return Ok(id);
        }

        [HttpPost("start-work-on-ticket")]
        public async Task<IActionResult> StartWork([FromBody] TicketStartWorkDTO dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            dto.UserId = Guid.Parse(userId);
            var timeLogId = await _timeLogService.TicketWorkStart(dto);
            await _hubContext.Clients.Group(dto.BoardId.ToString()).SendAsync("WorkflowUpdated", dto.BoardId);
            return Ok(timeLogId);
        }

        [HttpPost("end-work-on-ticket")]
        public async Task<IActionResult> EndWork([FromBody] TicketEndWorkDTO dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            dto.UserId = Guid.Parse(userId);
            var timeLogId = await _timeLogService.TicketWorkEnd(dto);
            await _hubContext.Clients.Group(dto.BoardId.ToString()).SendAsync("WorkflowUpdated", dto.BoardId);
            return Ok(timeLogId);
        }

        [HttpPost("assign-user-to-ticket")]
        public async Task<IActionResult> AssignUserToTicket([FromBody] TicketAssigneUserDTO dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            dto.UserId = Guid.Parse(userId);
            var ticketId = await _ticketManagementService.AssignUserToTicket(dto);
            await _hubContext.Clients.Group(dto.BoardId.ToString()).SendAsync("WorkflowUpdated", dto.BoardId);
            return Ok(ticketId);
        }

        [HttpPost("get-tickets-by-state")]
        public async Task<IActionResult> GetTicketsByState([FromBody] TicketGetByState dto)
        {
            var tickets = await _ticketManagementService.GetTicketsByState(dto);
            return Ok(tickets);
        }
    }
}