using smarthome.Domain;
using smarthome.Dtos;

namespace smarthome.IBusinessLogic;

public interface IHomeService
{
    public int AddHome(HomeDto dto);
    public IEnumerable<Home> GetHomes(int? skip = 0, int? take = 20, int? userId = null);
    public void AddMember(int homeId, string userEmail);
    public void AddDevice(int homeId, AddHomeDeviceDto dto);
    public void ChangeDeviceName(int homeId, int deviceId, string deviceName);
    public IEnumerable<HomeOwnerDto> GetMembers(int homeId);
    public IEnumerable<DeviceDto> GetDevices(int homeId, int? roomId = null);
    public void ConfigureNotification(
        int homeId,
        NotificationConfigurationDto notificationConfiguration
    );
    public IEnumerable<HomeMemberNotification> GetNotifications(
        int homeId,
        int userId,
        bool? read,
        DateTime? sinceDate
    );

    public bool IsMember(int homeId, int userId);
    public void AddRoom(int homeId, RoomDto room);
    public void UpdateHome(int homeId, string homeName);
    public void ChangeConnectedState(int homeId, int deviceId, bool isConnected);
    public void ChangeLampState(int homeId, int deviceId, bool newState);
    public void ChangeWindowState(int homeId, int deviceId, bool newState);
    public IEnumerable<Room> GetRooms(int homeId);
}
