using System.Linq.Expressions;
using Moq;
using Moq.Protected;
using smarthome.Domain;
using smarthome.Dtos;
using smarthome.IBusinessLogic;
using smarthome.IDataAccess;

namespace smarthome.BusinessLogic.tests


{
    public class DeviceServiceTests
    {
        [TestClass]
        public class AddDeviceTests
        {
            private IDeviceService _deviceService;
            private Mock<IGenericRepository<Device>> _deviceRepositoryMock;
            private Mock<IGenericRepository<Company>> _companyRepositoryMock;
            private Mock<IImportStrategy> _importJsonStrategyMock; 

            [TestInitialize]
            public void Setup()
            {
                _deviceRepositoryMock = new Mock<IGenericRepository<Device>>();
                _companyRepositoryMock = new Mock<IGenericRepository<Company>>();
                _importJsonStrategyMock = new Mock<IImportStrategy>();
                
                _deviceService = new DeviceService(
                    _deviceRepositoryMock.Object,
                    _companyRepositoryMock.Object
                );
            }

            [TestMethod]
            public void AddDevice_WithSecurityCamera_ShouldAddSecurityCamera()
            {
                var deviceDto = new DeviceDto()
                {
                    Name = "Test Camera",
                    Model = "Test Model",
                    Description = "Test Description",
                    ImageUrl = "Test Image",
                    IsInterior = true,
                    IsExterior = false,
                    CanDetectMotion = true,
                    CanDetectPerson = false,
                    CompanyId = 1,
                    RoomId = 1
                };

                var deviceType = DeviceType.SecurityCamera;

                _companyRepositoryMock
                    .Setup(repo => repo.Get(It.IsAny<Expression<Func<Company, bool>>>(), null))
                    .Returns(new Company());

                _deviceService.AddDevice(deviceDto, deviceType);

                _deviceRepositoryMock.Verify(
                    repo => repo.Insert(It.IsAny<SecurityCamera>()),
                    Times.Once
                );
            }

            [TestMethod]
            public void AddDevice_WithWindowSensor_ShouldAddWindowSensor()
            {
                var deviceDto = new DeviceDto();
                var deviceType = DeviceType.WindowSensor;

                _companyRepositoryMock
                    .Setup(repo => repo.Get(It.IsAny<Expression<Func<Company, bool>>>(), null))
                    .Returns(new Company());

                _deviceService.AddDevice(deviceDto, deviceType);

                _deviceRepositoryMock.Verify(
                    repo => repo.Insert(It.IsAny<WindowSensor>()),
                    Times.Once
                );
            }

            [TestMethod]
            public void AddDevice_WithSmartLamp_ShouldAddSmartLamp()
            {
                var deviceDto = new DeviceDto();
                var deviceType = DeviceType.SmartLamp;

                _companyRepositoryMock
                    .Setup(repo => repo.Get(It.IsAny<Expression<Func<Company, bool>>>(), null))
                    .Returns(new Company());


                _deviceService.AddDevice(deviceDto, deviceType);

                _deviceRepositoryMock.Verify(
                    repo => repo.Insert(It.IsAny<SmartLamp>()),
                    Times.Once
                );
            }
            
            [TestMethod]
            public void AddDevice_WithMotionSensor_ShouldAddMotionSensor()
            {
                var deviceDto = new DeviceDto();
                var deviceType = DeviceType.MotionSensor;

                _companyRepositoryMock
                    .Setup(repo => repo.Get(It.IsAny<Expression<Func<Company, bool>>>(), null))
                    .Returns(new Company());

                _deviceService.AddDevice(deviceDto, deviceType);

                _deviceRepositoryMock.Verify(
                    repo => repo.Insert(It.IsAny<MotionSensor>()),
                    Times.Once
                );
            }

            [TestMethod]
            public void AddDevice_WithInvalidDeviceType_ShouldThrowArgumentException()
            {
                var deviceDto = new DeviceDto();
                var deviceType = (DeviceType)99;

                Assert.ThrowsException<ArgumentException>(() => { _deviceService.AddDevice(deviceDto, deviceType); });
            }
            
            [TestMethod]
            public void AddDevice_WithInvalidCompany_ShouldThrowArgumentException()
            {
                var deviceDto = new DeviceDto();
                var deviceType = DeviceType.SecurityCamera;

                _companyRepositoryMock
                    .Setup(repo => repo.Get(It.IsAny<Expression<Func<Company, bool>>>(), null))
                    .Returns((Company)null);

                Assert.ThrowsException<ArgumentException>(() => { _deviceService.AddDevice(deviceDto, deviceType); });
            }
            
            [TestMethod]
            public void AddDevice_WithInvalidValidator_ShouldThrowArgumentException()
            {
                var deviceDto = new DeviceDto();
                var deviceType = DeviceType.SecurityCamera;

                _companyRepositoryMock
                    .Setup(repo => repo.Get(It.IsAny<Expression<Func<Company, bool>>>(), null))
                    .Returns(new Company { DeviceValidator = "InvalidValidator" });

                Assert.ThrowsException<ArgumentException>(() => { _deviceService.AddDevice(deviceDto, deviceType); });
            }

