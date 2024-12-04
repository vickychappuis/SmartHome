using System.Text;
using smarthome.Domain;
using smarthome.Dtos;
using smarthome.IBusinessLogic;
using smarthome.IDataAccess;

namespace smarthome.BusinessLogic;

public class AuthService : IAuthService
{
    private readonly IGenericRepository<Session> _sessionRepository;
    private readonly IGenericRepository<User> _userRepository;

    public AuthService(
        IGenericRepository<Session> sessionRepository,
        IGenericRepository<User> userRepository
    )
    {
        _sessionRepository = sessionRepository;
        _userRepository = userRepository;
    }

    public Session Login(AuthDto auth)
    {
        var user = _userRepository.Get(
            (user => user.Email == auth.Email && user.Password == auth.Password)
        );
        if (user == null)
        {
            throw new Exception("Invalid username or password");
        }

        return CreateSession(user);
    }

    public Session Signup(SignupDto dto)
    {
        var existingUser = _userRepository.Get((user => user.Email == dto.Email));

        if (existingUser is not null)
        {
            throw new Exception("User already exists");
        }

        var user = new User(dto);

        _userRepository.Insert(user);

        return CreateSession(user);
    }

    private Session CreateSession(User user)
    {
        const uint EXPIRATION_HOURS = 24;

        var session = new Session
        {
            Token = Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString())),
            UserId = user.UserId,
            Expires = DateTime.Now.AddHours(EXPIRATION_HOURS),
        };

        _sessionRepository.Insert(session);

        return session;
    }

    public void Logout(string token)
    {
        var session = _sessionRepository.Get(session => session.Token == token);

        if (session is null)
        {
            throw new Exception("Invalid session token");
        }

        _sessionRepository.Delete(session);
    }

    public Session? GetSession(string token)
    {
        var session = _sessionRepository.Get(session => session.Token == token, ["User"]);

        if (session is not null && session.Expires < DateTime.Now)
        {
            _sessionRepository.Delete(session);
            return null;
        }

        return session;
    }
}
