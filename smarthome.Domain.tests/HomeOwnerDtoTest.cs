using smarthome.Dtos;

namespace smarthome.Domain.tests;

[TestClass]
public class HomeOwnerDtoTest
{
    [TestMethod]
    public void ShouldCreateHomeOwnerDto()
    {
        var homeOwnerDto = new HomeOwnerDto()
        {
            FirstName = "john",
            Email = "a@gmail.com",
            CanAddDevice = true,
            CanListDevices = true,
            CanReceiveNotifications = true,
            ImageUrl = "https://www.google.com"
        };
        
        // Assert
        Assert.AreEqual("john", homeOwnerDto.FirstName);
        Assert.AreEqual("a@gmail.com", homeOwnerDto.Email);
        Assert.AreEqual(true, homeOwnerDto.CanAddDevice);
        Assert.AreEqual(true, homeOwnerDto.CanListDevices);
        Assert.AreEqual(true, homeOwnerDto.CanReceiveNotifications);
        Assert.AreEqual("https://www.google.com", homeOwnerDto.ImageUrl);
    }
}