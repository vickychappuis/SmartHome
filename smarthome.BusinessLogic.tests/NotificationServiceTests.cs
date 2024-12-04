using System.Linq.Expressions;
using Moq;
using smarthome.Domain;
using smarthome.IBusinessLogic;
using smarthome.IDataAccess;

namespace smarthome.BusinessLogic.tests
{
    [TestClass]
    public class NotificationServiceTests
    {
        private INotificationService _notificationService;
        private Mock<IGenericRepository<Notification>> _notificationRepositoryMock;
        private Mock<IGenericRepository<Device>> _deviceRepositoryMock;
        private Mock<IGenericRepository<HomeDevice>> _homeDeviceRepositoryMock;
        private Mock<IGenericRepository<User>> _userRepositoryMock;
        private Mock<IGenericRepository<HomeMember>> _homeMemberRepository;
        private Mock<IGenericRepository<HomeMemberNotification>> _homeMemberNotificationRepository;

        [TestInitialize]
        public void Setup()
        {
            _notificationRepositoryMock = new Mock<IGenericRepository<Notification>>();
            _deviceRepositoryMock = new Mock<IGenericRepository<Device>>();
            _homeDeviceRepositoryMock = new Mock<IGenericRepository<HomeDevice>>();
            _userRepositoryMock = new Mock<IGenericRepository<User>>();
            _homeMemberRepository = new Mock<IGenericRepository<HomeMember>>();
            _homeMemberNotificationRepository =
                new Mock<IGenericRepository<HomeMemberNotification>>();

            _notificationService = new NotificationService(
                _notificationRepositoryMock.Object,
                _deviceRepositoryMock.Object,
                _homeDeviceRepositoryMock.Object,
                _userRepositoryMock.Object,
                _homeMemberRepository.Object,
                _homeMemberNotificationRepository.Object
            );
        }

        [TestMethod]
        public void DetectMotion_WhenSecurityCameraNotFound_ShouldReturnError()
        {
            var deviceId = 1;
            var homeId = 2;

            _deviceRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Expression<Func<Device, bool>>>(), null))
                .Returns((Device)null);

