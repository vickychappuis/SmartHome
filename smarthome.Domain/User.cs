using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using smarthome.Dtos;

namespace smarthome.Domain
{
    [Flags]
    public enum UserRoles
    {
        None = -1,
        Administrator = 0,
        CompanyOwner = 1,
        HomeOwner = 2,
    }

    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public UserRoles Role { get; set; }

        [Required]
        public DateTime RegisterDate { get; set; }

        [JsonIgnore]
        public Company? Company { get; set; }

        public User() { }

        public User(SignupDto dto)
        {
            FirstName = dto.FirstName;
            LastName = dto.LastName;
            Email = dto.Email;
            Password = dto.Password;
            RegisterDate = DateTime.Now;
            Role = UserRoles.HomeOwner;
            ImageUrl = dto.ImageUrl;
        }

        public User(UserDto dto)
        {
            FirstName = dto.firstName;
            LastName = dto.lastName;
            Email = dto.email;
            Password = dto.password;
            RegisterDate = DateTime.Now;
            Role = (UserRoles)Enum.Parse(typeof(UserRoles), dto.role);
            ImageUrl = dto.imageUrl;
            Company = null;
        }
    }
}
