namespace smarthome.Dtos;

public class DeviceDto
{
    public string Name { get; set; }
    public string Model { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public int CompanyId { get; set; }
    public int? RoomId { get; set; }
    public bool? IsInterior { get; set; }
    public bool? IsExterior { get; set; }
    public bool? CanDetectMotion { get; set; }
    public bool? CanDetectPerson { get; set; }
}
