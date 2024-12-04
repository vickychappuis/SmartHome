using smarthome.Domain;
using Moq;

namespace smarthome.Domain.tests;

[TestClass]
public class SmartLampTests
{
    [TestMethod]
    public void Should_Create_SmartLamp()
    {
        var smartLamp = new SmartLamp
        {
            DeviceId = 1,
            Name = "Smart Lamp",
            Model = "600",
            Description = "The best Smart Lamp in the market",
            ImageUrl = "https://device1.com/logo.png",
            IsTurnedOn = false,
        };
        Assert.IsNotNull(smartLamp);
        Assert.AreEqual(1, smartLamp.DeviceId);
        Assert.AreEqual("Smart Lamp", smartLamp.Name);
        Assert.AreEqual("600", smartLamp.Model);
        Assert.AreEqual("The best Smart Lamp in the market", smartLamp.Description);
        Assert.AreEqual("https://device1.com/logo.png", smartLamp.ImageUrl);
        Assert.IsFalse(smartLamp.IsTurnedOn);
    }

    [TestMethod]
    public void ChangeLampState_ShouldChangeLampState()
    {
        var smartLamp = new SmartLamp
        {
            DeviceId = 1,
            Name = "Smart Lamp",
            Model = "600",
            Description = "The best Smart Lamp in the market",
            ImageUrl = "https://device1.com/logo.png",
            IsTurnedOn = false,
        };
        smartLamp.ChangeLampState(true);
        Assert.IsTrue(smartLamp.IsTurnedOn);
    }

}