using smarthome.Dtos;
namespace smarthome.Domain.tests;

[TestClass]
public class CompanyOwnerDtoTests
{
    [TestMethod]
    public void ShouldCreateCompanyOwnerDto()
    {
        var companyOwner = new CompanyOwnerDto()
        {
            Email = "a@gmail.com",
            Name = "john",
        };
        
        // Assert
        Assert.AreEqual("john", companyOwner.Name);
        Assert.AreEqual("a@gmail.com", companyOwner.Email);
    }
}