using smarthome.Domain;

namespace smarthome.Domain.testsDomain;

[TestClass]
public class WindowSensorTests
{
    [TestMethod]
    public void Should_Create_Device()
    {
        var device = new WindowSensor()
        {
            DeviceId = 1,
            Name = "Window Sensor",
            Model = "600",
            Description = "The best Window Sensor in the market",
            ImageUrl = "https://device1.com/logo.png",
        };

        Assert.IsNotNull(device);
        Assert.AreEqual(1, device.DeviceId);
        Assert.AreEqual("Window Sensor", device.Name);
        Assert.AreEqual("600", device.Model);
        Assert.AreEqual("The best Window Sensor in the market", device.Description);
        Assert.AreEqual("https://device1.com/logo.png", device.ImageUrl);
    }
}