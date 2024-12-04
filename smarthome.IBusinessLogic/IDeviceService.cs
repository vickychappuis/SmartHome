using smarthome.Domain;
using smarthome.Dtos;

namespace smarthome.IBusinessLogic;

public interface IDeviceService
{
    public void AddDevice(DeviceDto dto, DeviceType deviceType);
    public IEnumerable<Device> GetDevices(
        int? skip = 0,
        int? take = 20,
        string? name = null,
        string? model = null,
        string? companyName = null,
        DeviceType? deviceType = null
    );
    public List<DeviceType> ListDeviceTypes();
    public List<string> ListImportStrategies(string basePath = "");
    public void ImportDevices(string data, string importStrategy, int companyId, string basePath = "");
    public Device GetDevice(int id);
    public List<string> ListValidators(string basePath = "");
}
