using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smarthome.Domain;

public class HomeMember
{
    [Required]
    public int HomeId { get; set; }

    [Required]
    public Home Home { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public User User { get; set; }

    [Required]
    public bool CanAddDevice { get; set; } = false;

    [Required]
    public bool CanListDevices { get; set; } = false;

    [Required]
    public bool CanReceiveNotifications { get; set; } = false;

    public HomeMember() { }
}
