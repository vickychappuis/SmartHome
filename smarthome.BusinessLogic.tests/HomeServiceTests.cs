using System.Linq.Expressions;
using Moq;
using smarthome.Domain;
using smarthome.Dtos;
using smarthome.IBusinessLogic;
using smarthome.IDataAccess;

namespace smarthome.BusinessLogic.tests;

[TestClass]
public class HomeServiceTests
{
    private Mock<IGenericRepository<Home>> _homeRepositoryMock;
    private Mock<IGenericRepository<User>> _userRepositoryMock;
    private Mock<IGenericRepository<Device>> _deviceRepositoryMock;
    private Mock<IGenericRepository<HomeMember>> _homeMemberRepositoryMock;
    private Mock<IGenericRepository<HomeDevice>> _homeDeviceRepositoryMock;
    private Mock<IGenericRepository<HomeMemberNotification>> _homeMemberNotificationRepositoryMock;
    private Mock<IGenericRepository<Room>> _roomRepositoryMock;
    private IHomeService _homeService;

    [TestInitialize]
    public void Setup()
    {
        _homeRepositoryMock = new Mock<IGenericRepository<Home>>();
        _userRepositoryMock = new Mock<IGenericRepository<User>>();
        _deviceRepositoryMock = new Mock<IGenericRepository<Device>>();
        _homeMemberRepositoryMock = new Mock<IGenericRepository<HomeMember>>();
        _homeDeviceRepositoryMock = new Mock<IGenericRepository<HomeDevice>>();
        _homeMemberNotificationRepositoryMock =
            new Mock<IGenericRepository<HomeMemberNotification>>();
        _roomRepositoryMock = new Mock<IGenericRepository<Room>>();

        _homeService = new HomeService(
            _homeRepositoryMock.Object,
            _userRepositoryMock.Object,
            _homeMemberRepositoryMock.Object,
            _deviceRepositoryMock.Object,
            _homeDeviceRepositoryMock.Object,
            _homeMemberNotificationRepositoryMock.Object,
            _roomRepositoryMock.Object
        );
    }

