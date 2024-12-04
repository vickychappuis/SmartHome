using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using smarthome.Domain;
using smarthome.Dtos;
using smarthome.IBusinessLogic;
using smarthome.WebApi.Controllers;

namespace smarthome.WebApi.tests;

[TestClass]
public class UsersControllerTests
{
    private Mock<IUserService> _userServiceMock;
    private DefaultHttpContext _httpContext;
    private UsersController _usersController;

    [TestInitialize]
    public void Setup()
    {
        _userServiceMock = new Mock<IUserService>();
        _usersController = new UsersController(_userServiceMock.Object);
        _httpContext = new DefaultHttpContext();
        _usersController.ControllerContext = new ControllerContext() { HttpContext = _httpContext };
    }

    [TestMethod]
    public void GetUsers_InvalidData_ShouldReturnBadRequest()
    {
        _userServiceMock
            .Setup(service => service.GetUsers(null, null, null, null))
            .Throws(new ArgumentException("Missing data in User constructor"));
        var result = _usersController.Get(null, null, null, null);
        Assert.IsInstanceOfType(result, typeof(BadRequestResult));
    }

    [TestMethod]
    public void GetUsers_ValidData_ShouldReturnOk()
    {
        _userServiceMock
            .Setup(service => service.GetUsers(null, null, null, null))
            .Returns(new List<User>());
        var result = _usersController.Get(null, null, null, null);
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void GetFromId_InvalidData_ShouldReturnBadRequest()
    {
        var userId = 1;
        _userServiceMock
            .Setup(service => service.GetById(It.IsAny<int>()))
            .Throws(new ArgumentException("User not found"));
        var result = _usersController.Get(userId);
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public void GetFromId_ValidData_ShouldReturnOk()
    {
        var userId = 1;
        _userServiceMock.Setup(service => service.GetById(It.IsAny<int>())).Returns(new User());
        var result = _usersController.Get(userId);
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void PostUser_InvalidData_ShouldReturnBadRequest()
    {
        _userServiceMock
            .Setup(service => service.CreateUser(It.IsAny<UserDto>()))
            .Throws(new ArgumentException("Missing data in User constructor"));
        var result = _usersController.Post(new UserDto());
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public void PostUser_ValidData_ShouldReturnOk()
    {
        _userServiceMock.Setup(service => service.CreateUser(It.IsAny<UserDto>()));
        var result = _usersController.Post(new UserDto());
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }

    [TestMethod]
    public void DeleteUsers_InvalidData_ShouldReturnBadRequest()
    {
        var userId = 1;
        _userServiceMock
            .Setup(service => service.DeleteUser(It.IsAny<int>()))
            .Throws(new ArgumentException("User not found"));
        var result = _usersController.Delete(userId);
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public void DeleteUsers_ValidData_ShouldReturnNoContent()
    {
        var userId = 1;
        _userServiceMock.Setup(service => service.DeleteUser(It.IsAny<int>()));
        var result = _usersController.Delete(userId);
        Assert.IsInstanceOfType(result, typeof(NoContentResult));
    }
}
