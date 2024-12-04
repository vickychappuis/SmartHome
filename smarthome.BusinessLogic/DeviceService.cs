using smarthome.Domain;
using smarthome.Dtos;
using smarthome.IBusinessLogic;
using smarthome.IDataAccess;
using System.Reflection;

namespace smarthome.BusinessLogic;

public class DeviceService : IDeviceService
{
    private readonly IGenericRepository<Device> _deviceRepository;
    private readonly IGenericRepository<Company> _companyRepository;

    public DeviceService(IGenericRepository<Device> deviceRepository, IGenericRepository<Company> companyRepository)
    {
        _deviceRepository = deviceRepository;
        _companyRepository = companyRepository;
    }

    public void AddDevice(DeviceDto deviceDto, DeviceType deviceType)
    {
        Device device;

        switch (deviceType)
        {
            case DeviceType.SecurityCamera:
                device = new SecurityCamera(deviceDto);
                break;
            case DeviceType.WindowSensor:
                device = new WindowSensor(deviceDto);
                break;
            case DeviceType.SmartLamp:
                device = new SmartLamp(deviceDto);
                break;
            case DeviceType.MotionSensor:
                device = new MotionSensor(deviceDto);
                break;
            default:
                throw new ArgumentException("Invalid device type");
        }

        var company = _companyRepository.Get((x) => x.CompanyId == deviceDto.CompanyId);
        if (company == null)
        {
            throw new ArgumentException("Company not found");
        }

        if (!string.IsNullOrEmpty(company.DeviceValidator))
        {
            var validator = LoadValidator(company.DeviceValidator);
            var validatorMethod = validator.GetType().GetMethod("EsValido");
            if (validatorMethod == null)
            {
                throw new ArgumentException($"Validator method 'EsValido' not found on {validator.GetType().Name}");
            }
            if (!(bool)validatorMethod.Invoke(validator, new object[] { device }))
            {
                throw new ArgumentException($"Device does not pass '{company.DeviceValidator}' validator.");
            }
        }

        _deviceRepository.Insert(device);
    }

    private object LoadValidator(string validatorName, string basePath = "")
    {
        var assemblyPath = basePath + "../DeviceValidators/bin/debug/net8.0/DeviceValidators.dll";
        var assemblyBytes = File.ReadAllBytes(assemblyPath);
        var assembly = Assembly.Load(assemblyBytes);

        var dllBytes = File.ReadAllBytes("../DeviceValidators/ModeloValidadorAbstracciones.dll");
        var dll = Assembly.Load(dllBytes);

        var interfaceType = dll.GetType("ModeloValidador.Abstracciones.IModeloValidador");

        if (interfaceType == null)
        {
            throw new ArgumentException("Interface IModeloValidador not found");
        }

        var type = assembly.GetType($"DeviceValidators.{validatorName}");
        if (type == null)
        {
            throw new ArgumentException($"Validator {validatorName} not found");
        }

        return Activator.CreateInstance(type)!;
    }


    private IImportStrategy LoadStrategy(string importStrategy, string basePath = "")
    {
        var assemblyPath = basePath + "../ImportStrategies/bin/debug/net8.0/ImportStrategies.dll";
        var assemblyBytes = File.ReadAllBytes(assemblyPath);
        var assembly = Assembly.Load(assemblyBytes);

        var type = assembly.GetType($"ImportStrategies.{importStrategy}");
        if (type == null)
        {
            throw new ArgumentException($"Strategy {importStrategy} not found");
        }

        if (!typeof(IImportStrategy).IsAssignableFrom(type))
        {
            throw new ArgumentException($"Type {type.Name} does not implement IImportStrategy");
        }

        return (IImportStrategy)Activator.CreateInstance(type)!;
    }

    public void ImportDevices(string data, string importStrategy, int companyId, string basePath = "")
    {
        var strategy = LoadStrategy(importStrategy, basePath);
        var devices = strategy.Import(data);

        var company = _companyRepository.Get((x) => x.CompanyId == companyId);
        if (company == null)
        {
            throw new ArgumentException("Company not found");
        }

        if (!string.IsNullOrEmpty(company.DeviceValidator))
        {
            foreach (var device in devices)
            {
                var validator = LoadValidator(company.DeviceValidator, basePath);
                var validatorMethod = validator.GetType().GetMethod("EsValido");
                if (validatorMethod == null)
                {
                    throw new ArgumentException($"Validator method 'EsValido' not found on {validator.GetType().Name}");
                }
                if (!(bool)validatorMethod.Invoke(validator, new object[] { device }))
                {
                    throw new ArgumentException($"Device {device.Name} does not pass '{company.DeviceValidator}' validator.");
                }
            }
        }

        foreach (var device in devices)
        {
            device.CompanyId = companyId;
            _deviceRepository.Insert(device);
        }
    }

    public List<string> ListImportStrategies(string basePath = "")
    {
        var assemblyPath = basePath + "../ImportStrategies/bin/debug/net8.0/ImportStrategies.dll";
        var assemblyBytes = File.ReadAllBytes(assemblyPath);
        var assembly = Assembly.Load(assemblyBytes);

        var strategies = assembly.GetTypes()
            .Where(t => typeof(IImportStrategy).IsAssignableFrom(t) && !t.IsInterface)
            .Select(t => ((IImportStrategy)Activator.CreateInstance(t)!).Name)
            .ToList();
        return strategies;
    }

    public IEnumerable<Device> GetDevices(
        int? skip = 0,
        int? take = 20,
        string? name = null,
        string? model = null,
        string? companyName = null,
        DeviceType? deviceType = null
    )
    {
        var devices = _deviceRepository
            .GetAll(u =>
                (name is null || u.Name.Contains(name))
                && (model is null || u.Model.Contains(model))
                && (deviceType is null || u.DeviceType.Equals(deviceType))
                && (companyName is null || u.Company.Name.Contains(companyName))
            )
            .Skip(skip ?? 0)
            .Take(take ?? 20);
        return devices;
    }

    public Device GetDevice(int id)
    {
        return _deviceRepository.Get((x) => x.DeviceId == id);
    }

    public List<DeviceType> ListDeviceTypes()
    {
        return Enum.GetValues(typeof(DeviceType)).Cast<DeviceType>().ToList();
    }

    public List<string> ListValidators(string basePath = "")
    {
        var assemblyPath = basePath + "../DeviceValidators/bin/debug/net8.0/DeviceValidators.dll";
        var assemblyBytes = File.ReadAllBytes(assemblyPath);
        var assembly = Assembly.Load(assemblyBytes);

        var validators = assembly.GetTypes()
            .Where(t => typeof(IDeviceValidator).IsAssignableFrom(t) && !t.IsInterface)
            .Select(t => ((IDeviceValidator)Activator.CreateInstance(t)!).Modelo)
            .ToList();
        return validators;
    }
}
