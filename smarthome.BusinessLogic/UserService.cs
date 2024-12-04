using smarthome.Domain;
using smarthome.Dtos;
using smarthome.IBusinessLogic;
using smarthome.IDataAccess;

namespace smarthome.BusinessLogic;

public class UserService : IUserService
{
    private readonly IGenericRepository<User> _userRepository;

    public UserService(IGenericRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public void DeleteUser(int userId)
    {
        var user = GetById(userId);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        else if (!user.Role.Equals(UserRoles.Administrator))
        {
            throw new Exception("Cannot delete. User is not an administrator");
        }
        _userRepository.Delete(user);
    }

    public User GetById(int userId)
    {
        var user = _userRepository.Get(u => u.UserId == userId);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        return user;
    }

    public void UpdateUser(User user)
    {
        _userRepository.Update(user);
    }

    public IEnumerable<User> GetUsers(
        int? skip = 0,
        int? take = 20,
        string? name = null,
        UserRoles? userRole = null
    )
    {
        var users = _userRepository
            .GetAll(u =>
                (name == null || u.FirstName.Contains(name))
                && (userRole == null || u.Role.Equals(userRole))
            )
            .Skip(skip ?? 0)
            .Take(take ?? 20);
        return users;
    }

    public void CreateUser(UserDto userDto)
    {
        if (
            userDto.role != UserRoles.Administrator.ToString()
            && userDto.role != UserRoles.CompanyOwner.ToString()
        )
        {
            throw new Exception("Cannot create a user that's not a company owner or administrator");
        }

        var user = new User(userDto);

        _userRepository.Insert(user);
    }
}
