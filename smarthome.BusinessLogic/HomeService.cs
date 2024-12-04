using System.Linq;
using smarthome.Domain;
using smarthome.Dtos;
using smarthome.IBusinessLogic;
using smarthome.IDataAccess;

namespace smarthome.BusinessLogic;

public class HomeService : IHomeService
{
    private readonly IGenericRepository<Home> _homeRepository;
    private readonly IGenericRepository<User> _userRepository;
    private readonly IGenericRepository<Device> _deviceRepository;
    private readonly IGenericRepository<HomeMember> _homeMemberRepository;
    private readonly IGenericRepository<HomeDevice> _homeDeviceRepository;
    private readonly IGenericRepository<HomeMemberNotification> _homeMemberNotificationRepository;
    private readonly IGenericRepository<Room> _roomRepository;

    public HomeService(
        IGenericRepository<Home> homeRepository,
        IGenericRepository<User> userRepository,
        IGenericRepository<HomeMember> homeMemberRepository,
        IGenericRepository<Device> deviceRepository,
        IGenericRepository<HomeDevice> homeDeviceRepository,
        IGenericRepository<HomeMemberNotification> homeMemberNotificationRepository,
        IGenericRepository<Room> roomRepository
    )
    {
        _homeRepository = homeRepository;
        _userRepository = userRepository;
        _deviceRepository = deviceRepository;
        _homeMemberRepository = homeMemberRepository;
        _homeDeviceRepository = homeDeviceRepository;
        _homeMemberNotificationRepository = homeMemberNotificationRepository;
        _roomRepository = roomRepository;
    }

    public int AddHome(HomeDto dto)
    {
        var home = new Home(dto);
        _homeRepository.Insert(home);
        return home.HomeId;
    }

    public IEnumerable<Home> GetHomes(int? skip = 0, int? take = 20, int? userId = null)
    {
        if (userId.HasValue)
        {
            var homeIds = _homeMemberRepository.GetAll()
                                               .Where(x => x.UserId == userId)
                                               .Select(x => x.HomeId);

            var homes = _homeRepository.GetAll()
                                    .Where(x => homeIds.Contains(x.HomeId));
            return homes;
        }
        else
        {
            var homes = _homeRepository.GetAll();
            return homes;
        }

    }

    public void AddMember(int homeId, string userEmail)
    {
        var homeMember = _userRepository.Get(x => x.Email == userEmail);
        if (homeMember == null)
        {
            throw new Exception("User not found");
        }
        var home = _homeRepository.Get(x => x.HomeId == homeId);
        if (home == null)
        {
            throw new Exception("Home not found");
        }
        var members = _homeMemberRepository.GetAll(hm => hm.HomeId == homeId, ["User"]);

        if (members.Any(m => m.User?.Email == userEmail))
        {
            throw new Exception("User is already a member of home");
        }

        if (members.Count() >= home.MaxMembers)
        {
            throw new Exception("Home is full");
        }

        var homeMemberEntity = new HomeMember
        {
            HomeId = home.HomeId,
            Home = home,
            UserId = homeMember.UserId,
            User = homeMember,
        };
        _homeMemberRepository.Insert(homeMemberEntity);
    }

    public void AddDevice(int homeId, AddHomeDeviceDto dto)
    {
        var home = _homeRepository.Get(x => x.HomeId == homeId);
        if (home == null)
        {
            throw new Exception("Home not found");
        }
        var device = _deviceRepository.Get(x => x.DeviceId == dto.DeviceId);
        if (device == null)
        {
            throw new Exception("Device not found");
        }

        if (dto.RoomId.HasValue)
        {
            var room = _roomRepository.Get(x => x.RoomId == dto.RoomId && x.HomeId == homeId);
            if (room == null)
            {
                throw new Exception("Room not found");
            }
        }

        var homeDevice = new HomeDevice
        {
            HomeId = home.HomeId,
            Home = home,
            DeviceId = device.DeviceId,
            Device = device,
            DeviceName = device.Name,
            RoomId = dto.RoomId,
        };

        _homeDeviceRepository.Insert(homeDevice);
    }

    public void ChangeDeviceName(int homeId, int deviceId, string deviceName)
    {
        var homeDevice = _homeDeviceRepository.Get(x => x.HomeId == homeId && x.DeviceId == deviceId);
        if (homeDevice == null)
        {
            throw new Exception("Device not found in home");
        }
        homeDevice.DeviceName = deviceName;
        _homeDeviceRepository.Update(homeDevice);
    }

    public IEnumerable<HomeOwnerDto> GetMembers(int homeId)
    {
        if (_homeRepository.Get(x => x.HomeId == homeId) == null)
        {
            throw new Exception("Home not found");
        }
        var homeMembers = _homeMemberRepository.GetAll(x => x.HomeId == homeId).ToList();
        var members = new List<HomeOwnerDto>();

        foreach (var homeMember in homeMembers)
        {
            var user = _userRepository.Get(x => x.UserId == homeMember.UserId);
            if (user != null)
            {
                members.Add(new HomeOwnerDto
                {
                    UserId = user.UserId,
                    FirstName = user.FirstName,
                    Email = user.Email,
                    CanAddDevice = homeMember.CanAddDevice,
                    CanListDevices = homeMember.CanListDevices,
                    CanReceiveNotifications = homeMember.CanReceiveNotifications,
                    ImageUrl = user.ImageUrl
                });
            }
        }

        return members;
    }


