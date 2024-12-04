using Microsoft.AspNetCore.Mvc;
using smarthome.Domain;
using smarthome.Dtos;
using smarthome.IBusinessLogic;

namespace smarthome.WebApi.Controllers
{
    [Route("/api/homes")]
    [ApiController]
    public class HomesController : ControllerBase
    {
        private IHomeService _homeService;
        private INotificationService _notificationService;

        public HomesController(IHomeService homeService, INotificationService notificationService)
        {
            _homeService = homeService;
            _notificationService = notificationService;
        }

        [HttpGet]
        [RoleAuthorizationFilter()]
        public IActionResult GetHomes([FromQuery] int? skip, [FromQuery] int? take, [FromQuery] int? userId)
        {
            return Ok(_homeService.GetHomes(skip, take, userId));
        }

        [HttpPost]
        [RoleAuthorizationFilter()]
        public IActionResult Post([FromBody] HomeDto home)
        {
            Session session = (Session)HttpContext.Items["Session"]!;

            try
            {
                var homeId = _homeService.AddHome(home);
                _homeService.AddMember(homeId, session.User.Email);
                return Created("/homes", home);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{homeId}/members")]
        [RoleAuthorizationFilter()]

        public IActionResult GetHomeMembers([FromRoute] int homeId)
        {
            Session session = (Session)HttpContext.Items["Session"]!;

            try
            {
                if (!_homeService.IsMember(homeId, session.User.UserId))
                {
                    return Unauthorized("You must be a member of the home to see the members");
                }
                var members = _homeService.GetMembers(homeId);
                return Ok(members);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{homeId}/devices")]
        [RoleAuthorizationFilter()]

        public IActionResult GetDevices([FromRoute] int homeId, [FromQuery] int? roomId)
        {
            Session session = (Session)HttpContext.Items["Session"]!;

            try
            {
                if (!_homeService.IsMember(homeId, session.User.UserId))
                {
                    return Unauthorized("You must be a member of the home to see the devices");
                }
                var devices = _homeService.GetDevices(homeId, roomId);
                return Ok(devices);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{homeId}/devices")]
        [RoleAuthorizationFilter()]

        public IActionResult AddDeviceToHome([FromRoute] int homeId, [FromBody] AddHomeDeviceDto addHomeDeviceDto)
        {
            Session session = (Session)HttpContext.Items["Session"]!;

            try
            {
                if (!_homeService.IsMember(homeId, session.User.UserId))
                {
                    return Unauthorized("You must be a member of the home to add a device");
                }
                _homeService.AddDevice(homeId, addHomeDeviceDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{homeId}/devices/{deviceId}")]
        [RoleAuthorizationFilter(UserRoles.HomeOwner)]
        public IActionResult ChangeDeviceName([FromRoute] int homeId, [FromRoute] int deviceId, [FromBody] string deviceName)
        {
            Session session = (Session)HttpContext.Items["Session"]!;
            try
            {
                if (!_homeService.IsMember(homeId, session.User.UserId))
                {
                    return Unauthorized("You must be a member of the home to change the device name");
                }
                _homeService.ChangeDeviceName(homeId, deviceId, deviceName);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{homeId}/notifications")]
        [RoleAuthorizationFilter()]

        public IActionResult GetNotifications([FromRoute] int homeId, [FromQuery] bool? read, [FromQuery] DateTime? sinceDate)
        {
            Session session = (Session)HttpContext.Items["Session"]!;

            try
            {
                if (!_homeService.IsMember(homeId, session.User.UserId))
                {
                    return Unauthorized("You must be a member of the home to get notifications");
                }
                var notifications = _homeService.GetNotifications(homeId, session.User.UserId, read, sinceDate);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{homeId}/devices/{deviceId}/notifications")]
        [RoleAuthorizationFilter()]
        public IActionResult CreateNotification(
            [FromRoute] int homeId,
            [FromRoute] int deviceId,
            [FromBody] string notificationType
        )
        {
            Session session = (Session)HttpContext.Items["Session"]!;

            try
            {
                if (!_homeService.IsMember(homeId, session.User.UserId))
                {
                    return Unauthorized(
                        "You must be a member of the home to create a notification"
                    );
                }
                _notificationService.CreateNotification(deviceId, homeId, notificationType, session.User.UserId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{homeId}/members")]
        [RoleAuthorizationFilter()]
        public IActionResult AddMember([FromRoute] int homeId, [FromBody] AddMemberDto addMemberDto)
        {
            Session session = (Session)HttpContext.Items["Session"]!;
            try
            {
                if (!_homeService.IsMember(homeId, session.User.UserId))
                {
                    return Unauthorized("You must be a member of the home to add a member");
                }
                _homeService.AddMember(homeId, addMemberDto.userEmail);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{homeId}/members/notifications")]
        [RoleAuthorizationFilter()]

        public IActionResult Notifications(
            [FromRoute] int homeId,
            [FromBody] NotificationConfigurationDto notificationConfigurationDto
        ) //dont think dinamic is the best option
        {
            Session session = (Session)HttpContext.Items["Session"]!;

            try
            {
                if (!_homeService.IsMember(homeId, session.User.UserId))
                {
                    return Unauthorized(
                        "You must be a member of the home to configure notifications"
                    );
                }
                _homeService.ConfigureNotification(homeId, notificationConfigurationDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("{homeId}/rooms")]
        [RoleAuthorizationFilter(UserRoles.HomeOwner)]
        public IActionResult AddRoom([FromRoute] int homeId, [FromBody] RoomDto room)
        {
            Session session = (Session)HttpContext.Items["Session"]!;
            try
            {
                if (!_homeService.IsMember(homeId, session.User.UserId))
                {
                    return Unauthorized("You must be a member of the home to add a room");
                }
                _homeService.AddRoom(homeId, room);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{homeId}")]
        [RoleAuthorizationFilter(UserRoles.HomeOwner)]
        public IActionResult UpdateHomeName([FromRoute] int homeId, [FromBody] string homeName)
        {
            Session session = (Session)HttpContext.Items["Session"]!;
            try
            {
                if (!_homeService.IsMember(homeId, session.User.UserId))
                {
                    return Unauthorized("You must be a member of the home to update it");
                }
                _homeService.UpdateHome(homeId, homeName);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{homeId}/devices/{deviceId}/connectedState")]
        public IActionResult ChangeHomeDeviceConnectedState(
            [FromRoute] int homeId,
            [FromRoute] int deviceId,
            [FromBody] bool status
        )
        {
            Session session = (Session)HttpContext.Items["Session"]!;
            try
            {
                if (!_homeService.IsMember(homeId, session.User.UserId))
                {
                    return Unauthorized("You must be a member of the home to update the device status");
                }
                _homeService.ChangeConnectedState(homeId, deviceId, status);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{homeId}/devices/{deviceId}/state")]
        [RoleAuthorizationFilter(UserRoles.HomeOwner | UserRoles.Administrator | UserRoles.CompanyOwner)]
        public IActionResult ChangeLampState([FromRoute] int homeId, [FromRoute] int deviceId, [FromBody] bool newState)
        {
            Session session = (Session)HttpContext.Items["Session"]!;

            try
            {
                if (!_homeService.IsMember(homeId, session.User.UserId))
                {
                    return Unauthorized("You must be a member of the home to change the smart lamp state");
                }
                _homeService.ChangeLampState(homeId, deviceId, newState);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{homeId}/devices/{deviceId}/opening")]
        [RoleAuthorizationFilter(UserRoles.HomeOwner | UserRoles.Administrator | UserRoles.CompanyOwner)]
        public IActionResult ChangeWindowState([FromRoute] int homeId, [FromRoute] int deviceId, [FromBody] bool isOpened)
        {
            Session session = (Session)HttpContext.Items["Session"]!;

            try
            {
                if (!_homeService.IsMember(homeId, session.User.UserId))
                {
                    return Unauthorized("You must be a member of the home to change the window state");
                }
                _homeService.ChangeWindowState(homeId, deviceId, isOpened);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{homeId}/rooms")]
        [RoleAuthorizationFilter()]
        public IActionResult GetRooms([FromRoute] int homeId)
        {
            Session session = (Session)HttpContext.Items["Session"]!;
            try
            {
                if (!_homeService.IsMember(homeId, session.User.UserId))
                {
                    return Unauthorized("You must be a member of the home to get the rooms");
                }
                var rooms = _homeService.GetRooms(homeId);
                return Ok(rooms);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
