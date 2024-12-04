using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smarthome.Domain;

public class HomeMemberNotification
{
  [Required]
  public int NotificationId { get; set; }

  [Required]
  public Notification Notification { get; set; }

  [Required]
  public int HomeId { get; set; }

  [Required]
  public int UserId { get; set; }

  [ForeignKey(nameof(HomeId) + "," + nameof(UserId))]
  [Required]
  public virtual HomeMember HomeMember { get; set; }

  [Required]
  public bool Read { get; set; } = false;
}
