using smarthome.Domain;
using smarthome.Dtos;
using smarthome.IBusinessLogic;
using smarthome.IDataAccess;

namespace smarthome.BusinessLogic;

public class CompanyService : ICompanyService
{
    private readonly IGenericRepository<Company> _companyRepository;
    private readonly IGenericRepository<User> _userRepository;

    public CompanyService(
        IGenericRepository<Company> companyRepository,
        IGenericRepository<User> userRepository
    )
    {
        _companyRepository = companyRepository;
        _userRepository = userRepository;
    }

    public IEnumerable<Company> GetCompanies(
        int? skip = 0,
        int? take = 20,
        string? companyName = null,
        string? companyOwnerName = null,
        int? companyId = null
    )
    {
        var companies = _companyRepository
            .GetAll(
                c =>
                    (companyName is null || c.Name.Contains(companyName))
                    && (companyOwnerName is null || c.CompanyOwner.FirstName.Contains(companyOwnerName))
                    && (companyId is null || c.CompanyId == companyId),
                ["CompanyOwner"]
            )
            .Skip(skip ?? 0)
            .Take(take ?? 20);

        return companies;
    }

    public Company AddCompany(CompanyDto company, int companyOwnerId)
    {
        User? owner = _userRepository.Get(u => u.UserId == companyOwnerId, ["Company"]);

        if (owner is null)
        {
            throw new ArgumentException("User not found");
        }

        if (owner.Role != UserRoles.CompanyOwner)
        {
            throw new Exception("User not company owner");
        }

        if (owner.Company is not null)
        {
            throw new Exception("User already has a company");
        }

        var newCompany = new Company(company);

        newCompany.CompanyOwner = owner;
        owner.Company = newCompany;

        _companyRepository.Insert(newCompany);
        _userRepository.Update(owner);

        return newCompany;
    }

    public Company? GetCompanyByUserId(int userId)
    {
        Console.WriteLine("CompanyService");
        Console.WriteLine(userId);

        return _companyRepository.Get(c => c.CompanyOwner.UserId == userId);
    }

}
