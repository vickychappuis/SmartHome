using smarthome.Dtos;

namespace smarthome.Domain;

public class WindowSensor : Device
{
    public WindowSensor() { }

    public WindowSensor(DeviceDto windowSensorDto)
    {
        this.Name = windowSensorDto.Name;
        this.Model = windowSensorDto.Model;
        this.Description = windowSensorDto.Description;
        this.ImageUrl = windowSensorDto.ImageUrl;
        this.CompanyId = windowSensorDto.CompanyId;
    }
}
