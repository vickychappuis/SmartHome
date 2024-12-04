using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using smarthome.Domain;
using smarthome.Dtos;
using smarthome.IBusinessLogic;

namespace smarthome.WebApi.Controllers
{
    [Route("/api/devices")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private IDeviceService _deviceService;

        public DevicesController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        [HttpGet]
        [RoleAuthorizationFilter()]
        public IActionResult Get(
            [FromQuery] int? skip,
            [FromQuery] int? take,
            [FromQuery] string? name,
            [FromQuery] string? model,
            [FromQuery] string? companyName,
            [FromQuery] DeviceType? deviceType
        )
        {
            var devices = _deviceService.GetDevices(
                skip,
                take,
                name,
                model,
                companyName,
                deviceType
            );

            return Ok(devices);
        }

        [Route("{id}")]
        [HttpGet]
        [RoleAuthorizationFilter()]
        public IActionResult Get(int id)
        {
            var device = _deviceService.GetDevice(id);

            if (device == null)
            {
                return NotFound();
            }

            return Ok(device);
        }

        [Route("types")]
        [RoleAuthorizationFilter]
        [HttpGet]
        public IActionResult GetDeviceTypes()
        {
            var deviceTypes = _deviceService.ListDeviceTypes();

            return Ok(deviceTypes);
        }

        [RoleAuthorizationFilter(UserRoles.CompanyOwner)]
        [HttpPost]
        public IActionResult Post([FromBody] DeviceDto device, [FromQuery] int type)
        {
            try
            {
                _deviceService.AddDevice(device, (DeviceType)type);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("import/strategies")]
        [RoleAuthorizationFilter(UserRoles.CompanyOwner)]
        [HttpGet]
        public IActionResult GetImportStrategies()
        {
            var strategies = _deviceService.ListImportStrategies();
            return Ok(strategies);
        }

        [RoleAuthorizationFilter(UserRoles.CompanyOwner)]
        [HttpPost("import")]
        public IActionResult ImportDevices([FromBody] ImportDevicesDto importDevicesDto)
        {
            _deviceService.ImportDevices(importDevicesDto.Data, importDevicesDto.ImportStrategy, importDevicesDto.CompanyId);
            return Ok();
        }

        [Route("validators")]
        [RoleAuthorizationFilter(UserRoles.CompanyOwner)]
        [HttpGet]
        public IActionResult GetValidators()
        {
            var validators = _deviceService.ListValidators();
            return Ok(validators);
        }
    }
}
