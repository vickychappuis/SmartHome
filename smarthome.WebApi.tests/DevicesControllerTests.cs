using Microsoft.AspNetCore.Mvc;
using Moq;
using smarthome.Domain;
using smarthome.Dtos;
using smarthome.IBusinessLogic;
using smarthome.WebApi.Controllers;

namespace smarthome.WebApi.tests;

[TestClass]
public class DevicesControllerTests
{
    private Mock<IDeviceService> _deviceServiceMock;
    private DevicesController _deviceController;
    
    [TestInitialize]
    public void Setup()
    {
        _deviceServiceMock = new Mock<IDeviceService>();
        _deviceController = new DevicesController(_deviceServiceMock.Object);
    }
    
    [TestMethod]
    public void GetDevices_ShouldReturnOk()
    {
        _deviceServiceMock.Setup(service => service.GetDevices(null,null,null,null,null,null)).Returns(new List<Device>());
        var result = _deviceController.Get(null,null,null,null,null,null);
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }
    
    [TestMethod]
    public void GetDeviceTypes_ShouldReturnOk()
    {
        _deviceServiceMock.Setup(service => service.ListDeviceTypes()).Returns(new List<DeviceType>());
        var result = _deviceController.GetDeviceTypes();
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }
    
    [TestMethod]
    public void PostDevice_InvalidData_ShouldReturnBadRequest()
    {
        _deviceServiceMock.Setup(service => service.AddDevice(It.IsAny<DeviceDto>(), It.IsAny<DeviceType>())).Throws(new ArgumentException("Missing data in Device constructor")); 
        var result = _deviceController.Post(new DeviceDto(), (char)DeviceType.WindowSensor); 
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }
    
    [TestMethod]
    public void PostDevice_ValidData_ShouldReturnOk()
    {
        _deviceServiceMock.Setup(service => service.AddDevice(It.IsAny<DeviceDto>(), It.IsAny<DeviceType>()));
        var result = _deviceController.Post(new DeviceDto(), (char)DeviceType.WindowSensor);
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }
    
    [TestMethod]
    public void Get_ShouldReturnOk()
    {
        _deviceServiceMock.Setup(service => service.GetDevice(It.IsAny<int>())).Returns(new SecurityCamera());
        var result = _deviceController.Get(1);
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void Get_ShouldReturnNotFound()
    {
        _deviceServiceMock.Setup(service => service.GetDevice(It.IsAny<int>())).Returns((Device)null);
        var result = _deviceController.Get(1);
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
    }
    
}