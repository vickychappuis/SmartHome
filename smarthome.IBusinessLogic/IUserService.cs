using smarthome.Domain;
using smarthome.Dtos;

namespace smarthome.IBusinessLogic;

public interface IUserService
{
    public IEnumerable<User> GetUsers(
        int? skip = 0,
        int? take = 20,
        string? name = null,
        UserRoles? userRole = null
    );
    public void DeleteUser(int userId);
    public User GetById(int userId);
    public void UpdateUser(User user);

    public void CreateUser(UserDto user);
}
