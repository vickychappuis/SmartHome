namespace smarthome.Dtos;

public class HomeOwnerDto
{
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string Email { get; set; }
    public bool CanAddDevice { get; set; }
    public bool CanListDevices { get; set; }
    public bool CanReceiveNotifications { get; set; }
    public string ImageUrl { get; set; }

}