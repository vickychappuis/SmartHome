using smarthome.Dtos;

namespace smarthome.Domain;

public class SmartLamp : Device
{
    public bool IsTurnedOn { get; set; }
    public SmartLamp() { }
    public SmartLamp(DeviceDto smartLampDto)
    {
        Name = smartLampDto.Name;
        Model = smartLampDto.Model;
        DeviceType = DeviceType.SmartLamp;
        Description = smartLampDto.Description;
        ImageUrl = smartLampDto.ImageUrl;
        CompanyId = smartLampDto.CompanyId;
        IsTurnedOn = false;
    }

    public void ChangeLampState(bool newState)
    {
        IsTurnedOn = newState;
    }
}

