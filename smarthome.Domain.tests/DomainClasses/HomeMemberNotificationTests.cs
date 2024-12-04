using smarthome.Domain;

namespace smarthome.Domain.tests;

[TestClass]
public class HomeMemberNotificationTests
{
    [TestMethod]
    public void ShouldCreateHomeMemberNotification()
    {
        var homeMemberNotification = new HomeMemberNotification()
        {
            HomeId = 1,
            HomeMember = new HomeMember(),
            UserId = 1,
            NotificationId = 1,
            Read = false,
            Notification = new Notification()
        };
        
        // Assert
        Assert.AreEqual(1, homeMemberNotification.HomeId); 
        Assert.IsNotNull(homeMemberNotification.HomeMember);
        Assert.AreEqual(1, homeMemberNotification.UserId);
        Assert.AreEqual(1, homeMemberNotification.NotificationId);
        Assert.IsFalse(homeMemberNotification.Read);
        Assert.IsNotNull(homeMemberNotification.Notification);
    }
}