            [TestMethod]
            public void AddDevice_DeviceDoesNotPassValidator_ShouldThrowException()
            {
                var deviceDto = new DeviceDto()
                {
                    CanDetectMotion = false,
                    CanDetectPerson = false,
                    IsExterior = false,
                    IsInterior = false,
                };
                var deviceType = DeviceType.SecurityCamera;

                _companyRepositoryMock
                    .Setup(repo => repo.Get(It.IsAny<Expression<Func<Company, bool>>>(), null))
                    .Returns(new Company { DeviceValidator = "InvalidValidator" });

                Assert.ThrowsException<DirectoryNotFoundException>(() => { _deviceService.AddDevice(deviceDto, deviceType); });
            }

            [TestMethod]
            public void GetDevices_WithNoDevices_ShouldReturnEmpty()
            {
                _deviceRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<Device>());

                var Devices = _deviceService.GetDevices();

                Assert.IsFalse(Devices.Any());
            }

            [DataRow(1)]
            [DataRow(9)]
            [DataRow(16)]
            [DataRow(20)]
            [TestMethod]
            public void GetDevices_WithLte20Devices_ShouldReturnAllDevices(int count)
            {
                _deviceRepositoryMock
                    .Setup(repo => repo.GetAll(It.IsAny<Func<Device, bool>>(), null))
                    .Returns(Enumerable.Repeat(new SecurityCamera(), count));

                var Devices = _deviceService.GetDevices();

                Assert.AreEqual(count, Devices.Count());
            }

            [DataRow(21)]
            [DataRow(24)]
            [DataRow(32)]
            [DataRow(68)]
            [DataRow(126)]
            [TestMethod]
            public void GetDevices_WithGt20Devices_ShouldReturn20Devices(int count)
            {
                _deviceRepositoryMock
                    .Setup(repo => repo.GetAll(It.IsAny<Func<Device, bool>>(), null))
                    .Returns(Enumerable.Repeat(new SecurityCamera(), count));

                var Devices = _deviceService.GetDevices();

                Assert.AreEqual(20, Devices.Count());
            }

            [DataRow(1)]
            [DataRow(5)]
            [DataRow(10)]
            [DataRow(20)]
            [TestMethod]
            public void GetDevices_WithSkip_ShouldStartAtSkip(int skip)
            {
                var mockDevices = Enumerable
                    .Range(1, 40)
                    .Select(i => new SecurityCamera { DeviceId = i, Name = $"Device {i}" })
                    .ToList();

                _deviceRepositoryMock
                    .Setup(repo => repo.GetAll(It.IsAny<Func<Device, bool>>(), null))
                    .Returns(mockDevices);

                var Devices = _deviceService.GetDevices(skip: skip);

                Assert.AreEqual(20, Devices.Count());
                Assert.AreEqual(skip + 1, Devices.First().DeviceId);
            }

            [DataRow(1)]
            [DataRow(5)]
            [DataRow(10)]
            [DataRow(20)]
            [TestMethod]
            public void GetDevices_WithTake_ShouldReturnTake(int take)
            {
                var mockDevices = Enumerable
                    .Range(1, 40)
                    .Select(i => new SecurityCamera { DeviceId = i, Name = $"Device {i}" })
                    .ToList();

                _deviceRepositoryMock
                    .Setup(repo => repo.GetAll(It.IsAny<Func<Device, bool>>(), null))
                    .Returns(mockDevices);

                var Devices = _deviceService.GetDevices(take: take);

                Assert.AreEqual(take, Devices.Count());
            }

            [TestMethod]
            public void ListDeviceTypes_ShouldReturnAllDeviceTypes()
            {
                var deviceTypes = _deviceService.ListDeviceTypes();

                Assert.AreEqual(Enum.GetValues(typeof(DeviceType)).Length, deviceTypes.Count);
            }
            
            [TestMethod]
            public void GetDevice_WithValidId_ShouldReturnDevice()
            {
                var deviceId = 1;
                var device = new SecurityCamera { DeviceId = deviceId };

                _deviceRepositoryMock
                    .Setup(repo => repo.Get(It.IsAny<Expression<Func<Device, bool>>>(), null))
                    .Returns(device);

                var result = _deviceService.GetDevice(deviceId);

                Assert.AreEqual(device, result);
            }
            
            [TestMethod]
            public void GetDevice_WithInvalidId_ShouldReturnNull()
            {
                var deviceId = 1;

                _deviceRepositoryMock
                    .Setup(repo => repo.Get(It.IsAny<Expression<Func<Device, bool>>>(), null))
                    .Returns((Device)null);

                var result = _deviceService.GetDevice(deviceId);

                Assert.IsNull(result);
            }
            
            [TestMethod]
            public void ListImportStrategies_ShouldReturnAllImportStrategies()
            {
                var strategies = _deviceService.ListImportStrategies("../../../");
    
                Assert.IsNotNull(strategies);
            }
            
            [TestMethod]
            public void ListValidators_ShouldReturnAllValidators()
            {
                var validators = _deviceService.ListValidators("../../../");
            
                Assert.IsNotNull(validators);
            }
            
            
            
        }
        
        
    }
    
    
}
