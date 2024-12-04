using System.Linq.Expressions;
using Moq;
using smarthome.Domain;
using smarthome.Dtos;
using smarthome.IDataAccess;

namespace smarthome.BusinessLogic.tests;

[TestClass]
public class CompanyServiceTests
{
    private Mock<IGenericRepository<Company>> _companyRepositoryMock;
    private Mock<IGenericRepository<User>> _userRepositoryMock;

    private CompanyService _companyService;

    public CompanyServiceTests()
    {
        _companyRepositoryMock = new Mock<IGenericRepository<Company>>();
        _userRepositoryMock = new Mock<IGenericRepository<User>>();

        _companyService = new CompanyService(
            _companyRepositoryMock.Object,
            _userRepositoryMock.Object
        );
    }

    [TestMethod]
    public void GetCompanies_WithNoCompanies_ShouldReturnEmpty()
    {
        _companyRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<Company>());

        var companies = _companyService.GetCompanies();

        Assert.IsFalse(companies.Any());
    }

    [DataRow(1)]
    [DataRow(9)]
    [DataRow(16)]
    [DataRow(20)]
    [TestMethod]
    public void GetCompanies_WithLte20Companies_ShouldReturnAllCompanies(int count)
    {
        _companyRepositoryMock
            .Setup(repo => repo.GetAll(It.IsAny<Func<Company, bool>>(), It.IsAny<List<string>>()))
            .Returns(Enumerable.Repeat(new Company(), count));

        var companies = _companyService.GetCompanies();

        Assert.AreEqual(count, companies.Count());
    }

    [DataRow(21)]
    [DataRow(24)]
    [DataRow(32)]
    [DataRow(68)]
    [DataRow(126)]
    [TestMethod]
    public void GetCompanies_WithGt20Companies_ShouldReturn20Companies(int count)
    {
        _companyRepositoryMock
            .Setup(repo => repo.GetAll(It.IsAny<Func<Company, bool>>(), It.IsAny<List<string>>()))
            .Returns(Enumerable.Repeat(new Company(), count));

        var companies = _companyService.GetCompanies();

        Assert.AreEqual(20, companies.Count());
    }

    [DataRow(1)]
    [DataRow(5)]
    [DataRow(10)]
    [DataRow(20)]
    [TestMethod]
    public void GetCompanies_WithSkip_ShouldStartAtSkip(int skip)
    {
        var mockCompanies = Enumerable
            .Range(1, 40)
            .Select(i => new Company { CompanyId = i, Name = $"Company {i}" })
            .ToList();

        _companyRepositoryMock
            .Setup(repo => repo.GetAll(It.IsAny<Func<Company, bool>>(), It.IsAny<List<string>>()))
            .Returns(mockCompanies);

        var companies = _companyService.GetCompanies(skip: skip);

        Assert.AreEqual(20, companies.Count());
        Assert.AreEqual(skip + 1, companies.First().CompanyId);
    }

    [DataRow(1)]
    [DataRow(5)]
    [DataRow(10)]
    [DataRow(20)]
    [TestMethod]
    public void GetCompanies_WithTake_ShouldReturnTake(int take)
    {
        var mockCompanies = Enumerable
            .Range(1, 40)
            .Select(i => new Company { CompanyId = i, Name = $"Company {i}" })
            .ToList();

        _companyRepositoryMock
            .Setup(repo => repo.GetAll(It.IsAny<Func<Company, bool>>(), It.IsAny<List<string>>()))
            .Returns(mockCompanies);

        var companies = _companyService.GetCompanies(take: take);

        Assert.AreEqual(take, companies.Count());
    }

    [TestMethod]
    public void AddCompany_UserNotFound_ShouldThrow()
    {
        _userRepositoryMock
            .Setup(repo =>
                repo.Get(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<List<string>>())
            )
            .Returns((User?)null);

        var companyOwnerId = 1;
        
        var company = new CompanyDto();

        Assert.ThrowsException<ArgumentException>(() => _companyService.AddCompany(company, companyOwnerId));
    }

    [DataRow(UserRoles.HomeOwner)]
    [DataRow(UserRoles.Administrator)]
    [TestMethod]
    public void AddCompany_UserNotCompanyOwner_ShouldThrow(UserRoles role)
    {
        var user = new User() { UserId = 1, Role = role };

        _userRepositoryMock
            .Setup(repo =>
                repo.Get(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<List<string>>())
            )
            .Returns(user);

        var company = new CompanyDto() ;

        Assert.ThrowsException<Exception>(() => _companyService.AddCompany(company, user.UserId));
    }

    [TestMethod]
    public void AddCompany_UserAlreadyHasCompany_ShouldThrow()
    {
        var user = new User()
        {
            UserId = 1,
            Role = UserRoles.CompanyOwner,
            Company = new Company(),
        };

        _userRepositoryMock
            .Setup(repo =>
                repo.Get(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<List<string>>())
            )
            .Returns(user);

        var company = new CompanyDto();

        Assert.ThrowsException<Exception>(() => _companyService.AddCompany(company, user.UserId));
    }

    [TestMethod]
    public void AddCompany_ShouldAddCompany()
    {
        var user = new User() { UserId = 1, Role = UserRoles.CompanyOwner };

        _userRepositoryMock
            .Setup(repo =>
                repo.Get(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<List<string>>())
            )
            .Returns(user);

        var company = new CompanyDto();

        _companyService.AddCompany(company, user.UserId);
        
        _userRepositoryMock.Verify(repo => repo.Update(It.IsAny<User>()), Times.Once);
        _companyRepositoryMock.Verify(repo => repo.Insert(It.IsAny<Company>()), Times.Once);
    }
    
    [TestMethod]
    public void GetCompanyByUserId_ShouldReturnCompany()
    {
        var userId = 1;
        var _company = new Company() { CompanyOwner = new User(){UserId = 1},CompanyId = 1, Name = "Test Company" }; 

        _companyRepositoryMock
            .Setup(repo =>
                repo.Get(It.IsAny<Expression<Func<Company, bool>>>(), null)
            )
            .Returns(_company);

        var company = _companyService.GetCompanyByUserId(userId);

        Assert.IsNotNull(company);
    }
}
