using Microsoft.AspNetCore.Mvc;
using PayTime.Models;
using PayTime.Orchestrators;

namespace PayTime.Controllers;

[Route("api/benefits")]
public class BenefitsController : ControllerBase
{
    private readonly IBenefitsOrchestrator _benefitsOrchestrator;

    public BenefitsController(IBenefitsOrchestrator benefitsOrchestrator)
    {
        _benefitsOrchestrator = benefitsOrchestrator;
    }

    [HttpPost]
    public async Task<ActionResult<Benefits>> CreateBenefits([FromBody] CreateBenefitsRequest request)
    {
        // Would normally validate in FluentValidation or something
        if (request.Dependents < 0)
            return BadRequest(new ErrorResponse("Cannot have negative dependents"));
        if (request.Dependents > 102)
            return BadRequest(new ErrorResponse("Cannot afford more than 102 dependants"));
        if (string.IsNullOrEmpty(request.EmployeeName))
            return BadRequest(new ErrorResponse("Must provide employee name"));

        return await _benefitsOrchestrator.Create(request);
    }

    [HttpGet("{benefitsId:Guid}")]
    public async Task<ActionResult<Benefits>> Get(Guid benefitsId)
    {
        var benefits = await _benefitsOrchestrator.Get(benefitsId);

        return benefits == null ? NotFound() : benefits;
    }

    [HttpPatch("{benefitsId:Guid}")]
    public async Task<ActionResult<Benefits>> Update(Guid benefitsId, [FromBody] UpdateBenefitsRequest request)
    {
        var benefits = await _benefitsOrchestrator.Update(benefitsId, request);

        return benefits == null ? NotFound() : benefits;
    }
}