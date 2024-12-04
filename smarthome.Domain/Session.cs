using System.ComponentModel.DataAnnotations;

namespace smarthome.Domain
{
    public class Session
    {
        [Key]
        public string Token { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        public DateTime Expires { get; set; }
    }
}
