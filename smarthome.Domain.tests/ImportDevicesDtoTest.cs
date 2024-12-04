using smarthome.Dtos;

namespace smarthome.Domain.tests;

[TestClass]
public class ImportDevicesDtoTest
{
    [TestMethod]
    public void ShouldCreateImportDevicesDto()
    {
        var importDevicesDto = new ImportDevicesDto
        {
            Data = "Test data",
            ImportStrategy = "Test strategy",
            CompanyId = 1
        };
        
        //Arrange
        Assert.AreEqual("Test data", importDevicesDto.Data);
        Assert.AreEqual("Test strategy", importDevicesDto.ImportStrategy);
        Assert.AreEqual(1, importDevicesDto.CompanyId);
    }

}