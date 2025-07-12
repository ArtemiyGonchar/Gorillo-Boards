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

        [HttpPost("register-user")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDTO userRegistrationDTO)
        {
            var userId = await _userManagmentService.RegisterUser(userRegistrationDTO);
            return Ok(userId);
        }

        [HttpPost("delete-user")]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserDTO dto)
        {
            var IsDeleted = await _userManagmentService.DeleteUserByUsername(dto.UserName);

            return Ok(IsDeleted);
        }

        [HttpGet("get-all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManagmentService.GetAllUsers();
            return Ok(users);
        }
    }
}
