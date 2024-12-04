using smarthome.Domain;
using smarthome.Dtos;

namespace smarthome.IBusinessLogic;

public interface IAuthService
{
    Session Login(AuthDto dto);
    Session Signup(SignupDto dto);
    Session? GetSession(string token);
    void Logout(string token);
}
