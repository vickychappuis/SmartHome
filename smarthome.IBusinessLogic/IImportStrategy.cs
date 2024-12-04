using smarthome.Domain;

namespace smarthome.IBusinessLogic;
public interface IImportStrategy
{
    string Name { get; }
    IEnumerable<Device> Import(string data);
}
