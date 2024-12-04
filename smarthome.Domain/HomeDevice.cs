using System.ComponentModel.DataAnnotations;

namespace smarthome.Domain;

public class HomeDevice
{
    [Key]
    public int HardwareId { get; set; }

    [Required]
    public int HomeId { get; set; }

    [Required]
    public Home Home { get; set; }

    public int? RoomId { get; set; }

    public Room? Room { get; set; }

    [Required]
    public int DeviceId { get; set; }

    [Required]
    public Device Device { get; set; }

    [Required]
    public bool IsConnected { get; set; } = true;

    public bool? IsOpen { get; set; }

    [Required]
    public string DeviceName { get; set; }
}
