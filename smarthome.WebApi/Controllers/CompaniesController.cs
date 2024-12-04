using Microsoft.AspNetCore.Mvc;
using smarthome.BusinessLogic;
using smarthome.Domain;
using smarthome.Dtos;
using smarthome.IBusinessLogic;

namespace smarthome.WebApi.Controllers
{
    [Route("/api/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        [RoleAuthorizationFilter()]
        public IActionResult Get(
            [FromQuery] int? skip,
            [FromQuery] int? take,
            [FromQuery] string? companyName,
            [FromQuery] string? companyOwnerName,
            [FromQuery] int? companyId
        )
        {
            var companies = _companyService.GetCompanies(skip, take, companyName, companyOwnerName, companyId);
            return Ok(companies);
        }

        [HttpPost]
        [RoleAuthorizationFilter(UserRoles.CompanyOwner)]
        public IActionResult Post([FromBody] CompanyDto company)
        {
            Session session = (Session)HttpContext.Items["Session"]!;
            try
            {
                var companyCreated = _companyService.AddCompany(company, session.UserId);
                return Ok(companyCreated);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("my")]
        [RoleAuthorizationFilter(UserRoles.CompanyOwner)]
        public IActionResult MyCompany()
        {
            Session session = (Session)HttpContext.Items["Session"]!;

            var company = _companyService.GetCompanyByUserId(session.UserId);
            if (company == null)
            {
                return NotFound();
            }

            return Ok(company);
        }

    }
}
