using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using smarthome.Domain;
using smarthome.Dtos;
using smarthome.IBusinessLogic;
using smarthome.WebApi.Controllers;

namespace smarthome.WebApi.tests;

[TestClass]
public class AuthControllerTests
{
    private Mock<IAuthService> _authServiceMock;
    private DefaultHttpContext _httpContext;
    private AuthController _authController;
    
    [TestInitialize]
    public void Setup()
    {
        _authServiceMock = new Mock<IAuthService>();
        _httpContext = new DefaultHttpContext();
        _authController = new AuthController(_authServiceMock.Object);
        _authController.ControllerContext = new ControllerContext()
        {
            HttpContext = _httpContext
        };
    }

    [TestMethod]
    public void Login_InvalidUser_ShouldReturnUnauthorized()
    {
        _authServiceMock.Setup(service => service.Login(It.IsAny<AuthDto>())).Throws(new Exception("Invalid user name or password"));        
        var result = _authController.Login(new AuthDto());
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }
    
    [TestMethod]
    public void Login_ValidUser_ShouldReturnOk()
    {
        _authServiceMock.Setup(service => service.Login(It.IsAny<AuthDto>())).Returns(new Session());
        var result = _authController.Login(new AuthDto()
        {
            Email = "johndoe@gmail.com",
            Password = "password"
        });
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }
    
    [TestMethod]
    public void Signup_InvalidUser_ShouldReturnOk()
    {
        _authServiceMock.Setup(service => service.Signup(It.IsAny<SignupDto>())).Throws(new Exception("Username or password missing"));
        var result = _authController.Signup(new SignupDto());
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public void Signup_ValidUser_ShouldReturnOk()
    {
        _authServiceMock.Setup(service => service.Signup(It.IsAny<SignupDto>())).Returns(new Session());
        var result = _authController.Signup(new SignupDto()
        {
            Email = "johndoe@gmail.com",
            Password = "password",
            FirstName = "John",
            LastName = "Doe"
        });
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void Logout_UserNotLoggedIn_ShouldThrowException()
    {
        _authServiceMock.Setup(service => service.Logout(null)).Throws(new Exception("Invalid session token"));
        var result = _authController.Logout();
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }
    
    [TestMethod]
    public void Logout_UserLoggedIn_ShouldReturnOk()
    {
        _authController.HttpContext.Items["Session"] = new Session()
        {
            User = new User()
            {
                UserId = 1
            }
        };
        _authServiceMock.Setup(service => service.Logout(It.IsAny<string>()));
        var result = _authController.Logout();
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }

}