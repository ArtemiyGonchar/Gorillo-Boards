using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Entities;
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
        private readonly ILogger<UserManagmentController> _logger;
        public UserManagmentController(IUserManagmentService userManagmentService, ILogger<UserManagmentController> logger)
        {
            _userManagmentService = userManagmentService;
            _logger = logger;
        }

        [HttpPost("register-user")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDTO userRegistrationDTO)
        {
            var userId = await _userManagmentService.RegisterUser(userRegistrationDTO);
            _logger.LogInformation($"User registered {userId}");
            return Ok(userId);
        }

        [HttpPost("delete-user")]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserDTO dto)
        {
            var IsDeleted = await _userManagmentService.DeleteUserByUsername(dto.UserName);
            _logger.LogInformation($"User registered {dto.UserName}");
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
