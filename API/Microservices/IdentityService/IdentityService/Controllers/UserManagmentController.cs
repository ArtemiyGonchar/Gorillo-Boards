using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    [Route("api/admin/usermanagment")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserManagmentController : Controller
    {
        private readonly IUserManagmentService _userManagmentService;

        public UserManagmentController(IUserManagmentService userManagmentService)
        {
            _userManagmentService = userManagmentService;
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDTO userRegistrationDTO)
        {
            var userId = await _userManagmentService.RegisterUser(userRegistrationDTO);
            return Ok(userId);
        }

        [HttpPost("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] string username)
        {
            var IsDeleted = await _userManagmentService.DeleteUserByUsername(username);

            return Ok(IsDeleted);
        }
    }
}
