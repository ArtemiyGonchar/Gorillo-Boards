using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Extenstions;
using System.Security.Claims;

namespace PresentationLayer.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly JwtTokenProvider _jwtTokenProvider;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IAuthService authService, JwtTokenProvider jwtTokenProvider, ILogger<AuthController> logger)
        {
            _authService = authService;
            _jwtTokenProvider = jwtTokenProvider;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            var userJwt = await _authService.LoginUser(userLoginDTO);

            if (userJwt != null)
            {
                var token = _jwtTokenProvider.CreateToken(userJwt.Id, userJwt.UserName, userJwt.Role.ToString());
                _logger.LogInformation($"User logged: {userJwt.UserName}");
                return Ok(token);
            }
            return Unauthorized();
        }


        //for react hook
        [Authorize]
        [HttpGet("isAuthorized")]
        public async Task<IActionResult> IsAuthorized()
        {
            return Ok(true);
        }

        [Authorize]
        [HttpPost("get-user-by-id")]
        public async Task<IActionResult> GetUserById(GetUserByIdDTO dto)
        {
            var user = await _authService.GetUserById(dto.Id);
            return Ok(user);
        }

        [Authorize]
        [HttpGet("user-is-admin")]
        public async Task<IActionResult> isAdmin()
        {
            var role = User.FindFirst(ClaimTypes.Role).Value;
            if (role == "Admin")
            {
                return Ok(true);
            }
            return Ok(false);
        }
    }
}
