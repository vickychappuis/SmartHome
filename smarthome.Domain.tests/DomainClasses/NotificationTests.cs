using smarthome.Domain.tests;

namespace smarthome.Domain.tests;

[TestClass]
public class NotificationTests
{
    [TestMethod]
    public void Should_Create_Notification()
    {
        var notification = new Notification
        {
            NotificationId = 1,
            EventName = "Movement Detected",
            HardwareId = 1,
            CreatedAt = "Camera 600",
        };

        Assert.IsNotNull(notification);
        Assert.AreEqual(1, notification.NotificationId);
        Assert.AreEqual("Movement Detected", notification.EventName);
        Assert.AreEqual(1, notification.HardwareId);
        Assert.AreEqual("Camera 600", notification.CreatedAt);
    }
}
