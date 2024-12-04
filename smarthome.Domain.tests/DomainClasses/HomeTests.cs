using smarthome.Dtos;

namespace smarthome.Domain.tests;

[TestClass]
public class HomeTests
{
    [TestMethod]
    public void Should_Create_Home()
    {
        var home = new Home
        {
            HomeId = 1,
            Address = "1234 Home St.",
            HomeName = "Family home",
            Latitude = 14.2,
            Longitude = 32.4,
            MaxMembers = 8,
        };

        Assert.IsNotNull(home);
        Assert.AreEqual(1, home.HomeId);
        Assert.AreEqual("1234 Home St.", home.Address);
        Assert.AreEqual("Family home", home.HomeName);
        Assert.AreEqual(14.2, home.Latitude);
        Assert.AreEqual(32.4, home.Longitude);
        Assert.AreEqual(8, home.MaxMembers);
    }
}
