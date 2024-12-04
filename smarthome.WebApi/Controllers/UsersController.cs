using Microsoft.AspNetCore.Mvc;
using smarthome.Domain;
using smarthome.Dtos;
using smarthome.IBusinessLogic;

namespace smarthome.WebApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [RoleAuthorizationFilter(UserRoles.Administrator)]
        public IActionResult Get(
            [FromQuery] string? name,
            [FromQuery] UserRoles? role,
            [FromQuery] int? skip,
            [FromQuery] int? take
        )
        {
            try
            {
                return Ok(_userService.GetUsers(skip, take, name, role));
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpGet("{userId}")]
        [RoleAuthorizationFilter(UserRoles.Administrator)]
        public IActionResult Get([FromRoute] int userId)
        {
            try
            {
                var user = _userService.GetById(userId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [RoleAuthorizationFilter(UserRoles.Administrator)]
        public IActionResult Post([FromBody] UserDto user)
        {
            try
            {
                _userService.CreateUser(user);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{userId}")]
        [RoleAuthorizationFilter(UserRoles.Administrator)]
        public IActionResult Delete([FromRoute] int userId)
        {
            try
            {
                _userService.DeleteUser(userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
