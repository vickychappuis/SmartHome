using smarthome.Dtos;

namespace smarthome.Domain;

public class MotionSensor : Device
{
    public MotionSensor() { }

    public MotionSensor(DeviceDto MotionSensorDto)
    {
        this.Name = MotionSensorDto.Name;
        this.Model = MotionSensorDto.Model;
        this.DeviceType = DeviceType.MotionSensor;

        this.Description = MotionSensorDto.Description;
        this.ImageUrl = MotionSensorDto.ImageUrl;
        this.CompanyId = MotionSensorDto.CompanyId;
    }
}
