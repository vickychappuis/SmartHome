using System.Text.Json;
using smarthome.Domain;
using smarthome.Dtos;
using smarthome.IBusinessLogic;

namespace ImportStrategies;

public class JsonDto
{
    public List<JsonDeviceDto> Dispositivos { get; set; }
}

public class JsonDeviceDto
{
    public string Id { get; set; }
    public string Tipo { get; set; }
    public string Nombre { get; set; }
    public string Modelo { get; set; }
    public List<JsonPhotoDto> Fotos { get; set; }
    public bool PersonDetection { get; set; }
    public bool MovementDetection { get; set; }
}

public class JsonPhotoDto
{
    public string Path { get; set; }
    public bool EsPrincipal { get; set; }

}

class JsonDeviceFactory
{
    public Device Create(JsonDeviceDto deviceDto)
    {
        Device device = deviceDto.Tipo switch
        {
            "camera" => new SecurityCamera
            {
                Name = deviceDto.Nombre,
                Model = deviceDto.Modelo,
                Description = "A security camera",
                DeviceType = DeviceType.SecurityCamera,
                ImageUrl = deviceDto.Fotos.FirstOrDefault(f => f.EsPrincipal)?.Path ?? deviceDto.Fotos.First().Path,
                CanDetectMotion = deviceDto.MovementDetection,
                CanDetectPerson = deviceDto.PersonDetection,
                IsInterior = true,
                IsExterior = false
            },
            "sensor-open-close" => new WindowSensor
            {
                Name = deviceDto.Nombre,
                Model = deviceDto.Modelo,
                Description = "A window sensor",
                DeviceType = DeviceType.WindowSensor,
                ImageUrl = deviceDto.Fotos.FirstOrDefault(f => f.EsPrincipal)?.Path ?? deviceDto.Fotos.First().Path
            },
            "sensor-movement" => new MotionSensor
            {
                Name = deviceDto.Nombre,
                Model = deviceDto.Modelo,
                Description = "A motion sensor",
                DeviceType = DeviceType.MotionSensor,
                ImageUrl = deviceDto.Fotos.FirstOrDefault(f => f.EsPrincipal)?.Path ?? deviceDto.Fotos.First().Path
            },
            _ => throw new ArgumentException($"Invalid device type: {deviceDto.Tipo}")
        };

        return device;
    }
}

public class ImportJsonStrategy : IImportStrategy
{
    private readonly JsonDeviceFactory _deviceFactory = new JsonDeviceFactory();

    public string Name => "ImportJsonStrategy";

    public IEnumerable<Device> Import(string data)
    {
        var jsonDto = JsonSerializer.Deserialize<JsonDto>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Console.WriteLine(jsonDto != null);

        Console.WriteLine(jsonDto!.Dispositivos != null);
        Console.WriteLine(jsonDto!.Dispositivos.Count);

        var devices = jsonDto.Dispositivos.Select(_deviceFactory.Create);

        return devices;
    }
}
