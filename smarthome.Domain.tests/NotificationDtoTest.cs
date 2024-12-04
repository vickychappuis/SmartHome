using smarthome.Dtos;

namespace smarthome.Domain.tests;

[TestClass]
public class NotificationDtoTest
{
    [TestMethod]
    public void ShouldCreateNotificationDto()
    {
        var notificationDto = new NotificationDto
        {
            NotificationId = 1,
            EventName = "TestEvent",
            CreatedAt = "2021-01-01",
            DeviceId = 1,
            Read = false
        };
        

        // Assert
        Assert.AreEqual(1, notificationDto.NotificationId);
        Assert.AreEqual("TestEvent", notificationDto.EventName);
        Assert.AreEqual("2021-01-01", notificationDto.CreatedAt);
        Assert.AreEqual(1, notificationDto.DeviceId);
        Assert.AreEqual(false, notificationDto.Read);
    }
}