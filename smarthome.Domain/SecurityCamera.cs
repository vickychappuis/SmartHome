using smarthome.Dtos;

namespace smarthome.Domain;

public class SecurityCamera : Device
{
    public bool IsInterior { get; set; }
    public bool IsExterior { get; set; }
    public bool CanDetectMotion { get; set; }
    public bool CanDetectPerson { get; set; }

    public SecurityCamera() { }

    public SecurityCamera(DeviceDto dto)
    {
        if (dto.CanDetectMotion is null || dto.CanDetectPerson is null)
        {
            throw new ArgumentException(
                "You must specify if the camera can detect motion and/or people."
            );
        }
        else if (dto.IsInterior is null || dto.IsExterior is null)
        {
            throw new ArgumentException(
                "You must specify if the camera is for interior or exterior use."
            );
        }

        Name = dto.Name;
        Model = dto.Model;
        DeviceType = DeviceType.SecurityCamera;
        Description = dto.Description;
        ImageUrl = dto.ImageUrl;
        CompanyId = dto.CompanyId;
        IsInterior = (bool)dto.IsInterior;
        IsExterior = (bool)dto.IsExterior;
        CanDetectMotion = (bool)dto.CanDetectMotion;
        CanDetectPerson = (bool)dto.CanDetectPerson;
    }
}