            var ex = Assert.ThrowsException<Exception>(
                () => _notificationService.CreateNotification(deviceId, homeId, "SecurityCameraMotion", null)
            );
            Assert.AreEqual("Security Camera not found", ex.Message);
        }

        [TestMethod]
        public void DetectMotion_WhenSecurityCameraCannotDetectMotion_ShouldReturnError()
        {
            var deviceId = 1;
            var homeId = 2;
            var camera = new SecurityCamera { CanDetectMotion = false };

            _deviceRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Expression<Func<Device, bool>>>(), null))
                .Returns(camera);

            var ex = Assert.ThrowsException<Exception>(
                () => _notificationService.CreateNotification(deviceId, homeId, "SecurityCameraMotion", null)
            );
            Assert.AreEqual("Security Camera cannot detect motion", ex.Message);
        }

        [TestMethod]
        public void DetectMotion_WhenDeviceNotFoundInHome_ShouldReturnError()
        {
            var deviceId = 1;
            var homeId = 2;
            var camera = new SecurityCamera { CanDetectMotion = true };

            _deviceRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Expression<Func<Device, bool>>>(), null))
                .Returns(camera);
            _homeDeviceRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Expression<Func<HomeDevice, bool>>>(), null))
                .Returns((HomeDevice)null);

            var ex = Assert.ThrowsException<Exception>(
                () => _notificationService.CreateNotification(deviceId, homeId, "SecurityCameraMotion", null)
            );
            Assert.AreEqual("Device not found in home", ex.Message);
        }

        [TestMethod]
        public void DetectMotion_WhenDeviceNotConnected_ShouldReturnError()
        {
            var deviceId = 1;
            var homeId = 2;
            var camera = new SecurityCamera { CanDetectMotion = true };
            var homeDevice = new HomeDevice { IsConnected = false };

            _deviceRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Expression<Func<Device, bool>>>(), null))
                .Returns(camera);
            _homeDeviceRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Expression<Func<HomeDevice, bool>>>(), null))
                .Returns(homeDevice);

            var ex = Assert.ThrowsException<Exception>(
                () => _notificationService.CreateNotification(deviceId, homeId, "SecurityCameraMotion", null)
            );
            Assert.AreEqual("Device is not connected", ex.Message);
        }

        [TestMethod]
        public void DetectMotion_ShouldAddNotification()
        {
            var deviceId = 1;
            var homeId = 2;
            var camera = new SecurityCamera { CanDetectMotion = true };
            var homeDevice = new HomeDevice { IsConnected = true, HardwareId = 5 };

            _deviceRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Expression<Func<Device, bool>>>(), null))
                .Returns(camera);
            _homeDeviceRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Expression<Func<HomeDevice, bool>>>(), null))
                .Returns(homeDevice);

            _notificationService.CreateNotification(deviceId, homeId, "SecurityCameraMotion", null);

            _notificationRepositoryMock.Verify(
                repo => repo.Insert(It.IsAny<Notification>()),
                Times.Once
            );
        }

        [TestMethod]
        public void DetectPerson_WhenSecurityCameraNotFound_ShouldReturnError()
        {
            var deviceId = 1;
            var homeId = 2;

            var userId = 1;

            _deviceRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Expression<Func<Device, bool>>>(), null))
                .Returns((Device)null);

            var ex = Assert.ThrowsException<Exception>(
                () => _notificationService.CreateNotification(deviceId, homeId, "Person", userId)
            );
            Assert.AreEqual("Security Camera not found", ex.Message);
        }

        [TestMethod]
        public void DetectPerson_WhenCameraCannotDetectPerson_ShouldReturnError()
        {
            var deviceId = 1;
            var homeId = 2;
            var camera = new SecurityCamera { CanDetectPerson = false };

            var userId = 1;

            _deviceRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Expression<Func<Device, bool>>>(), null))
                .Returns(camera);

            var ex = Assert.ThrowsException<Exception>(
                () => _notificationService.CreateNotification(deviceId, homeId, "Person", userId)
            );
            Assert.AreEqual("Security Camera cannot detect person", ex.Message);
        }

        [TestMethod]
        public void DetectPerson_WhenDeviceNotFoundInHome_ShouldReturnError()
        {
            var deviceId = 1;
            var homeId = 2;
            var camera = new SecurityCamera { CanDetectPerson = true };

            var userId = 1;

            _deviceRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Expression<Func<Device, bool>>>(), null))
                .Returns(camera);
            _homeDeviceRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Expression<Func<HomeDevice, bool>>>(), null))
                .Returns((HomeDevice)null);

            var ex = Assert.ThrowsException<Exception>(
                () => _notificationService.CreateNotification(deviceId, homeId, "Person", userId)
            );
            Assert.AreEqual("Device not found in home", ex.Message);
        }

        [TestMethod]
        public void DetectPerson_WhenDeviceNotConnected_ShouldReturnError()
        {
            var deviceId = 1;
            var homeId = 2;
            var camera = new SecurityCamera { CanDetectPerson = true };
            var homeDevice = new HomeDevice { IsConnected = false };

            var userId = 1;

            _deviceRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Expression<Func<Device, bool>>>(), null))
                .Returns(camera);
            _homeDeviceRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Expression<Func<HomeDevice, bool>>>(), null))
                .Returns(homeDevice);

            var ex = Assert.ThrowsException<Exception>(
                () => _notificationService.CreateNotification(deviceId, homeId, "Person", userId)
            );
            Assert.AreEqual("Device is not connected", ex.Message);
        }

        [TestMethod]
        public void DetectPerson_WhenPersonDetected_ShouldAddNotification()
        {
            var deviceId = 1;
            var homeId = 2;
            var camera = new SecurityCamera { CanDetectPerson = true };
            var homeDevice = new HomeDevice { IsConnected = true, HardwareId = 5 };

            var userId = 1;

            _deviceRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Expression<Func<Device, bool>>>(), null))
                .Returns(camera);
            _homeDeviceRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Expression<Func<HomeDevice, bool>>>(), null))
                .Returns(homeDevice);

            _notificationService.CreateNotification(deviceId, homeId, "Person", userId);

            _notificationRepositoryMock.Verify(
                repo => repo.Insert(It.IsAny<Notification>()),
                Times.Once
            );
        }

        [TestMethod]
        public void DetectOpeningOrClosing_WhenWindowSensorNotFound_ShouldReturnError()
        {
            var deviceId = 1;
            var homeId = 2;

            _deviceRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Expression<Func<Device, bool>>>(), null))
                .Returns((Device)null);

            var ex = Assert.ThrowsException<Exception>(
                () => _notificationService.CreateNotification(deviceId, homeId, "OpeningOrClosing", null)
            );
            Assert.AreEqual("Window Sensor not found", ex.Message);
        }

        [TestMethod]
        public void DetectOpeningOrClosing_WhenWindowSensorNotFoundInHome_ShouldReturnError()
        {
            var deviceId = 1;
            var homeId = 2;
            var windowSensor = new WindowSensor
            {
                DeviceId = deviceId,
                DeviceType = DeviceType.WindowSensor,
            };

            _deviceRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Expression<Func<Device, bool>>>(), null))
                .Returns(windowSensor);
            _homeDeviceRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Expression<Func<HomeDevice, bool>>>(), null))
                .Returns((HomeDevice)null);

            var ex = Assert.ThrowsException<Exception>(
                () => _notificationService.CreateNotification(deviceId, homeId, "OpeningOrClosing", null)
            );
            Assert.AreEqual("Device not found in home", ex.Message);
        }

        [TestMethod]
        public void DetectOpeningOrClosing_WhenWindowSensorNotConnected_ShouldReturnError()
        {
            var deviceId = 1;
            var homeId = 2;
            var windowSensor = new WindowSensor
            {
                DeviceId = deviceId,
                DeviceType = DeviceType.WindowSensor,
            };
            var homeDevice = new HomeDevice { IsConnected = false };

            _deviceRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Expression<Func<Device, bool>>>(), null))
                .Returns(windowSensor);
            _homeDeviceRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Expression<Func<HomeDevice, bool>>>(), null))
                .Returns(homeDevice);

            var ex = Assert.ThrowsException<Exception>(
                () => _notificationService.CreateNotification(deviceId, homeId, "OpeningOrClosing", null)
            );
            Assert.AreEqual("Device is not connected", ex.Message);
        }

        [TestMethod]
        public void DetectOpeningOrClosing_WhenOpeningClosingDetected_ShouldAddNotification()
        {
            var deviceId = 1;
            var homeId = 2;
            var windowSensor = new WindowSensor
            {
                DeviceId = deviceId,
                DeviceType = DeviceType.WindowSensor,
            };
            var homeDevice = new HomeDevice { IsConnected = true, HardwareId = 5 };

            _deviceRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Expression<Func<Device, bool>>>(), null))
                .Returns(windowSensor);
            _homeDeviceRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Expression<Func<HomeDevice, bool>>>(), null))
                .Returns(homeDevice);

            _notificationService.CreateNotification(deviceId, homeId, "OpeningOrClosing", null);

            _notificationRepositoryMock.Verify(
                repo => repo.Insert(It.IsAny<Notification>()),
                Times.Once
            );
        }

        [TestMethod]
        public void DetectMotionSensorMotion_WhenMotionSensorNotFound_ShouldReturnError()
        {
            var deviceId = 1;
            var homeId = 2;

            _deviceRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Expression<Func<Device, bool>>>(), null))
                .Returns((Device)null);

            var ex = Assert.ThrowsException<Exception>(
                () => _notificationService.CreateNotification(deviceId, homeId, "MotionSensorMotion", null)
            );
            Assert.AreEqual("Motion Sensor not found", ex.Message);
        }

        [TestMethod]
        public void DetectMotionSensorMotion_WhenMotionSensorNotFoundInHome_ShouldReturnError()
        {
            var deviceId = 1;
            var homeId = 2;
            var motionSensor = new MotionSensor
            {
                DeviceId = deviceId,
                DeviceType = DeviceType.MotionSensor,
            };

            _deviceRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Expression<Func<Device, bool>>>(), null))
                .Returns(motionSensor);
            _homeDeviceRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Expression<Func<HomeDevice, bool>>>(), null))
                .Returns((HomeDevice)null);

            var ex = Assert.ThrowsException<Exception>(
                () => _notificationService.CreateNotification(deviceId, homeId, "MotionSensorMotion", null)
            );
            Assert.AreEqual("Device not found in home", ex.Message);
        }

        [TestMethod]
        public void DetectMotionSensorMotion_WhenMotionSensorNotConnected_ShouldReturnError()
        {
            var deviceId = 1;
            var homeId = 2;
            var motionSensor = new MotionSensor
            {
                DeviceId = deviceId,
                DeviceType = DeviceType.MotionSensor,
            };
            var homeDevice = new HomeDevice { IsConnected = false };

            _deviceRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Expression<Func<Device, bool>>>(), null))
                .Returns(motionSensor);
            _homeDeviceRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Expression<Func<HomeDevice, bool>>>(), null))
                .Returns(homeDevice);

            var ex = Assert.ThrowsException<Exception>(
                () => _notificationService.CreateNotification(deviceId, homeId, "MotionSensorMotion", null)
            );
            Assert.AreEqual("Device is not connected", ex.Message);
        }

        [TestMethod]
        public void DetectMotionSensorMotion_WhenMotionDetected_ShouldAddNotification()
        {
            var deviceId = 1;
            var homeId = 2;
            var motionSensor = new MotionSensor
            {
                DeviceId = deviceId,
                DeviceType = DeviceType.MotionSensor,
            };
            var homeDevice = new HomeDevice { IsConnected = true, HardwareId = 5 };

            _deviceRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Expression<Func<Device, bool>>>(), null))
                .Returns(motionSensor);
            _homeDeviceRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Expression<Func<HomeDevice, bool>>>(), null))
                .Returns(homeDevice);

            _notificationService.CreateNotification(deviceId, homeId, "MotionSensorMotion", null);

            _notificationRepositoryMock.Verify(
                repo => repo.Insert(It.IsAny<Notification>()),
                Times.Once
            );
        }
        
        [TestMethod]
        public void GetNotificationsByIds_WhenIdsAreNull_ShouldThrowException()
        {
            var ids = (int[])null;

            var ex = Assert.ThrowsException<ArgumentException>(
                () => _notificationService.GetNotificationsByIds(ids)
            );
            Assert.AreEqual("IDs cannot be null or empty.", ex.Message);
        }
        
        [TestMethod]
        public void GetNotificationsByIds_WhenIdsAreEmpty_ShouldThrowException()
        {
            var ids = new int[0];

            var ex = Assert.ThrowsException<ArgumentException>(
                () => _notificationService.GetNotificationsByIds(ids)
            );
            Assert.AreEqual("IDs cannot be null or empty.", ex.Message);
        }
        
       

    }

}
