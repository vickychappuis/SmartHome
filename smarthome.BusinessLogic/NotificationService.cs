using System.Globalization;
using smarthome.Domain;
using smarthome.IBusinessLogic;
using smarthome.IDataAccess;

namespace smarthome.BusinessLogic
{
    public class NotificationService : INotificationService
    {
        private readonly IGenericRepository<Notification> _notificationRepository;
        private readonly IGenericRepository<Device> _deviceRepository;
        private readonly IGenericRepository<HomeDevice> _homeDeviceRepository;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<HomeMember> _homeMemberRepository;
        private readonly IGenericRepository<HomeMemberNotification> _homeMemberNotificationRepository;

        public NotificationService(
            IGenericRepository<Notification> notificationRepository,
            IGenericRepository<Device> deviceRepository,
            IGenericRepository<HomeDevice> homeDeviceRepository,
            IGenericRepository<User> userRepository,
            IGenericRepository<HomeMember> homeMemberRepository,
            IGenericRepository<HomeMemberNotification> homeMemberNotificationRepository
        )
        {
            _notificationRepository = notificationRepository;
            _deviceRepository = deviceRepository;
            _homeDeviceRepository = homeDeviceRepository;
            _userRepository = userRepository;
            _homeMemberNotificationRepository = homeMemberNotificationRepository;
            _homeMemberRepository = homeMemberRepository;
        }

        public void CreateNotification(int deviceId, int homeId, string notificationType, int? userId)
        {
            Notification notification;

            switch (notificationType)
            {
                case "SecurityCameraMotion":
                    notification = DetectSecurityCameraMotion(deviceId, homeId);
                    break;
                case "MotionSensorMotion":
                    notification = DetectMotionSensorMotion(deviceId, homeId);
                    break;
                case "Person":
                    notification = DetectPerson(deviceId, homeId, userId!.Value);
                    break;
                case "OpeningOrClosing":
                    notification = DetectOpeningOrClosing(deviceId, homeId);
                    break;
                default:
                    throw new Exception("Invalid notification type");
            }

            SendNotification(homeId, notification);
        }

        private void SendNotification(int homeId, Notification notification)
        {
            var homeMembers = _homeMemberRepository.GetAll(x =>
                x.HomeId == homeId && x.CanReceiveNotifications
            );
            foreach (var member in homeMembers)
            {
                var homeMemberNotification = new HomeMemberNotification
                {
                    Notification = notification,
                    HomeId = homeId,
                    UserId = member.UserId,
                    Read = false,
                };
                _homeMemberNotificationRepository.Insert(homeMemberNotification);
            }
        }

        public IEnumerable<Notification> GetNotificationsByIds(int[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                throw new ArgumentException("IDs cannot be null or empty.");
            }

            var notifications = _notificationRepository.Get(x => ids.Contains(x.NotificationId));

            if (notifications == null)
            {
                throw new Exception("Notifications not found");
            }

            return (IEnumerable<Notification>)notifications;
        }


        private Notification DetectMotionSensorMotion(int motionSensorId, int homeDeviceId)
        {
            var device = _deviceRepository.Get(x =>
                x.DeviceId == motionSensorId && x.DeviceType == DeviceType.MotionSensor
            );
            if (device == null)
            {
                throw new Exception("Motion Sensor not found");
            }

            var homeDevice = _homeDeviceRepository.Get(x =>
                x.DeviceId == motionSensorId && x.HomeId == homeDeviceId
            );
            if (homeDevice == null)
            {
                throw new Exception("Device not found in home");
            }

            if (!homeDevice.IsConnected)
            {
                throw new Exception("Device is not connected");
            }

            var notification = new Notification
            {
                EventName = "Motion detected",
                HardwareId = homeDevice.HardwareId,
                CreatedAt = DateTime.Now.ToString("G", CultureInfo.CreateSpecificCulture("en-US")),
            };
            _notificationRepository.Insert(notification);

            return notification;
        }

        private Notification DetectSecurityCameraMotion(int securityCameraId, int homeDeviceId)
        {
            var device = _deviceRepository.Get(x =>
                x.DeviceId == securityCameraId && x.DeviceType == DeviceType.SecurityCamera
            );
            if (device == null)
            {
                throw new Exception("Security Camera not found");
            }
            SecurityCamera camera = (SecurityCamera)device;
            if (!camera.CanDetectMotion)
            {
                throw new Exception("Security Camera cannot detect motion");
            }

            var homeDevice = _homeDeviceRepository.Get(x =>
                x.DeviceId == securityCameraId && x.HomeId == homeDeviceId
            );
            if (homeDevice == null)
            {
                throw new Exception("Device not found in home");
            }

            if (!homeDevice.IsConnected)
            {
                throw new Exception("Device is not connected");
            }

            var notification = new Notification
            {
                EventName = "Motion detected",
                HardwareId = homeDevice.HardwareId,
                CreatedAt = DateTime.Now.ToString("G", CultureInfo.CreateSpecificCulture("en-US")),
            };
            _notificationRepository.Insert(notification);

            return notification;
        }

        private Notification DetectPerson(int securityCameraId, int homeDeviceId, int homeOwnerId)
        {
            var device = _deviceRepository.Get(x =>
                x.DeviceId == securityCameraId && x.DeviceType == DeviceType.SecurityCamera
            );
            if (device == null)
            {
                throw new Exception("Security Camera not found");
            }
            SecurityCamera camera = (SecurityCamera)device;

            if (camera.CanDetectPerson == false)
            {
                throw new Exception("Security Camera cannot detect person");
            }

            var homeDevice = _homeDeviceRepository.Get(x =>
                x.DeviceId == securityCameraId && x.HomeId == homeDeviceId
            );
            if (homeDevice == null)
            {
                throw new Exception("Device not found in home");
            }

            if (!homeDevice.IsConnected)
            {
                throw new Exception("Device is not connected");
            }

            var homeOwner = _userRepository.Get(x => x.UserId == homeOwnerId);

            var notification = new Notification
            {
                EventName = "Person detected",
                HardwareId = homeDevice.HardwareId,
                CreatedAt = DateTime.Now.ToString("G", CultureInfo.CreateSpecificCulture("en-US")),
            };

            if (homeOwner != null)
            {
                notification = new Notification
                {
                    EventName = $"Detected {homeOwner.FirstName}",
                    HardwareId = homeDevice.HardwareId,
                    CreatedAt = DateTime.Now.ToString("G", CultureInfo.CreateSpecificCulture("en-US")),
                };
            }
            _notificationRepository.Insert(notification);

            return notification;
        }

        private Notification DetectOpeningOrClosing(int windowSensorId, int homeDeviceId)
        {
            var windowSensor = _deviceRepository.Get(x =>
                x.DeviceId == windowSensorId && x.DeviceType == DeviceType.WindowSensor
            );
            if (windowSensor == null)
            {
                throw new Exception("Window Sensor not found");
            }

            var homeDevice = _homeDeviceRepository.Get(x =>
                x.DeviceId == windowSensor.DeviceId && x.HomeId == homeDeviceId
            );

            if (homeDevice == null)
            {
                throw new Exception("Device not found in home");
            }
            if (!homeDevice.IsConnected)
            {
                throw new Exception("Device is not connected");
            }
            var notification = new Notification
            {
                EventName = "Opening or closing detected",
                HardwareId = homeDevice.HardwareId,
                CreatedAt = DateTime.Now.ToString("G", CultureInfo.CreateSpecificCulture("en-US")),
            };
            _notificationRepository.Insert(notification);

            return notification;
        }
    }
}
