using smarthome.Dtos;
namespace smarthome.Domain.tests;

[TestClass]
public class SecurityCameraDtoTest
{
    [TestMethod]
    public void ShouldCreateSecurityCameraDto()
    {
        var securityCameraDto = new SecurityCameraDto()
        {
            CanDetectMotion = false,
            IsInterior = true,
            IsExterior = false,
            CanDetectPerson = true
        };

        // Assert
        Assert.AreEqual(false, securityCameraDto.CanDetectMotion);
        Assert.AreEqual(true, securityCameraDto.IsInterior);
        Assert.AreEqual(false, securityCameraDto.IsExterior);
        Assert.AreEqual(true, securityCameraDto.CanDetectPerson);
    }
}