using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smarthome.Domain;

public class Notification
{
    [Key]
    public int NotificationId { get; set; }

    [Required]
    public string EventName { get; set; }

    [Required]
    [ForeignKey(nameof(HomeDevice))]
    public int HardwareId { get; set; }

    [Required]
    public string CreatedAt { get; set; }
}
