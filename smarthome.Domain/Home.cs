using System.ComponentModel.DataAnnotations;
using smarthome.Dtos;

namespace smarthome.Domain;

public class Home
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

        [Required]
        public ICollection<Room> Rooms { get; set; }

        public string? HomeName { get; set; }

        public Home() { }

        public Home(HomeDto dto)
        {
                HomeId = dto.HomeId;
                Address = dto.Address;
                Latitude = dto.Latitude;
                Longitude = dto.Longitude;
                MaxMembers = dto.MaxMembers;
                HomeName = dto.HomeName;
                Rooms = new List<Room>();
        }
}
