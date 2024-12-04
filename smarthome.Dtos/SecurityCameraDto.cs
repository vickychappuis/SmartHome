namespace smarthome.Dtos;

public class SecurityCameraDto : DeviceDto
{
    public bool IsInterior { get; set; }
    public bool IsExterior { get; set; }
    public bool CanDetectMotion { get; set; }
    public bool CanDetectPerson { get; set; }
}
