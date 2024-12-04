using smarthome.Domain;
using smarthome.IBusinessLogic;

namespace DeviceValidators;

public class ModelHasThreeLettersThreeNumbersValidator : IDeviceValidator
{
    public string Modelo => "ModelHasThreeLettersThreeNumbersValidator";
    public bool EsValido(Device device)
    {
        return device.Model.Length == 6 && device.Model.Take(3).All(char.IsLetter) && device.Model.Skip(3).All(char.IsDigit);
    }
}