    [TestMethod]
    public void Should_Throw_Exception_AddMember_Member_Not_Found()
    {
        var homeId = 1;

        var userEmail = "test@test.com";

        _userRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<User, bool>>>(), null))
            .Returns((User)null);

        var exception = Assert.ThrowsException<Exception>(
            () => _homeService.AddMember(homeId, userEmail)
        );
        Assert.AreEqual(exception.Message, "User not found");
    }

    [TestMethod]
    public void Should_Throw_Exception_AddMember_Home_Not_Found()
    {
        var homeId = 1;
        var userEmail = "test@test.com";

        _userRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<User, bool>>>(), null))
            .Returns(new User());
        var exception = Assert.ThrowsException<Exception>(
            () => _homeService.AddMember(homeId, userEmail)
        );
        Assert.AreEqual(exception.Message, "Home not found");
    }

    [TestMethod]
    public void Should_Add_Home_Member()
    {
        var homeId = 1;

        var userEmail = "test@test.com";

        _userRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<User, bool>>>(), null))
            .Returns(new User());
        _homeRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Home, bool>>>(), null))
            .Returns(new Home { MaxMembers = 3 });

        _homeService.AddMember(homeId, userEmail);

        _homeMemberRepositoryMock.Verify(x => x.Insert(It.IsAny<HomeMember>()), Times.Once);
    }

    [TestMethod]
    public void AddMember_UserAlreadyMember_ShouldThrow()
    {
        var homeId = 1;

        var userEmail = "test@test.com";

        _userRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<User, bool>>>(), null))
            .Returns(new User { Email = userEmail });
        _homeRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Home, bool>>>(), null))
            .Returns(
                new Home
                {
                    HomeId = 1,
                    Address = "Address",
                    Latitude = 12.3,
                    Longitude = 28.4,
                    MaxMembers = 2,
                }
            );

        _homeMemberRepositoryMock
            .Setup(repo => repo.GetAll(It.IsAny<Func<HomeMember, bool>>(), It.IsAny<List<string>>()))
            .Returns([new HomeMember { User = new User { Email = userEmail } }, new HomeMember()]);

        Assert.ThrowsException<Exception>(() => _homeService.AddMember(homeId, userEmail));
        _homeMemberRepositoryMock.Verify(x => x.Insert(It.IsAny<HomeMember>()), Times.Never);
    }

    [TestMethod]
    public void AddMember_HomeFull_ShouldThrow()
    {
        var homeId = 1;

        var userEmail = "test@test.com";

        _userRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<User, bool>>>(), null))
            .Returns(new User());
        _homeRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Home, bool>>>(), null))
            .Returns(
                new Home
                {
                    HomeId = 1,
                    Address = "Address",
                    Latitude = 12.3,
                    Longitude = 28.4,
                    MaxMembers = 2,
                }
            );

        _homeMemberRepositoryMock
            .Setup(repo => repo.GetAll(It.IsAny<Func<HomeMember, bool>>(), It.IsAny<List<string>>()))
            .Returns([new HomeMember(), new HomeMember()]);

        Assert.ThrowsException<Exception>(() => _homeService.AddMember(homeId, userEmail));
        _homeMemberRepositoryMock.Verify(x => x.Insert(It.IsAny<HomeMember>()), Times.Never);
    }

    [TestMethod]
    public void Should_Return_Exception_AddDevice_Home_Not_Found()
    {
        var homeId = 1;

        var deviceId = 1;
        var dto = new AddHomeDeviceDto { DeviceId = deviceId };

        _homeRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Home, bool>>>(), It.IsAny<List<string>>()))
            .Returns((Home)null);

        var exception = Assert.ThrowsException<Exception>(
            () => _homeService.AddDevice(homeId, dto)
        );
        Assert.AreEqual(exception.Message, "Home not found");
    }

    [TestMethod]
    public void Should_Return_Exception_AddDevice_Device_Not_Found()
    {
        var homeId = 1;

        var deviceId = 1;
        _homeRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Home, bool>>>(), null))
            .Returns(new Home());

        var dto = new AddHomeDeviceDto { DeviceId = deviceId };
        var exception = Assert.ThrowsException<Exception>(
            () => _homeService.AddDevice(homeId, dto)
        );
        Assert.AreEqual(exception.Message, "Device not found");
    }

    [TestMethod]
    public void Should_Add_Device()
    {
        var homeId = 1;

        var deviceId = 1;
        var dto = new AddHomeDeviceDto { DeviceId = deviceId };

        _homeRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Home, bool>>>(), null))
            .Returns(new Home());
        _deviceRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Device, bool>>>(), null))
            .Returns(new WindowSensor());

        _homeService.AddDevice(homeId, dto);

        _homeDeviceRepositoryMock.Verify(x => x.Insert(It.IsAny<HomeDevice>()), Times.Once);
    }

    [TestMethod]
    public void AddDevice_ShouldSetDeviceNameToTheAddedDeviceName()
    {
        int homeId = 1;
        int deviceId = 1;
        var dto = new AddHomeDeviceDto { DeviceId = deviceId };
        _homeRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Home, bool>>>(), null))
            .Returns(new Home());
        _deviceRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Device, bool>>>(), null))
            .Returns(new WindowSensor { Name = "Window Sensor" });

        _homeService.AddDevice(homeId, dto);
        _homeDeviceRepositoryMock.Verify(x => x.Insert(It.Is<HomeDevice>(hd => hd.DeviceName == "Window Sensor")), Times.Once);
    }

    [TestMethod]
    public void ChangeDeviceName_HomeDeviceNotFound()
    {
        int homeId = 1;
        int deviceId = 1;
        string deviceName = "Window Sensor";

        _homeDeviceRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<HomeDevice, bool>>>(), null))
            .Returns((HomeDevice)null);

        Assert.ThrowsException<Exception>(() => _homeService.ChangeDeviceName(homeId, deviceId, deviceName));
    }

    [TestMethod]
    public void ChangeDeviceName_ShouldChangeDeviceName()
    {
        int homeId = 1;
        int deviceId = 1;
        string deviceName = "Window Sensor";

        _homeDeviceRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<HomeDevice, bool>>>(), null))
            .Returns(new HomeDevice());

        _homeService.ChangeDeviceName(homeId, deviceId, deviceName);

        _homeDeviceRepositoryMock.Verify(x => x.Update(It.IsAny<HomeDevice>()), Times.Once);
    }

    [TestMethod]
    public void Should_Return_Exception_GetMembers_Home_Not_Found()
    {
        var homeId = 1;

        _homeRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Home, bool>>>(), null))
            .Returns((Home)null);

        var exception = Assert.ThrowsException<Exception>(() => _homeService.GetMembers(homeId));
        Assert.AreEqual(exception.Message, "Home not found");
    }

    [TestMethod]
    public void Should_Return_No_Members_GetMembers()
    {
        var homeId = 1;

        _homeRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Home, bool>>>(), null))
            .Returns(
                new Home
                {
                    HomeId = 1,
                    Address = "Address",
                    Latitude = 12.3,
                    Longitude = 28.4,
                    MaxMembers = 8,
                }
            );

        var members = _homeService.GetMembers(homeId);
        Assert.IsFalse(members.Any());
    }

    [TestMethod]
    public void Should_Return_Members_Get_Members()
    {
        var homeId = 1;

        _homeRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Home, bool>>>(), null))
            .Returns(
                new Home
                {
                    HomeId = 1,
                    Address = "Address",
                    Latitude = 12.3,
                    Longitude = 28.4,
                    MaxMembers = 8,
                }
            );

        _homeMemberRepositoryMock
            .Setup(repo => repo.GetAll(It.IsAny<Func<HomeMember, bool>>(), null))
            .Returns(
                new List<HomeMember>()
                {
                    new HomeMember()
                    {
                        User = new User(),
                        UserId = 1,
                        Home = new Home(),
                        HomeId = 1,
                    },
                    new HomeMember()
                    {
                        User = new User(),
                        UserId = 2,
                        Home = new Home(),
                        HomeId = 1,
                    },
                }
            );

        _userRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<User, bool>>>(), null))
            .Returns(new User());

        var members = _homeService.GetMembers(homeId);
        Assert.IsTrue(members.Any());
    }

    [TestMethod]
    public void Should_Return_Exception_GetDevices_Home_Not_Found()
    {
        var homeId = 1;

        var exception = Assert.ThrowsException<Exception>(() => _homeService.GetDevices(homeId));
        Assert.AreEqual(exception.Message, "Home not found");
    }

    [TestMethod]
    public void Should_Return_No_Devices_GetDevices()
    {
        var homeId = 1;

        _homeRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Home, bool>>>(), null))
            .Returns(
                new Home
                {
                    HomeId = 1,
                    Address = "Address",
                    Latitude = 12.3,
                    Longitude = 28.4,
                    MaxMembers = 8,
                }
            );

        var devices = _homeService.GetDevices(homeId);
        Assert.IsFalse(devices.Any());
    }

    [TestMethod]
    public void Should_Return_Devices_Get_Devices()
    {
        var homeId = 1;

        _homeRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Home, bool>>>(), null))
            .Returns(
                new Home
                {
                    HomeId = 1,
                    Address = "Address",
                    Latitude = 12.3,
                    Longitude = 28.4,
                    MaxMembers = 8,
                }
            );

        var homeDevices = new List<HomeDevice>
        {
            new HomeDevice
            {
                Device = new WindowSensor
                {
                    Description = "Description",
                    DeviceId = 1,
                    ImageUrl = "ImageUrl",
                    Model = "Model",
                    Name = "Name",
                },
                DeviceId = 1,
                Home = new Home(),
                HomeId = 1,
            }
        };

        _homeDeviceRepositoryMock
            .Setup(repo => repo.GetAll(It.IsAny<Func<HomeDevice, bool>>(), It.IsAny<List<string>>()))
            .Returns(homeDevices);

        var devices = _homeService.GetDevices(homeId);
        Assert.IsTrue(devices.Any());
    }

    [TestMethod]
    public void Should_Throw_Exception_ConfigureNotification_User_Not_Found()
    {
        var homeId = 1;

        var notifcationConfigurationDto = new NotificationConfigurationDto { UserId = 1 };

        _userRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<User, bool>>>(), null))
            .Returns((User)null);

        var exception = Assert.ThrowsException<Exception>(
            () => _homeService.ConfigureNotification(homeId, notifcationConfigurationDto)
        );
        Assert.AreEqual(exception.Message, "User not found");
    }

    [TestMethod]
    public void Should_Throw_Exception_ConfigureNotification_Home_Not_Found()
    {
        var homeId = 1;

        var notificationConfigurationDto = new NotificationConfigurationDto { UserId = 1 };

        _userRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<User, bool>>>(), null))
            .Returns(new User());

        var exception = Assert.ThrowsException<Exception>(
            () => _homeService.ConfigureNotification(homeId, notificationConfigurationDto)
        );
        Assert.AreEqual(exception.Message, "Home not found");
    }

    [TestMethod]
    public void Should_Throw_Exception_ConfigureNotification_Member_Not_Found_In_Home()
    {
        var homeId = 1;

        var notificationConfigurationDto = new NotificationConfigurationDto { UserId = 1 };

        _userRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<User, bool>>>(), null))
            .Returns(new User());
        _homeRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Home, bool>>>(), null))
            .Returns(new Home());

        var exception = Assert.ThrowsException<Exception>(
            () => _homeService.ConfigureNotification(homeId, notificationConfigurationDto)
        );
        Assert.AreEqual(exception.Message, "Member not found in home");
    }

    [TestMethod]
    public void Should_Configure_Notification()
    {
        var homeId = 1;

        var notificationConfigurationDto = new NotificationConfigurationDto
        {
            UserId = 1,
            Allowed = true,
        };

        _userRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<User, bool>>>(), null))
            .Returns(new User());
        _homeRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Home, bool>>>(), null))
            .Returns(new Home());
        _homeMemberRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<HomeMember, bool>>>(), null))
            .Returns(new HomeMember());

        _homeService.ConfigureNotification(homeId, notificationConfigurationDto);

        _homeMemberRepositoryMock.Verify(x => x.Update(It.IsAny<HomeMember>()), Times.Once);
    }

    [TestMethod]
    public void AddHome_Should_Add_Home()
    {
        var homeDto = new HomeDto
        {
            HomeId = 1,
            Address = "Address",
            Latitude = 12.3,
            Longitude = 28.4,
            MaxMembers = 8,
            HomeName = "name"
        };

        _homeService.AddHome(homeDto);

        _homeRepositoryMock.Verify(x => x.Insert(It.IsAny<Home>()), Times.Once);
    }

    [TestMethod]
    public void GetHomes_WithNoHomes_ShouldReturnEmpty()
    {
        _homeRepositoryMock.Setup(x => x.GetAll()).Returns(new List<Home>());

        var result = _homeService.GetHomes();

        Assert.AreEqual(0, result.Count());
    }

    [TestMethod]
    public void GetHomes_WithHomes_ShouldReturnHomes()
    {
        var homes = new List<Home>
        {
            new Home
            {
                HomeId = 1,
                Address = "Address",
                Latitude = 12.3,
                Longitude = 28.4,
                MaxMembers = 8,
            },
            new Home
            {
                HomeId = 2,
                Address = "Address",
                Latitude = 12.3,
                Longitude = 28.4,
                MaxMembers = 8,
            },
        };

        _homeRepositoryMock.Setup(x => x.GetAll()).Returns(homes);

        var result = _homeService.GetHomes();

        Assert.AreEqual(2, result.Count());
    }

    [TestMethod]
    public void GetHomes_WithUserId_ShouldReturnHomes()
    {
        var homes = new List<Home>()
        {
            
        };
        
        _homeMemberRepositoryMock
            .Setup(repo => repo.GetAll(It.IsAny<Func<HomeMember, bool>>(), It.IsAny<List<string>>()))
            .Returns(new List<HomeMember>
            {
                new HomeMember
                {
                    HomeId = 1,
                    UserId = 1,
                },
                new HomeMember
                {
                    HomeId = 2,
                    UserId = 1,
                },
            });
        
        _homeRepositoryMock.Setup(x => x.GetAll(It.IsAny<Func<Home, bool>>(), null)).Returns(homes); 
        var result = _homeService.GetHomes(userId: 1);
    
        Assert.AreEqual(0, result.Count());
    }

    [TestMethod]
    public void IsMember_NoHomeFound_ShouldReturnException()
    {
        var homeId = 1;
        var userId = 1;

        _homeRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Home, bool>>>(), null))
            .Returns((Home)null);

        var exception = Assert.ThrowsException<Exception>(
            () => _homeService.IsMember(homeId, userId)
        );

        Assert.AreEqual("Home not found", exception.Message);
    }

    [TestMethod]
    public void IsMember_NoMemberFound_ShouldReturnFalse()
    {
        var homeId = 1;
        var userId = 1;

        _homeRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Home, bool>>>(), null))
            .Returns(new Home());

        _homeMemberRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<HomeMember, bool>>>(), null))
            .Returns((HomeMember)null);

        var result = _homeService.IsMember(homeId, userId);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsMember_MemberFound_ShouldReturnTrue()
    {
        var homeId = 1;
        var userId = 1;

        _homeRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Home, bool>>>(), null))
            .Returns(new Home());

        _homeMemberRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<HomeMember, bool>>>(), null))
            .Returns(new HomeMember());

        var result = _homeService.IsMember(homeId, userId);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void GetNotifications_HomeNotFound_ShouldThrowException()
    {
        var homeId = 1;
        var userId = 2;

        _homeRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Home, bool>>>(), null))
            .Returns((Home)null);

        var ex = Assert.ThrowsException<Exception>(
            () => _homeService.GetNotifications(homeId, userId, null, null)
        );
        Assert.AreEqual("Home not found", ex.Message);
    }

    [TestMethod]
    public void GetNotifications_UserNotFound_ShouldThrowException()
    {
        var homeId = 1;
        var userId = 2;

        _homeRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Home, bool>>>(), null))
            .Returns(new Home { HomeId = homeId });

        _userRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<User, bool>>>(), null))
            .Returns((User)null);

        var ex = Assert.ThrowsException<Exception>(
            () => _homeService.GetNotifications(homeId, userId, null, null)
        );
        Assert.AreEqual("User not found", ex.Message);
    }

    [TestMethod]
    public void GetNotifications_ValidHomeAndUser_ShouldReturnNotifications()
    {
        var homeId = 1;
        var userId = 2;
        var notifications = new List<HomeMemberNotification>
        {
            new HomeMemberNotification
            {
                HomeId = homeId,
                UserId = userId,
                Read = false,
            },
            new HomeMemberNotification
            {
                HomeId = homeId,
                UserId = userId,
                Read = true,
            },
        };

        _homeRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Home, bool>>>(), null))
            .Returns(new Home { HomeId = homeId });

        _userRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<User, bool>>>(), null))
            .Returns(new User { UserId = userId });

        _homeMemberNotificationRepositoryMock
            .Setup(repo => repo.GetAll(It.IsAny<Func<HomeMemberNotification, bool>>(), It.IsAny<List<string>>()))
            .Returns(notifications);

        var result = _homeService.GetNotifications(homeId, userId, null, null);

        Assert.AreEqual(2, result.Count());
    }

    [TestMethod]
    public void GetNotifications_NoNotifications_ShouldReturnEmptyEnumerable()
    {
        var homeId = 1;
        var userId = 2;

        _homeRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Home, bool>>>(), null))
            .Returns(new Home { HomeId = homeId });

        _userRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<User, bool>>>(), null))
            .Returns(new User { UserId = userId });

        _homeMemberNotificationRepositoryMock
            .Setup(repo => repo.GetAll(It.IsAny<Func<HomeMemberNotification, bool>>(), null))
            .Returns(new List<HomeMemberNotification>());

        var result = _homeService.GetNotifications(homeId, userId, null, null);

        Assert.IsFalse(result.Any());
    }

    [TestMethod]
    public void ChangeConnectedState_HomeNotFound_ShouldThrowException()
    {
        var homeId = 1;
        var deviceId = 2;
        var isConnected = true;

        _homeRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Home, bool>>>(), null))
            .Returns((Home)null);

        var ex = Assert.ThrowsException<Exception>(
            () => _homeService.ChangeConnectedState(homeId, deviceId, isConnected)
        );
        Assert.AreEqual("Device not found", ex.Message);
    }

    [TestMethod]
    public void ChangeConnectedState_DeviceNotFound_ShouldThrowException()
    {
        var homeId = 1;
        var deviceId = 2;
        var isConnected = true;

        _homeRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Home, bool>>>(), null))
            .Returns(new Home { HomeId = homeId });

        _deviceRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Device, bool>>>(), null))
            .Returns((Device)null);

        var ex = Assert.ThrowsException<Exception>(
            () => _homeService.ChangeConnectedState(homeId, deviceId, isConnected)
        );
        Assert.AreEqual("Device not found", ex.Message);
    }

    [TestMethod]
    public void ChangeConnectedState_DeviceFound_ShouldUpdateConnectedState()
    {
        var homeId = 1;
        var deviceId = 2;
        var isConnected = true;

        _homeRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Home, bool>>>(), null))
            .Returns(new Home { HomeId = homeId });

        _deviceRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Device, bool>>>(), null))
            .Returns(new WindowSensor { DeviceId = deviceId });

        _homeDeviceRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<HomeDevice, bool>>>(), null))
            .Returns(new HomeDevice { DeviceId = deviceId });

        _homeService.ChangeConnectedState(homeId, deviceId, isConnected);

        _homeDeviceRepositoryMock.Verify(x => x.Update(It.IsAny<HomeDevice>()), Times.Once);
    }
    [TestMethod]
    public void AddRoom_HomeNotFound_ShouldThrowException()
    {
        var homeId = 1;
        var roomDto = new RoomDto();

        _homeRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Home, bool>>>(), null))
            .Returns((Home)null);

        var ex = Assert.ThrowsException<Exception>(
            () => _homeService.AddRoom(homeId, roomDto)
        );
        Assert.AreEqual("Home not found", ex.Message);
    }

    [TestMethod]
    public void AddRoom_ShouldAddRoom()
    {
        var homeId = 1;
        var roomDto = new RoomDto();

        _homeRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Home, bool>>>(), null))
            .Returns(new Home()
            {
                Rooms = new List<Room>()
            });

        _homeService.AddRoom(homeId, roomDto);

        _roomRepositoryMock.Verify(x => x.Insert(It.IsAny<Room>()), Times.Once);
    }

    [TestMethod]
    public void AddDeviceToRoom_HomeNotFound_ShouldThrowException()
    {
        var homeId = 1;
        var roomId = 1;
        var deviceId = 1;
        var dto = new AddHomeDeviceDto { DeviceId = deviceId, RoomId = roomId };

        _homeRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Home, bool>>>(), null))
            .Returns((Home)null);

        var ex = Assert.ThrowsException<Exception>(
            () => _homeService.AddDevice(homeId, dto)
        );
        Assert.AreEqual("Home not found", ex.Message);
    }

    [TestMethod]
    public void AddDeviceToRoom_RoomNotFound_ShouldThrowException()
    {
        var homeId = 1;
        var roomId = 1;
        var deviceId = 1;
        var dto = new AddHomeDeviceDto { DeviceId = deviceId, RoomId = roomId };
        _homeRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Home, bool>>>(), null))
            .Returns(new Home()
            {
                Rooms = new List<Room>()
            });

        _deviceRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Device, bool>>>(), null))
            .Returns(new WindowSensor()
            {
                DeviceId = deviceId
            });

        var ex = Assert.ThrowsException<Exception>(
            () => _homeService.AddDevice(homeId, dto)
        );
        Assert.AreEqual("Room not found", ex.Message);
    }

    [TestMethod]
    public void AddDeviceToRoom_DeviceNotFound_ShouldThrowException()
    {
        var homeId = 1;
        var roomId = 1;
        var deviceId = 1;
        var dto = new AddHomeDeviceDto { DeviceId = deviceId, RoomId = roomId };
        _homeRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Home, bool>>>(), null))
            .Returns(new Home()
            {
                Rooms = new List<Room>()
                {
                    new Room(new RoomDto()
                    {
                        HomeId = 1,
                        RoomName = "Room"
                    })
                }
            });

        var ex = Assert.ThrowsException<Exception>(
            () => _homeService.AddDevice(homeId, dto)
        );
        Assert.AreEqual("Device not found", ex.Message);
    }

    [TestMethod]
    public void AddDeviceToRoom_ShouldAddDeviceToRoom()
    {
        var homeId = 1;
        var roomId = 1;
        var deviceId = 1;
        var dto = new AddHomeDeviceDto { DeviceId = deviceId, RoomId = roomId };
        _homeRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Home, bool>>>(), null))
            .Returns(new Home()
            {
                Rooms = new List<Room>()
                {
                    new Room(new RoomDto()
                    {
                        HomeId = 1,
                        RoomName = "Room"
                    })
                }
            });

        _deviceRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Device, bool>>>(), null))
            .Returns(new WindowSensor()
            {
                DeviceId = deviceId
            });

        _roomRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Room, bool>>>(), null))
            .Returns(new Room()
            {
                RoomId = roomId
            });

        _homeDeviceRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<HomeDevice, bool>>>(), null))
            .Returns(new HomeDevice());

        _homeService.AddDevice(homeId, dto);
        _homeDeviceRepositoryMock.Verify(x => x.Insert(It.IsAny<HomeDevice>()), Times.Once);
    }

    [TestMethod]
    public void UpdateHome_HomeNotFound_ShouldThrowException()
    {
        var homeId = 1;
        var homeName = "Home";

        _homeRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Home, bool>>>(), null))
            .Returns((Home)null);

        var ex = Assert.ThrowsException<Exception>(
            () => _homeService.UpdateHome(homeId, homeName)
        );
        Assert.AreEqual("Home not found", ex.Message);
    }

    [TestMethod]
    public void UpdateHome_ShouldUpdateHome()
    {
        var homeId = 1;
        var homeName = "Home";

        _homeRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Home, bool>>>(), null))
            .Returns(new Home());

        _homeService.UpdateHome(homeId, homeName);
        _homeRepositoryMock.Verify(x => x.Update(It.IsAny<Home>()), Times.Once);
    }

    [TestMethod]
    public void ChangeLampState_HomeDeviceNotFound_ShouldThrowException()
    {
        var homeId = 1;
        var deviceId = 1;
        var state = true;

        _homeDeviceRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<HomeDevice, bool>>>(), null))
            .Returns((HomeDevice)null);

        var ex = Assert.ThrowsException<ArgumentException>(
            () => _homeService.ChangeLampState(homeId, deviceId, state)
        );
        Assert.AreEqual($"Device with ID {deviceId} not found in home with ID {homeId}.", ex.Message);
    }

    [TestMethod]
    public void ChangeLampState_NotALamp()
    {
        var homeId = 1;
        var deviceId = 1;
        var state = true;

        _homeDeviceRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<HomeDevice, bool>>>(), null))
            .Returns(new HomeDevice { Device = new WindowSensor() });

        var ex = Assert.ThrowsException<InvalidOperationException>(
            () => _homeService.ChangeLampState(homeId, deviceId, state));

        Assert.AreEqual("The specified device is not a Smart Lamp.", ex.Message);
    }

    [TestMethod]
    public void ChangeLampState_ShouldChangeLampState()
    {
        var homeId = 1;
        var deviceId = 1;
        var state = true;

        _homeDeviceRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<HomeDevice, bool>>>(), null))
            .Returns(new HomeDevice()
            {
                Device = new SmartLamp()
            });

        _homeService.ChangeLampState(homeId, deviceId, state);

        _homeDeviceRepositoryMock.Verify(x => x.Update(It.IsAny<HomeDevice>()), Times.Once);
    }

    [TestMethod]
    public void ChangeWindowState_HomeDeviceNotFound_ShouldThrowException()
    {
        var homeId = 1;
        var deviceId = 1;
        var state = true;

        _homeDeviceRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<HomeDevice, bool>>>(), null))
            .Returns((HomeDevice)null);

        var ex = Assert.ThrowsException<ArgumentException>(
            () => _homeService.ChangeWindowState(homeId, deviceId, state)
        );
        Assert.AreEqual($"Device with ID {deviceId} not found in home with ID {homeId}.", ex.Message);
    }

    [TestMethod]
    public void ChangeWindowState_NotAWindowSensor()
    {
        var homeId = 1;
        var deviceId = 1;
        var state = true;

        _homeDeviceRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<HomeDevice, bool>>>(), It.IsAny<List<string>>()))
            .Returns(new HomeDevice { Device = new SmartLamp { DeviceId = deviceId } });

        var ex = Assert.ThrowsException<InvalidOperationException>(
            () => _homeService.ChangeWindowState(homeId, deviceId, state));

        Assert.AreEqual("The specified device is not a Window Sensor.", ex.Message);
    }

    [TestMethod]
    public void ChangeWindowState_ShouldChangeWindowState()
    {
        var homeId = 1;
        var deviceId = 1;
        var state = true;

        _homeDeviceRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<HomeDevice, bool>>>(), It.IsAny<List<string>>()))
            .Returns(new HomeDevice()
            {
                Device = new WindowSensor()
            });

        _homeService.ChangeWindowState(homeId, deviceId, state);

        _homeDeviceRepositoryMock.Verify(x => x.Update(It.IsAny<HomeDevice>()), Times.Once);
    }

    [TestMethod]
    public void GetRooms_ShouldGetRooms()
    {
        var homeId = 1;
        _roomRepositoryMock.Setup(repo => repo.GetAll(It.IsAny<Func<Room, bool>>(), null)).Returns(new List<Room>()
        {
            new Room()
            {
                RoomId = 1,
                RoomName = "Room"
            }
        });
        
        var result = _homeService.GetRooms(homeId);
        Assert.AreEqual(1, result.Count());
    }
}
