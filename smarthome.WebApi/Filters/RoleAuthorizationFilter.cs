using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using smarthome.Domain;
using smarthome.IBusinessLogic;

public class RoleAuthorizationFilter(UserRoles role = UserRoles.None)
    : Attribute,
        IAuthorizationFilter
{
    private readonly UserRoles _role = role;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        string bearerToken = context.HttpContext.Request.Headers["Authorization"].ToString();

        if (String.IsNullOrEmpty(bearerToken))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        string token = String.Join("", bearerToken.Split("Bearer ").Skip(1));

        IAuthService authService =
            context.HttpContext.RequestServices.GetRequiredService<IAuthService>();

        Session? session = authService.GetSession(token);

        if (session is null || (_role != UserRoles.None && session.User.Role != _role))
        {
            context.Result = new ForbidResult();
            return;
        }

        Console.WriteLine(
            $"User {session.User.FirstName} is authorized"
                + ((_role != UserRoles.None) ? $" as {_role}" : "")
        );

        context.HttpContext.Items.Add("Session", session);

    }
}
