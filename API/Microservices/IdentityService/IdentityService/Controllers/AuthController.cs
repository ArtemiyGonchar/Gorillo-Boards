using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Extenstions;

namespace PresentationLayer.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly JwtTokenProvider _jwtTokenProvider;
        public AuthController(IAuthService authService, JwtTokenProvider jwtTokenProvider)
        {
            _authService = authService;
            _jwtTokenProvider = jwtTokenProvider;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            var userJwt = await _authService.LoginUser(userLoginDTO);

            if (userJwt != null)
            {
                var token = _jwtTokenProvider.CreateToken(userJwt.Id, userJwt.UserName, userJwt.Role.ToString());
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

    }
}
