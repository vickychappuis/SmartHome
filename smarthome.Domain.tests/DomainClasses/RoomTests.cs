using smarthome.Domain;
namespace smarthome.Domain.tests;

[TestClass]
public class RoomTests
{
    [TestMethod]
    public void ShouldCreateRoom()
    {
        var room = new Room()
        {
            RoomId = 1,
            RoomName = "Living Room",
            HomeId = 1,
            Home = new Home()
        };
        
        // Assert
        Assert.AreEqual(1, room.RoomId); 
        Assert.AreEqual("Living Room", room.RoomName);
        Assert.AreEqual(1, room.HomeId);
        Assert.IsNotNull(room.Home);
    }
    
}