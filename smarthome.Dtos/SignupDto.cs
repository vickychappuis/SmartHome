using System.ComponentModel.DataAnnotations;

namespace smarthome.Dtos;

public class SignupDto
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public string ImageUrl { get; set; }
}
