namespace smarthome.Dtos;

public class NotificationDto
{
    public int NotificationId { get; set; }
    public string EventName { get; set; }
    public int DeviceId { get; set; }
    public bool Read { get; set; } = false;
    public string CreatedAt { get; set; }
}