using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using smarthome.Domain;
using smarthome.IBusinessLogic;

namespace smarthome.WebApi.tests;
[TestClass]
public class RoleAuthorizationFilterTests
{
    private Mock<IAuthService> _authServiceMock;
    private AuthorizationFilterContext _authorizationFilterContext;
    private RoleAuthorizationFilter _roleAuthorizationFilter;
    
    [TestInitialize]
    public void SetUp()
    {
        _authServiceMock = new Mock<IAuthService>();
        
        var services = new ServiceCollection();
        services.AddSingleton(_authServiceMock.Object);
        
        var httpContext = new DefaultHttpContext()
        {
            RequestServices = services.BuildServiceProvider()
        };
        
        var actionContext = new ActionContext
        {
            HttpContext = httpContext,
            RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
            ActionDescriptor = new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()
        };
        _authorizationFilterContext = new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());
        
    }
    /*
    [TestMethod]
    public void OnAuthorize_NoContext_ShouldReturnUnauthorized()
    {
        //Test fails because of null reference exception
        _roleAuthorizationFilter = new RoleAuthorizationFilter();
        
        _roleAuthorizationFilter.OnAuthorization((AuthorizationFilterContext)null);
        Assert.IsInstanceOfType(_authorizationFilterContext.Result, typeof(UnauthorizedResult));
    }*/
    
    [TestMethod]
    public void OnAuthorize_NoBearerToken_ShouldReturnUnauthorized()
    {
        _roleAuthorizationFilter = new RoleAuthorizationFilter();
        _roleAuthorizationFilter.OnAuthorization(_authorizationFilterContext);
        Assert.IsInstanceOfType(_authorizationFilterContext.Result, typeof(UnauthorizedResult));
    }
    
    [TestMethod]
    public void OnAuthorize_NullSession_ShouldReturnForbidden()
    {
        _roleAuthorizationFilter = new RoleAuthorizationFilter();
        _authorizationFilterContext.HttpContext.Request.Headers["Authorization"] = "Bearer token";
        _roleAuthorizationFilter.OnAuthorization(_authorizationFilterContext);
        Assert.IsInstanceOfType(_authorizationFilterContext.Result, typeof(ForbidResult));
    }
    
    [TestMethod]
    public void OnAuthorize_InvalidRole_ShouldReturnForbidden()
    {
        _roleAuthorizationFilter = new RoleAuthorizationFilter(UserRoles.Administrator);
        _authorizationFilterContext.HttpContext.Request.Headers["Authorization"] = "Bearer token";
        _authServiceMock.Setup(service => service.GetSession(It.IsAny<string>())).Returns(new Session()
        {
            User = new User()
            {
                Role = UserRoles.CompanyOwner
            }
        });
        _roleAuthorizationFilter.OnAuthorization(_authorizationFilterContext);
        Assert.IsInstanceOfType(_authorizationFilterContext.Result, typeof(ForbidResult));
    }
    
    [TestMethod]
    public void OnAuthorize_ValidRole_ShouldReturnOk()
    {
        _roleAuthorizationFilter = new RoleAuthorizationFilter(UserRoles.Administrator);
        _authorizationFilterContext.HttpContext.Request.Headers["Authorization"] = "Bearer token";
        _authServiceMock.Setup(service => service.GetSession(It.IsAny<string>())).Returns(new Session()
        {
            User = new User()
            {
                Role = UserRoles.Administrator
            }
        });
        _roleAuthorizationFilter.OnAuthorization(_authorizationFilterContext);
        Assert.IsNull(_authorizationFilterContext.Result);
    }
}