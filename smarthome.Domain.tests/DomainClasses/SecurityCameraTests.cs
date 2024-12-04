using smarthome.Domain;
using smarthome.Dtos;

namespace smarthome.Domain.tests;

[TestClass]
public class SecurityCameraTests
{
    [TestMethod]
    public void Should_Create_Security_Camera()
    {
        var securityCamera = new SecurityCamera
        {
            DeviceId = 1,
            Name = "Security Camera",
            Model = "600",
            Description = "The best Security Camera in the market",
            ImageUrl = "https://device1.com/logo.png",
            IsExterior = false,
            IsInterior = true,
            CanDetectPerson = false,
            CanDetectMotion = false,
        };
        Assert.IsNotNull(securityCamera);
        Assert.AreEqual(1, securityCamera.DeviceId);
        Assert.AreEqual("Security Camera", securityCamera.Name);
        Assert.AreEqual("600", securityCamera.Model);
        Assert.AreEqual("The best Security Camera in the market", securityCamera.Description);
        Assert.AreEqual("https://device1.com/logo.png", securityCamera.ImageUrl);
        Assert.IsFalse(securityCamera.IsExterior);
        Assert.IsTrue(securityCamera.IsInterior);
        Assert.IsFalse(securityCamera.CanDetectPerson);
        Assert.IsFalse(securityCamera.CanDetectMotion);
    }

    [TestMethod]
    public void ShouldThrowException_WhenDtoWithoutCanDetectMotion()
    {
        var dto = new DeviceDto
        {
            Name = "Security Camera",
            Model = "600",
            Description = "The best Security Camera in the market",
            ImageUrl = "https://device1.com/logo.png",
            IsExterior = false,
            IsInterior = true,
            CanDetectPerson = false,
            CanDetectMotion = null,
        };

        Assert.ThrowsException<ArgumentException>(() => new SecurityCamera(dto));
    }

    [TestMethod]
    public void ShouldThrowException_WhenDtoWithoutCanDetectPerson()
    {
        var dto = new DeviceDto
        {
            Name = "Security Camera",
            Model = "600",
            Description = "The best Security Camera in the market",
            ImageUrl = "https://device1.com/logo.png",
            IsExterior = false,
            IsInterior = true,
            CanDetectPerson = null,
            CanDetectMotion = false,
        };

        Assert.ThrowsException<ArgumentException>(() => new SecurityCamera(dto));
    }

    [TestMethod]
    public void ShouldThrowException_WhenDtoWithoutIsInterior()
    {
        var dto = new DeviceDto
        {
            Name = "Security Camera",
            Model = "600",
            Description = "The best Security Camera in the market",
            ImageUrl = "https://device1.com/logo.png",
            IsExterior = false,
            IsInterior = null,
            CanDetectPerson = false,
            CanDetectMotion = false,
        };

        Assert.ThrowsException<ArgumentException>(() => new SecurityCamera(dto));
    }

    [TestMethod]
    public void ShouldThrowException_WhenDtoWithoutIsExterior()
    {
        var dto = new DeviceDto
        {
            Name = "Security Camera",
            Model = "600",
            Description = "The best Security Camera in the market",
            ImageUrl = "https://device1.com/logo.png",
            IsExterior = null,
            IsInterior = true,
            CanDetectPerson = false,
            CanDetectMotion = false,
        };

        Assert.ThrowsException<ArgumentException>(() => new SecurityCamera(dto));
    }
}
