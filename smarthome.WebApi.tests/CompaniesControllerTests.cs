using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Moq;
using smarthome.Domain;
using smarthome.Dtos;
using smarthome.WebApi.Controllers;
using System;
using System.Collections.Generic;
using smarthome.IBusinessLogic;

namespace smarthome.WebApi.tests;

[TestClass]
public class CompaniesControllerTests
{
    private Mock<ICompanyService> _companyServiceMock;
    private CompaniesController _companiesController;
    private DefaultHttpContext _httpContext;

    [TestInitialize]
    public void Setup()
    {
        _companyServiceMock = new Mock<ICompanyService>();
        _companiesController = new CompaniesController(_companyServiceMock.Object);
        _httpContext = new DefaultHttpContext();
        _companiesController.ControllerContext = new ControllerContext()
        {
            HttpContext = _httpContext
        };
    }

    [TestMethod]
    public void GetCompanies_ShouldReturnOk()
    {
        _companyServiceMock.Setup(service => service.GetCompanies(null, null, null, null, null))
            .Returns(new List<Company>(){new Company()});

        var result = _companiesController.Get(null, null, null, null, null);
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void PostCompany_InvalidData_ShouldReturnBadRequest()
    {
        var session = new Session { UserId = 1, Token = "invalid-token" };
        _httpContext.Items["Session"] = session;
        _companyServiceMock.Setup(service => service.AddCompany(It.IsAny<CompanyDto>(), session.UserId)).Throws(new ArgumentException("Missing data in Company constructor"));
        var result = _companiesController.Post(new CompanyDto());
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public void PostCompany_ValidData_ShouldReturnOk()
    {
        var session = new Session { UserId = 1, Token = "valid-token" };
        _httpContext.Items["Session"] = session;
        _companyServiceMock.Setup(service => service.AddCompany(It.IsAny<CompanyDto>(), session.UserId)).Returns(new Company());
        var result = _companiesController.Post(new CompanyDto());
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void MyCompany_ShouldReturnOk()
    {
        var session = new Session { UserId = 1, Token = "new-token" };
        _httpContext.Items["Session"] = session;
        _companyServiceMock.Setup(service => service.GetCompanyByUserId(session.UserId)).Returns(new Company());
        var result = _companiesController.MyCompany();
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void MyCompany_CompanyNotFound()
    {
        var session = new Session { UserId = 1, Token = "new-token" };
        _httpContext.Items["Session"] = session;
        _companyServiceMock.Setup(service => service.GetCompanyByUserId(session.UserId)).Returns((Company)null);
        var result = _companiesController.MyCompany();
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
    }
}