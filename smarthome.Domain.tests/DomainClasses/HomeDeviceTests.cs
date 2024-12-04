using smarthome.Domain;
using Moq;

namespace smarthome.Domain.tests;

[TestClass]
public class HomeDeviceTests
{
    [TestMethod]
    public void Should_Create_Home_Device()
    {
        var homeMock = new Mock<Home>();
        var deviceMock = new Mock<Device>();
        
        var homeDevice = new HomeDevice
        {
            HomeId = 1,
            Home = homeMock.Object,
            DeviceId = 1,
            Device = deviceMock.Object,
            HardwareId = 1,
            IsConnected = true,
            DeviceName = "Test Device",
            RoomId = 1,
            Room = new Room(),
            IsOpen = false
        };
        Assert.IsNotNull(homeDevice);
        Assert.IsNotNull(homeDevice.Home);
        Assert.IsNotNull(homeDevice.Device);
        Assert.AreEqual(1, homeDevice.HomeId);
        Assert.AreEqual(1, homeDevice.DeviceId);
        Assert.AreEqual(1, homeDevice.HardwareId);
        Assert.AreEqual("Test Device", homeDevice.DeviceName);
        Assert.IsTrue(homeDevice.IsConnected);
        Assert.IsNotNull(homeDevice.Room);
        Assert.AreEqual(1, homeDevice.RoomId);
        Assert.IsFalse(homeDevice.IsOpen);
    }
}
