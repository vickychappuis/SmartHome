using System.ComponentModel.DataAnnotations;

namespace smarthome.Domain;

public enum DeviceType
{
    SecurityCamera = 0,
    WindowSensor = 1,
    SmartLamp = 2,
    MotionSensor = 3
}

public abstract class Device
{
    [Key]
    public int DeviceId { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public DeviceType DeviceType { get; set; }

    public string Model { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public string ImageUrl { get; set; }

    [Required]
    public int CompanyId { get; set; }

    [Required]
    public Company Company { get; set; }

    public Device() { }
}
