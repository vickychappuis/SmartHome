using smarthome.Domain;
using smarthome.Dtos;

namespace smarthome.Domain.tests;

[TestClass]
public class CompanyTests
{
    [TestMethod]
    public void Should_Create_Company_Without_DeviceValidator()
    {
        var company = new Company
        {
            CompanyId = 1,
            Name = "Company 1",
            LogotypeUrl = "https://company1.com/logo.png",
            Rut = 123456789,
            CompanyOwner = new User(),
            CompanyOwnerId = 1
        };

        Assert.IsNotNull(company);
        Assert.AreEqual(1, company.CompanyId);
        Assert.AreEqual("Company 1", company.Name);
        Assert.AreEqual("https://company1.com/logo.png", company.LogotypeUrl);
        Assert.AreEqual(123456789, company.Rut);
        Assert.IsNotNull(company.CompanyOwner);
        Assert.AreEqual(1, company.CompanyOwnerId); 
        Assert.IsNull(company.DeviceValidator);
    }

    [TestMethod]
    public void Should_Create_Company_With_DeviceValidator()
    {
        var company = new Company(new CompanyDto
        {
            Name = "Company 1",
            LogotypeUrl = "https://company1.com/logo.png",
            Rut = 123456789,
            DeviceValidator = "DeviceValidator"
        });

        Assert.IsNotNull(company);
        Assert.AreEqual("Company 1", company.Name);
        Assert.AreEqual("https://company1.com/logo.png", company.LogotypeUrl);
        Assert.AreEqual(123456789, company.Rut);
        Assert.AreEqual("DeviceValidator", company.DeviceValidator);
    }
}