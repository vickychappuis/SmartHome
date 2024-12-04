using smarthome.Domain;

namespace smarthome.Domain.testsDomain;

[TestClass]
public class MotionSensorTests
{
    [TestMethod]
    public void Should_Create_Device()
    {
        var device = new MotionSensor()
        {
            DeviceId = 1,
            Name = "Motion Sensor",
            Model = "500",
            Description = "The best Motion Sensor in the market",
            ImageUrl = "https://device1.com/logo.png"
        };

        Assert.IsNotNull(device);
        Assert.AreEqual(1, device.DeviceId);
        Assert.AreEqual("Motion Sensor", device.Name);
        Assert.AreEqual("500", device.Model);
        Assert.AreEqual("The best Motion Sensor in the market", device.Description);
        Assert.AreEqual("https://device1.com/logo.png", device.ImageUrl);
    }
}