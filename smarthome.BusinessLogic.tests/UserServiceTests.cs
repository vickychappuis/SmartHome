using System.Linq.Expressions;
using Moq;
using smarthome.Domain;
using smarthome.Dtos;
using smarthome.IDataAccess;

namespace smarthome.BusinessLogic.tests;

[TestClass]
public class UserServiceTests
{
    private Mock<IGenericRepository<User>> _userRepositoryMock;

    private UserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IGenericRepository<User>>();

        _userService = new UserService(_userRepositoryMock.Object);
    }

    [TestMethod]
    public void GetAll_WithoutUsers_Should_Return_No_Users()
    {
        _userRepositoryMock
            .Setup(repo => repo.GetAll(It.IsAny<Func<User, bool>>(), null))
            .Returns(new List<User>());

        var users = _userService.GetUsers();

        Assert.IsFalse(users.Any());
    }


    [TestMethod]
    public void DeleteUser_IsAdmin_ShouldDeleteUser()
    {
        var userId = 1;
        var expectedUser = new User { UserId = userId, Role = UserRoles.Administrator };

        _userRepositoryMock
            .Setup(repo =>
                repo.Get(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<List<string>>())
            )
            .Returns(expectedUser);

        _userService.DeleteUser(userId);

        _userRepositoryMock.Verify(repo => repo.Delete(It.IsAny<User>()), Times.Once);
    }

    [TestMethod]
    public void DeleteUser_UserNotFound_ShouldThrow()
    {
        var userId = 1;

        _userRepositoryMock
            .Setup(repo =>
                repo.Get(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<List<string>>())
            )
            .Returns((User?)null);

        var exception = Assert.ThrowsException<Exception>(() => _userService.DeleteUser(userId));

        Assert.AreEqual("User not found", exception.Message);
    }

    [DataRow(UserRoles.HomeOwner)]
    [DataRow(UserRoles.CompanyOwner)]
    [TestMethod]
    public void DeleteUser_UserNotAdmin_ShouldThrow(UserRoles role)
    {
        var userId = 1;
        var user = new User { UserId = userId, Role = role };

        _userRepositoryMock
            .Setup(repo =>
                repo.Get(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<List<string>>())
            )
            .Returns(user);

        var exception = Assert.ThrowsException<Exception>(() => _userService.DeleteUser(userId));

        Assert.AreEqual("Cannot delete. User is not an administrator", exception.Message);
    }

    [TestMethod]
    public void UpdateUser_ShouldUpdateUser()
    {
        var user = new User();

        _userService.UpdateUser(user);

        _userRepositoryMock.Verify(repo => repo.Update(It.IsAny<User>()), Times.Once);
    }

    [TestMethod]
    public void GetById_ShouldReturnUser()
    {
        var userId = 1;
        var expectedUser = new User { UserId = userId };

        _userRepositoryMock
            .Setup(repo =>
                repo.Get(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<List<string>>())
            )
            .Returns(expectedUser);

        var user = _userService.GetById(userId);

        Assert.IsNotNull(user);
        Assert.AreEqual(expectedUser.UserId, user.UserId);
    }

    [TestMethod]
    public void GetById_UserNotFound_ShouldThrow()
    {
        var userId = 1;

        _userRepositoryMock
            .Setup(repo =>
                repo.Get(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<List<string>>())
            )
            .Returns((User?)null);

        var exception = Assert.ThrowsException<Exception>(() => _userService.GetById(userId));

        Assert.AreEqual("User not found", exception.Message);
    }

    [TestMethod]
    public void GetUsers_WithNoUsers_ShouldReturnEmpty()
    {
        _userRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<User>());

        var users = _userService.GetUsers();

        Assert.IsFalse(users.Any());
    }

    [DataRow(1)]
    [DataRow(9)]
    [DataRow(16)]
    [DataRow(20)]
    [TestMethod]
    public void GetUsers_WithLte20Users_ShouldReturnAllUsers(int count)
    {
        _userRepositoryMock
            .Setup(repo => repo.GetAll(It.IsAny<Func<User, bool>>(), null))
            .Returns(Enumerable.Repeat(new User(), count));

        var users = _userService.GetUsers();

        Assert.AreEqual(count, users.Count());
    }

    [DataRow(21)]
    [DataRow(24)]
    [DataRow(32)]
    [DataRow(68)]
    [DataRow(126)]
    [TestMethod]
    public void GetUsers_WithGt20Users_ShouldReturn20Users(int count)
    {
        _userRepositoryMock
            .Setup(repo => repo.GetAll(It.IsAny<Func<User, bool>>(), null))
            .Returns(Enumerable.Repeat(new User(), count));

        var users = _userService.GetUsers();

        Assert.AreEqual(20, users.Count());
    }

    [DataRow(1)]
    [DataRow(5)]
    [DataRow(10)]
    [DataRow(20)]
    [TestMethod]
    public void GetUsers_WithSkip_ShouldStartAtSkip(int skip)
    {
        var mockUsers = Enumerable
            .Range(1, 40)
            .Select(i => new User { UserId = i, FirstName = $"User {i}" })
            .ToList();

        _userRepositoryMock
            .Setup(repo => repo.GetAll(It.IsAny<Func<User, bool>>(), null))
            .Returns(mockUsers);

        var users = _userService.GetUsers(skip: skip);

        Assert.AreEqual(20, users.Count());
        Assert.AreEqual(skip + 1, users.First().UserId);
    }

    [DataRow(1)]
    [DataRow(5)]
    [DataRow(10)]
    [DataRow(20)]
    [TestMethod]
    public void GetUsers_WithTake_ShouldReturnTake(int take)
    {
        var mockUsers = Enumerable
            .Range(1, 40)
            .Select(i => new User { UserId = i, FirstName = $"User {i}" })
            .ToList();

        _userRepositoryMock
            .Setup(repo => repo.GetAll(It.IsAny<Func<User, bool>>(), null))
            .Returns(mockUsers);

        var users = _userService.GetUsers(take: take);

        Assert.AreEqual(take, users.Count());
    }

    [DataRow(UserRoles.Administrator)]
    [DataRow(UserRoles.CompanyOwner)]
    [TestMethod]
    public void CreateUser_CompanyOwnerOrAdministrator_ShouldCreateUser(UserRoles role)
    {

        var user = new UserDto { role = role.ToString() };

        _userService.CreateUser(user);

        _userRepositoryMock.Verify(repo => repo.Insert(It.IsAny<User>()), Times.Once);
    }

    [TestMethod]
    public void CreateUser_NotAdminOrCompanyOwner_ShouldThrow()
    {
        var user = new UserDto { role = UserRoles.HomeOwner.ToString() };

        var exception = Assert.ThrowsException<Exception>(() => _userService.CreateUser(user));

        Assert.AreEqual(
            "Cannot create a user that's not a company owner or administrator",
            exception.Message
        );
    }
}