    public IEnumerable<DeviceDto> GetDevices(int homeId, int? roomId = null)
    {
        if (_homeRepository.Get(x => x.HomeId == homeId) == null)
        {
            throw new Exception("Home not found");
        }

        var homeDevices = _homeDeviceRepository.GetAll(x => x.HomeId == homeId && (roomId == null || x.RoomId == roomId), ["Device", "Room"]);

        if (homeDevices == null)
        {
            throw new Exception("No devices found in home");
        }

        var deviceDtos = new List<DeviceDto>();
        foreach (var device in homeDevices)
        {
            deviceDtos.Add(
                new DeviceDto
                {
                    Name = device.Device.Name,
                    Model = device.Device.Model,
                    Description = device.Device.Description,
                    ImageUrl = device.Device.ImageUrl,
                    RoomId = device.RoomId,
                }
            );
        }
        return deviceDtos;
    }

    public void ConfigureNotification(int homeId, NotificationConfigurationDto configurationDto)
    {
        var homeMember = _userRepository.Get(x => x.UserId == configurationDto.UserId);
        if (homeMember == null)
        {
            throw new Exception("User not found");
        }
        var home = _homeRepository.Get(x => x.HomeId == homeId);
        if (home == null)
        {
            throw new Exception("Home not found");
        }
        var homeMemberEntity = _homeMemberRepository.Get(x =>
            x.HomeId == home.HomeId && x.UserId == homeMember.UserId
        );
        if (homeMemberEntity == null)
        {
            throw new Exception("Member not found in home");
        }
        homeMemberEntity.CanReceiveNotifications = configurationDto.Allowed;
        _homeMemberRepository.Update(homeMemberEntity);
    }

    public bool IsMember(int homeId, int userId)
    {
        if (_homeRepository.Get(x => x.HomeId == homeId) == null)
        {
            throw new Exception("Home not found");
        }
        return _homeMemberRepository.Get(x => x.HomeId == homeId && x.UserId == userId) != null;
    }

    public IEnumerable<HomeMemberNotification> GetNotifications(int homeId, int userId, bool? read, DateTime? sinceDate)
    {
        if (_homeRepository.Get(x => x.HomeId == homeId) == null)
        {
            throw new Exception("Home not found");
        }

        if (_userRepository.Get(x => x.UserId == userId) == null)
        {
            throw new Exception("User not found");
        }

        return _homeMemberNotificationRepository.GetAll(
            x => x.HomeId == homeId &&
                 x.UserId == userId &&
                 (read == null || x.Read == read) &&
                 (sinceDate == null || DateTime.Parse(x.Notification.CreatedAt) >= sinceDate),
            ["Notification"]
        );
    }
    public void UpdateHome(int homeId, string homeName)
    {
        var home = _homeRepository.Get(x => x.HomeId == homeId);
        if (home == null)
        {
            throw new Exception("Home not found");
        }
        home.HomeName = homeName;
        _homeRepository.Update(home);
    }

    public void ChangeConnectedState(int homeId, int deviceId, bool isConnected)
    {
        var homeDevice = _homeDeviceRepository.Get(x => x.HomeId == homeId && x.DeviceId == deviceId);
        if (homeDevice == null)
        {
            throw new Exception("Device not found");
        }
        homeDevice.IsConnected = isConnected;
        _homeDeviceRepository.Update(homeDevice);
    }

    public void AddRoom(int homeId, RoomDto dto)
    {
        var home = _homeRepository.Get(x => x.HomeId == homeId);
        if (home == null)
        {
            throw new Exception("Home not found");
        }
        var room = new Room(dto);
        _roomRepository.Insert(room);
    }

    public void ChangeLampState(int homeId, int deviceId, bool state)
    {
        var homeDevice = _homeDeviceRepository.Get(x => x.HomeId == homeId && x.DeviceId == deviceId);

        if (homeDevice == null)
        {
            throw new ArgumentException($"Device with ID {deviceId} not found in home with ID {homeId}.");
        }

        if (homeDevice.Device is not SmartLamp smartLamp)
        {
            throw new InvalidOperationException("The specified device is not a Smart Lamp.");
        }

        smartLamp.ChangeLampState(state);
        _homeDeviceRepository.Update(homeDevice);
    }

    public void ChangeWindowState(int homeId, int deviceId, bool state)
    {
        var homeDevice = _homeDeviceRepository.Get(x => x.HomeId == homeId && x.DeviceId == deviceId, ["Device"]);

        if (homeDevice == null)
        {
            throw new ArgumentException($"Device with ID {deviceId} not found in home with ID {homeId}.");
        }

        if (homeDevice.Device is not WindowSensor windowSensor)
        {
            throw new InvalidOperationException("The specified device is not a Window Sensor.");
        }

        homeDevice.IsOpen = state;
        _homeDeviceRepository.Update(homeDevice);
    }

    public IEnumerable<Room> GetRooms(int homeId)
    {
        return _roomRepository.GetAll(x => x.HomeId == homeId);
    }

}
