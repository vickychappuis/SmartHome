using Microsoft.AspNetCore.Mvc;
using smarthome.Domain;
using smarthome.Dtos;
using smarthome.IBusinessLogic;

namespace smarthome.WebApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService { get; set; }

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [Route("login")]
        [HttpPost]
        public IActionResult Login([FromBody] AuthDto body)
        {
            try
            {
                var token = _authService.Login(body);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("signup")]
        [HttpPost]
        public IActionResult Signup([FromBody] SignupDto dto)
        {
            try
            {
                var token = _authService.Signup(dto);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("logout")]
        [HttpPost]
        [RoleAuthorizationFilter()]
        public IActionResult Logout()
        {
            try
            {
                var session = (Session)HttpContext.Items["Session"]!;

                _authService.Logout(session.Token);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
