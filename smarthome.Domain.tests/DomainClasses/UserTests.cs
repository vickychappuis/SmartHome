using smarthome.Domain;

namespace smarthome.Domain.tests;

[TestClass]
public class UserTests
{
    private DateTime _today = DateTime.Today;

    [TestMethod]
    public void Should_Create_User()
    {
        var user = new User
        {
            UserId = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "johnDoe@gmail.com",
            Password = "PassWord1!",
            Role = UserRoles.HomeOwner,
            RegisterDate = _today,
            ImageUrl = "https://placehold.co/128",
        };

        Assert.IsNotNull(user);
        Assert.AreEqual(1, user.UserId);
        Assert.AreEqual("John", user.FirstName);
        Assert.AreEqual("Doe", user.LastName);
        Assert.AreEqual("johnDoe@gmail.com", user.Email);
        Assert.AreEqual("PassWord1!", user.Password);
        Assert.AreEqual(UserRoles.HomeOwner, user.Role);
        Assert.AreEqual(_today, user.RegisterDate);
        Assert.AreEqual("https://placehold.co/128", user.ImageUrl);
    }
}
