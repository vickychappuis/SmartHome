using smarthome.Dtos;

namespace smarthome.Domain.tests;

[TestClass]
public class HomeDeviceDtoTest
{
    [TestMethod]
    public void ShouldCreateHomeDevice()
    {
        var homeDevice = new HomeDeviceDto()
        {
            DeviceId = 1,
            HomeId = 1,
            RoomId = 1
        };
        
        // Assert
        Assert.AreEqual(1, homeDevice.DeviceId);
        Assert.AreEqual(1, homeDevice.HomeId);
        Assert.AreEqual(1, homeDevice.RoomId);
        
    }
    
}