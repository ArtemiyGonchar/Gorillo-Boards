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
        public async Task<IActionResult> CreateState(StateCreateDTO dto)
        {
            var stateId = await _stateManagementService.CreateState(dto);
            return Ok(stateId);
        }
    }
}
