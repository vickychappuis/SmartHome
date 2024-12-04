using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using smarthome.Dtos;

namespace smarthome.Domain;

public class Company
{
    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int CompanyId { get; set; }

    [Required]
    public string Name { get; set; }

    public string LogotypeUrl { get; set; }

    [Required]
    public int Rut { get; set; }

    [Required]
    public int CompanyOwnerId { get; set; }

    public User CompanyOwner { get; set; }

    public string? DeviceValidator { get; set; }

    public Company() { }

    public Company(CompanyDto dto)
    {
        Name = dto.Name;
        LogotypeUrl = dto.LogotypeUrl;
        Rut = dto.Rut;
        DeviceValidator = dto.DeviceValidator;
    }
}
