using smarthome.Domain;

namespace smarthome.IBusinessLogic;
public interface IDeviceValidator
{
    public string Modelo { get; }
    public bool EsValido(Device device);
}
