using smarthome.Domain;
using smarthome.IBusinessLogic;

namespace DeviceValidators;

public class ModelHasSixLettersValidator : IDeviceValidator
{
    public string Modelo => "ModelHasSixLettersValidator";
    public bool EsValido(Device device)
    {
        return device.Model.Length == 6 && device.Model.All(char.IsLetter);
    }
}
