using smarthome.Dtos;
namespace smarthome.Domain.tests;

[TestClass]
public class UserDtoTest
{
    [TestMethod]
    public void ShouldCreateUserDto()
    {
        var user = new UserDto()
        {
            role = "role",
            email = "a@gmail.com",
            password = "password",
            firstName = "john",
            lastName = "doe",
            imageUrl = "https://www.google.com"
        };
        
        // Assert
        Assert.AreEqual("role", user.role);
        Assert.AreEqual("a@gmail.com", user.email);
        Assert.AreEqual("password", user.password);
        Assert.AreEqual("john", user.firstName);
        Assert.AreEqual("doe", user.lastName);
        Assert.AreEqual("https://www.google.com", user.imageUrl);
    }
}