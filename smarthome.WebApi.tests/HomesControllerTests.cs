using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using smarthome.Domain;
using smarthome.Dtos;
using smarthome.IBusinessLogic;
using smarthome.WebApi.Controllers;

namespace smarthome.WebApi.tests;

[TestClass]
public class HomesControllerTests
{
    private Mock<IHomeService> _homeServiceMock;
    private Mock<INotificationService> _notificationServiceMock;
    private DefaultHttpContext _httpContext;
    private HomesController _homesController;

    [TestInitialize]
    public void Setup()
    {
        _homeServiceMock = new Mock<IHomeService>();
        _notificationServiceMock = new Mock<INotificationService>();
        _homesController = new HomesController(
            _homeServiceMock.Object,
            _notificationServiceMock.Object
        );
        _httpContext = new DefaultHttpContext();
        _homesController.ControllerContext = new ControllerContext() { HttpContext = _httpContext };
    }

    [TestMethod]
    public void Post_InvalidData_ShouldReturnBadRequest()
    {
        _homeServiceMock
            .Setup(service => service.AddHome(It.IsAny<HomeDto>()))
            .Throws(new ArgumentException("Missing data in Home constructor"));
        var result = _homesController.Post(null);
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public void Post_ValidDataButInvalidSession_ShouldBadRequest()
    {
        _homeServiceMock.Setup(service => service.AddHome(It.IsAny<HomeDto>()));
        _homeServiceMock
            .Setup(service => service.AddMember(It.IsAny<int>(), It.IsAny<string>()))
            .Throws(new Exception("Invalid session"));
        var result = _homesController.Post(new HomeDto());
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public void Post_ValidData_ShouldReturnCreated()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };
        _homeServiceMock.Setup(service => service.AddHome(It.IsAny<HomeDto>()));
        _homeServiceMock.Setup(service => service.AddMember(It.IsAny<int>(), It.IsAny<string>()));
        var result = _homesController.Post(new HomeDto());
        Assert.IsInstanceOfType(result, typeof(CreatedResult));
    }

