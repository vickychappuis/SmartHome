using System.ComponentModel.DataAnnotations;

namespace smarthome.Dtos;

public class HomeDto
{
    [Key]
    public int HomeId { get; set; }

    [Required]
    public string Address { get; set; }

    [Required]
    public double Latitude { get; set; }

    [Required]
    public double Longitude { get; set; }

    [Required]
    public int MaxMembers { get; set; }

    public string? HomeName { get; set; }
}
