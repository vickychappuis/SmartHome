namespace smarthome.Domain.tests;

using smarthome.Domain;

using Moq;

[TestClass]
public class HomeMemberTests
{
    [TestMethod]
    public void Should_Create_Home_Member()
    {
        var homeMock = new Mock<Home>();
        var userMock = new Mock<User>();
        
        var homeMember = new HomeMember
        {
            HomeId = 1,
            Home = homeMock.Object,
            UserId = 1,
            User = userMock.Object,
            CanAddDevice = true,
            CanListDevices = true,
            CanReceiveNotifications = true
        };
        
        Assert.IsNotNull(homeMember);
        Assert.IsNotNull(homeMember.Home);
        Assert.IsNotNull(homeMember.User);
        Assert.AreEqual(1, homeMember.HomeId);
        Assert.AreEqual(1, homeMember.UserId);
        Assert.IsTrue(homeMember.CanAddDevice);
        Assert.IsTrue(homeMember.CanListDevices);
        Assert.IsTrue(homeMember.CanReceiveNotifications);
    }
}