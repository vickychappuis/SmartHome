using smarthome.Domain;

namespace smarthome.IBusinessLogic;

public interface INotificationService
{
    public void CreateNotification(int deviceId, int homeDeviceId, string notificationType, int? userId);
    public IEnumerable<Notification> GetNotificationsByIds(int[] notificationsId);
}