    [TestMethod]
    public void GetHomeMembers_ValidDataButInvalidSession_ShouldReturnUnauthorized()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };
        var homeId = 1;
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(false);
        var result = _homesController.GetHomeMembers(homeId);
        Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
    }

    [TestMethod]
    public void GetHomeMembers_InvalidData_ShouldReturnBadRequest()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };
        var homeId = 1;
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(true);
        _homeServiceMock
            .Setup(service => service.GetMembers(It.IsAny<int>()))
            .Throws(new ArgumentException("Home not found"));
        var result = _homesController.GetHomeMembers(homeId);
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public void GetHomeMembers_ValidData_ShouldReturnOk()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };
        var homeId = 1;
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(true);
        _homeServiceMock
            .Setup(service => service.GetMembers(It.IsAny<int>()))
            .Returns(new List<HomeOwnerDto>());
        var result = _homesController.GetHomeMembers(homeId);
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void GetDevices_ValidDataButInvalidSession_ShouldReturnUnauthorized()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };
        var homeId = 1;
        var roomId = 1;
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(false);
        var result = _homesController.GetDevices(homeId, roomId);
        Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
    }

    [TestMethod]
    public void GetDevices_InvalidData_ShouldReturnBadRequest()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };
        var homeId = 1;
        var roomId = 1;
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(true);
        _homeServiceMock
            .Setup(service => service.GetDevices(It.IsAny<int>(), It.IsAny<int>()))
            .Throws(new ArgumentException("Home not found"));
        var result = _homesController.GetDevices(homeId, roomId);
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public void GetDevices_ValidData_ShouldReturnOk()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };
        var homeId = 1;
        var roomId = 1;
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(true);
        _homeServiceMock
            .Setup(service => service.GetDevices(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(new List<DeviceDto>());
        var result = _homesController.GetDevices(homeId, roomId);
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void PostDevice_ValidDataButInvalidSession_ShouldReturnUnauthorized()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };
        var homeId = 1;
        var deviceDto = new AddHomeDeviceDto { DeviceId = 1 };
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(false);
        var result = _homesController.AddDeviceToHome(homeId, deviceDto);
        Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
    }

    [TestMethod]
    public void PostDevice_InvalidData_ShouldReturnBadRequest()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };
        var homeId = 1;
        var deviceDto = new AddHomeDeviceDto { DeviceId = 1 };
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(true);
        _homeServiceMock
            .Setup(service => service.AddDevice(It.IsAny<int>(), It.IsAny<AddHomeDeviceDto>()))
            .Throws(new ArgumentException("Home not found"));
        var result = _homesController.AddDeviceToHome(homeId, deviceDto);
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public void PostDevice_ValidData_ShouldReturnOk()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };
        var homeId = 1;
        var deviceDto = new AddHomeDeviceDto { DeviceId = 1 };
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(true);
        _homeServiceMock.Setup(service => service.AddDevice(It.IsAny<int>(), It.IsAny<AddHomeDeviceDto>()));
        var result = _homesController.AddDeviceToHome(homeId, deviceDto);
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }

    [TestMethod]
    public void ChangeDeviceName_ValidDataButInvalidSession_ShouldReturnUnauthorized()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };
        var homeId = 1;
        var deviceId = 1;
        var deviceName = "Test Device";
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(false);
        var result = _homesController.ChangeDeviceName(homeId, deviceId, deviceName);
        Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
    }

    [TestMethod]
    public void ChangeDeviceName_InvalidData_ShouldReturnBadRequest()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };
        var homeId = 1;
        var deviceId = 1;
        var deviceName = "Test Device";
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(true);
        _homeServiceMock
            .Setup(service => service.ChangeDeviceName(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
            .Throws(new ArgumentException("Home not found"));
        var result = _homesController.ChangeDeviceName(homeId, deviceId, deviceName);
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public void ChangeDeviceName_ValidData_ShouldReturnOk()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };
        var homeId = 1;
        var deviceId = 1;
        var deviceName = "Test Device";
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(true);
        _homeServiceMock.Setup(service =>
            service.ChangeDeviceName(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())
        );
        var result = _homesController.ChangeDeviceName(homeId, deviceId, deviceName);
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }

    [TestMethod]
    public void PostMember_ValidDataButInvalidSession_ShouldReturnUnauthorized()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };
        var homeId = 1;
        var memberDto = new AddMemberDto { userEmail = "test@example.com" };
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(false);
        var result = _homesController.AddMember(homeId, memberDto);
        Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
    }

    [TestMethod]
    public void PostMember_InvalidData_ShouldReturnBadRequest()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };
        var homeId = 1;
        var memberDto = new AddMemberDto { userEmail = "test@example.com" };
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(true);
        _homeServiceMock
            .Setup(service => service.AddMember(It.IsAny<int>(), It.IsAny<string>()))
            .Throws(new ArgumentException("Home not found"));
        var result = _homesController.AddMember(homeId, memberDto);
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public void PostMember_ValidData_ShouldReturnOk()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };
        var homeId = 1;
        var memberDto = new AddMemberDto { userEmail = "test@example.com" };
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(true);
        _homeServiceMock.Setup(service => service.AddMember(It.IsAny<int>(), It.IsAny<string>()));
        var result = _homesController.AddMember(homeId, memberDto);
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }

    [TestMethod]
    public void PatchNotifcations_ValidDataInvalidaSession_ShouldReturnUnauthorized()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };
        var homeId = 1;
        var notifications = new NotificationConfigurationDto();
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(false);
        var result = _homesController.Notifications(homeId, notifications);
        Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
    }

    [TestMethod]
    public void PatchNotifcations_InvalidData_ShouldReturnBadRequest()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };
        var homeId = 1;
        var notifications = new NotificationConfigurationDto();
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(true);
        _homeServiceMock
            .Setup(service =>
                service.ConfigureNotification(
                    It.IsAny<int>(),
                    It.IsAny<NotificationConfigurationDto>()
                )
            )
            .Throws(new ArgumentException("Home not found"));
        var result = _homesController.Notifications(homeId, notifications);
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public void PatchNotifcations_ValidData_ShouldReturnOk()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };
        var homeId = 1;
        var notifications = new NotificationConfigurationDto();
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(true);
        _homeServiceMock.Setup(service =>
            service.ConfigureNotification(It.IsAny<int>(), It.IsAny<NotificationConfigurationDto>())
        );
        var result = _homesController.Notifications(homeId, notifications);
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }

    [TestMethod]
    public void GetNotifications_ValidDataButInvalidSession_ShouldReturnUnauthorized()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };
        var homeId = 1;
        bool? read = null;
        DateTime? since = null;
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(false);
        var result = _homesController.GetNotifications(homeId, read, since);
        Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
    }


    [TestMethod]
    public void PatchHomeName_ValidDataInvalidSession_ShouldReturnUnauthorized()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };
        var homeId = 1;

        var homeName = "HomeName";
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(false);
        var result = _homesController.UpdateHomeName(homeId, homeName);
        Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
    }

    [TestMethod]
    public void GetNotifications_ValidData_ShouldReturnOk()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User()
            {
                UserId = 1,
                Role = UserRoles.HomeOwner
            },
        };
        var homeId = 1;
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(true);
        _homeServiceMock
            .Setup(service => service.GetNotifications(It.IsAny<int>(), It.IsAny<int>(), null, null))
            .Returns(new List<HomeMemberNotification>());
        var result = _homesController.GetNotifications(homeId, null, null);
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void GetNotifications_InvalidData_ShouldReturnBadRequest()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };
        var homeId = 1;
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(true);
        _homeServiceMock
            .Setup(service => service.GetNotifications(It.IsAny<int>(), It.IsAny<int>(), null, null))
            .Throws(new ArgumentException("Home not found"));
        var result = _homesController.GetNotifications(homeId, null, null);
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public void CreateNotifications_ValidDataButInvalidSession_ShouldReturnUnauthorized()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };
        var homeId = 1;
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(false);
        var result = _homesController.CreateNotification(homeId, 1, "Test");
        Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
    }

    [TestMethod]
    public void CreateNotifications_InvalidData_ShouldReturnBadRequest()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };
        var homeId = 1;
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(true);
        _notificationServiceMock
            .Setup(service => service.CreateNotification(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
            .Throws(new ArgumentException("Invalid notification type"));
        var result = _homesController.CreateNotification(homeId, 1, "Test");
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }


    [TestMethod]
    public void PatchHomeName_InvalidData_ShouldReturnBadRequest()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };
        var homeId = 1;
        var homeName = "HomeName";
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(true);
        _homeServiceMock
                .Setup(service => service.UpdateHome(It.IsAny<int>(), It.IsAny<string>()))
                .Throws(new ArgumentException("Home not found"));
        var result = _homesController.UpdateHomeName(homeId, homeName);
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public void CreateNotifications_ValidData_ShouldReturnOk()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };
        var homeId = 1;
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(true);
        _notificationServiceMock
            .Setup(service => service.CreateNotification(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()));
        var result = _homesController.CreateNotification(homeId, 1, "Test");
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }

    [TestMethod]
    public void PostRoom_ValidDataButInvalidSession_ShouldReturnUnauthorized()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };
        var homeId = 1;
        var room = new RoomDto();
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(false);
        var result = _homesController.AddRoom(homeId, room);
        Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
    }

    [TestMethod]
    public void PostRoom_InvalidData_ShouldReturnBadRequest()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };
        var homeId = 1;
        var room = new RoomDto();
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(true);
        _homeServiceMock
            .Setup(service => service.AddRoom(It.IsAny<int>(), It.IsAny<RoomDto>()))
            .Throws(new ArgumentException("Home not found"));
        var result = _homesController.AddRoom(homeId, room);
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public void PostRoom_ValidData_ShouldReturnOk()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };
        var homeId = 1;
        var room = new RoomDto();
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(true);
        _homeServiceMock.Setup(service =>
            service.AddRoom(It.IsAny<int>(), It.IsAny<RoomDto>())
        );
        var result = _homesController.AddRoom(homeId, room);
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }

    [TestMethod]
    public void PatchHomeName_ValidData_ShouldReturnOk()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };
        var homeId = 1;
        var homeName = "HomeName";
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(true);
        _homeServiceMock.Setup(service =>
            service.UpdateHome(It.IsAny<int>(), It.IsAny<string>())
        );
        var result = _homesController.UpdateHomeName(homeId, homeName);
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }

    [TestMethod]
    public void GetNotifications_ValidDataInvalidSession_ShouldReturnUnauthorized()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };
        var homeId = 1;
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(false);
        var result = _homesController.GetNotifications(homeId, null, null);
        Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
    }

    [TestMethod]
    public void ChangeConnectedState_ValidDataInvalidSession_ShouldReturnUnauthorized()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };
        var homeId = 1;
        var deviceId = 1;
        var status = true;
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(false);
        var result = _homesController.ChangeHomeDeviceConnectedState(homeId, deviceId, status);
        Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
    }

    [TestMethod]
    public void ChangeConnectedState_InvalidData_ShouldReturnBadRequest()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };
        var homeId = 1;
        var deviceId = 1;
        var status = true;
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(true);
        _homeServiceMock
            .Setup(service =>
                service.ChangeConnectedState(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<bool>()
                )
            )
            .Throws(new ArgumentException("Home not found"));
        var result = _homesController.ChangeHomeDeviceConnectedState(homeId, deviceId, status);
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public void ChangeConnectedState_ValidData_ShouldReturnOk()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };
        var homeId = 1;
        var deviceId = 1;
        var status = true;
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(true);
        _homeServiceMock.Setup(service =>
            service.ChangeConnectedState(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())
        );
        var result = _homesController.ChangeHomeDeviceConnectedState(homeId, deviceId, status);
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }

    [TestMethod]
    public void ChangeLampState_ValidDataInvalidSession_ShouldReturnUnauthorized()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };
        var homeId = 1;
        var deviceId = 1;
        var newState = true;
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(false);
        var result = _homesController.ChangeLampState(homeId, deviceId, newState);
        Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
    }

    [TestMethod]
    public void ChangeLampState_InvalidData_ShouldReturnBadRequest()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User()
            {
                UserId = 1,
                Role = UserRoles.HomeOwner
            },
        };
        var homeId = 1;
        var deviceId = 1;
        var newState = true;
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(true);
        _homeServiceMock
            .Setup(service => service.ChangeLampState(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>()))
            .Throws(new ArgumentException("Home not found"));
        var result = _homesController.ChangeLampState(homeId, deviceId, newState);
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public void ChangeLampState_ValidData_ShouldReturnOk()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User()
            {
                UserId = 1,
                Role = UserRoles.HomeOwner
            },
        };
        var homeId = 1;
        var deviceId = 1;
        var newState = true;
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(true);
        _homeServiceMock.Setup(service =>
            service.ChangeLampState(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())
            );
        var result = _homesController.ChangeLampState(homeId, deviceId, newState);
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }

    [TestMethod]
    public void ChangeWindowState_ValidDataInvalidSession_ShouldReturnUnauthorized()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };
        var homeId = 1;
        var deviceId = 1;
        var newState = true;
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(false);
        var result = _homesController.ChangeWindowState(homeId, deviceId, newState);
        Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
    }

    [TestMethod]
    public void ChangeWindowState_InvalidData_ShouldReturnBadRequest()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User()
            {
                UserId = 1,
                Role = UserRoles.HomeOwner
            },
        };
        var homeId = 1;
        var deviceId = 1;
        var newState = true;
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(true);
        _homeServiceMock
            .Setup(service => service.ChangeWindowState(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>()))
            .Throws(new ArgumentException("Home not found"));
        var result = _homesController.ChangeWindowState(homeId, deviceId, newState);
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public void ChangeWindowState_ValidData_ShouldReturnOk()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User()
            {
                UserId = 1,
                Role = UserRoles.HomeOwner
            },
        };
        var homeId = 1;
        var deviceId = 1;
        var newState = true;
        _homeServiceMock
            .Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(true);
        _homeServiceMock.Setup(service =>
            service.ChangeWindowState(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())
        );
        var result = _homesController.ChangeWindowState(homeId, deviceId, newState);
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }
    
    [TestMethod]
    public void GetHomes_ShouldReturnOk()
    {
        _homeServiceMock.Setup(service => service.GetHomes(null, null, null))
            .Returns(new List<Home>(){new Home()});
        var result = _homesController.GetHomes(null, null, null);
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void GetRooms_ShouldReturnUnauthorized()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };
        _homeServiceMock.Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(false);
        
        var result = _homesController.GetRooms(1);
        Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
    }

    [TestMethod]
    public void GetRooms_ShouldReturnOk()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };

        _homeServiceMock.Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(true);

        _homeServiceMock.Setup(service => service.GetRooms(It.IsAny<int>()))
            .Returns(new List<Room>());

        var result = _homesController.GetRooms(1);
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }
    
    [TestMethod]
    public void GetRooms_InvalidData_ShouldReturnBadRequest()
    {
        _homesController.HttpContext.Items["Session"] = new Session()
        {
            User = new User() { UserId = 1 },
        };

        _homeServiceMock.Setup(service => service.IsMember(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(true);

        _homeServiceMock.Setup(service => service.GetRooms(It.IsAny<int>()))
            .Throws(new ArgumentException("Home not found"));

        var result = _homesController.GetRooms(1);
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }
}
