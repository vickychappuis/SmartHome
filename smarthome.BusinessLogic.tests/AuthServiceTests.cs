using System.Linq.Expressions;
using Moq;
using smarthome.Domain;
using smarthome.Dtos;
using smarthome.IDataAccess;

namespace smarthome.BusinessLogic.tests;

[TestClass]
public class AuthServiceTests
{
    private Mock<IGenericRepository<Session>> _sessionRepositoryMock;
    private Mock<IGenericRepository<User>> _userRepositoryMock;

    private AuthService _authService;

    public AuthServiceTests()
    {
        _sessionRepositoryMock = new Mock<IGenericRepository<Session>>();
        _userRepositoryMock = new Mock<IGenericRepository<User>>();

        _authService = new AuthService(_sessionRepositoryMock.Object, _userRepositoryMock.Object);
    }

    [TestMethod]
    public void Login_WithValidCredentials_ShouldReturnSession()
    {
        var authDto = new AuthDto { Email = "john@mail.com", Password = "MyPass!3" };

        var user = new User
        {
            UserId = 32,
            Email = authDto.Email,
            Password = authDto.Password,
        };

        _userRepositoryMock
            .Setup(repo =>
                repo.Get(
                    It.Is<Expression<Func<User, bool>>>(condition =>
                        condition.Compile().Invoke(user)
                    ),
                    null
                )
            )
            .Returns(user);

        _sessionRepositoryMock.Setup(repo => repo.Insert(It.IsAny<Session>()));

        var session = _authService.Login(authDto);

        Assert.AreEqual(user.UserId, session.UserId);
        Assert.IsNotNull(session.Token);
    }
    
    [TestMethod]
    public void Login_WithInvalidCredentials_ShouldThrowException()
    {
        var authDto = new AuthDto { Email = "", Password = "" };
        
        _userRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<User, bool>>>(), null))
            .Returns((User?)null);
        
        var ex = Assert.ThrowsException<Exception>(() => _authService.Login(authDto));
        
        Assert.AreEqual("Invalid username or password", ex.Message);
    }

    [TestMethod]
    public void Signup_UserAlreadyExists_ShouldThrowException()
    {
        var signupDto = new SignupDto
        {
            Email = "john@mail.com",
            Password = "Password123",
            FirstName = "John",
            LastName = "Doe",
            ImageUrl = "https://example.com",
        };
        var existingUser = new User { Email = signupDto.Email };

        _userRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<User, bool>>>(), null))
            .Returns(existingUser);

        var ex = Assert.ThrowsException<Exception>(() => _authService.Signup(signupDto));
        Assert.AreEqual("User already exists", ex.Message);
    }

    [TestMethod]
    public void Signup_NewUser_ShouldCreateSession()
    {
        var signupDto = new SignupDto
        {
            Email = "newuser@mail.com",
            Password = "Password123",
            FirstName = "New",
            LastName = "User",
        };

        _userRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<User, bool>>>(), null))
            .Returns((User?)null);

        var newUser = new User
        {
            Email = signupDto.Email,
            Password = signupDto.Password,
            FirstName = signupDto.FirstName,
            LastName = signupDto.LastName,
        };

        _userRepositoryMock
            .Setup(repo => repo.Insert(It.IsAny<User>()))
            .Callback<User>(u => u.UserId = 1); // Simulating that Insert sets an ID for the user

        var session = _authService.Signup(signupDto);

        _userRepositoryMock.Verify(repo => repo.Insert(It.IsAny<User>()), Times.Once);
        Assert.AreEqual(1, session.UserId);
    }

    [TestMethod]
    public void Logout_ValidToken_ShouldDeleteSession()
    {
        var validToken = "valid_token";
        var session = new Session { Token = validToken };

        _sessionRepositoryMock
            .Setup(repo =>
                repo.Get(
                    It.Is<Expression<Func<Session, bool>>>(condition =>
                        condition.Compile().Invoke(session)
                    ),
                    null
                )
            )
            .Returns(session);

        _authService.Logout(validToken);

        _sessionRepositoryMock.Verify(repo => repo.Delete(session), Times.Once);
    }

    [TestMethod]
    public void Logout_InvalidToken_ShouldThrowException()
    {
        var invalidToken = "invalid_token";

        _sessionRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Session, bool>>>(), null))
            .Returns((Session?)null); // No session found for the token

        var ex = Assert.ThrowsException<Exception>(() => _authService.Logout(invalidToken));
        Assert.AreEqual("Invalid session token", ex.Message);
    }

    [TestMethod]
    public void GetSession_ValidToken_ShouldReturnSession()
    {
        var validToken = "valid_token";
        var session = new Session { Token = validToken };

        _sessionRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Session, bool>>>(), It.IsAny<List<string>>()))
            .Returns(session);

        var result = _authService.GetSession(validToken);
        
        _sessionRepositoryMock.Verify(repo =>  repo.Get(It.IsAny<Expression<Func<Session, bool>>>(), It.IsAny<List<string>>()), Times.Once);

    }

    [TestMethod]
    public void GetSession_ExpiredToken_ShouldReturnNull()
    {
        _sessionRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<Session, bool>>>(), It.IsAny<List<string>>()))
            .Returns(new Session()
            {
                Expires = DateTime.Now.AddHours(-1),
                Token = "expired_token",
            }); 
        
        _sessionRepositoryMock
            .Setup(repo => repo.Delete(It.IsAny<Session>()));
        
        var session = _authService.GetSession("expired_token");
        
        Assert.IsNull(session);
    }
}
