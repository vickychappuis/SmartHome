using smarthome.Dtos;

namespace smarthome.IBusinessLogic;

public interface IAdminService
{
    public void AddAdmin(SignupDto dto);
    public void DeleteAdmin(string email);
    public void AddCompanyOwner(SignupDto dto);

}