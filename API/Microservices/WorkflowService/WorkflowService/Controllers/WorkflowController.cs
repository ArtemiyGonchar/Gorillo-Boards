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

        public WorkflowController(IStateManagementService stateManagementService)
        {
            _stateManagementService = stateManagementService;
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
    }
}
