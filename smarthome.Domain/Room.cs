using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using smarthome.Dtos;

namespace smarthome.Domain;

public class Room
{
    [Key]
    public int RoomId { get; set; }

    [Required]
    public string RoomName { get; set; }

    [Required]
    public int HomeId { get; set; }

    [Required]
    [JsonIgnore]
    public Home Home { get; set; }

    public Room(RoomDto roomDto)
    {
        RoomName = roomDto.RoomName;
        HomeId = roomDto.HomeId;
    }

    public Room() { }
}