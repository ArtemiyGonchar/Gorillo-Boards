using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class IdentityController : Controller
    {
        private readonly IUserService _userService;

        public IdentityController(IUserService userService) { _userService = userService; }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDTO userRegistrationDTO)
        {
            var userId = await _userService.RegisterUser(userRegistrationDTO);
            return Ok(userId);
        }
    }
}
