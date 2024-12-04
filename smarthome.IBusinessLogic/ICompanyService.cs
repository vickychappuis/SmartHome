using smarthome.Domain;
using smarthome.Dtos;

namespace smarthome.IBusinessLogic;

public interface ICompanyService
{
    public IEnumerable<Company> GetCompanies(
        int? skip = 0,
        int? take = 20,
        string? companyName = null,
        string? companyOwnerName = null,
        int? companyId = null
    );

    public Company AddCompany(CompanyDto company, int companyOwnerId);

    public Company? GetCompanyByUserId(int userId);
}