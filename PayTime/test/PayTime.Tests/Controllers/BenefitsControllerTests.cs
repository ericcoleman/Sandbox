using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using PayTime.Controllers;
using PayTime.Models;
using PayTime.Orchestrators;

namespace PayTime.Tests.Controllers;

public class BenefitsControllerTests
{
    private readonly BenefitsController _controller;
    private readonly IBenefitsOrchestrator _benefitsOrchestrator;

    public BenefitsControllerTests()
    {
        _benefitsOrchestrator = Substitute.For<IBenefitsOrchestrator>();
        _controller = new BenefitsController(_benefitsOrchestrator);
    }
    
    [Fact]
    public async Task CanCreateBenefits()
    {
        var request = new CreateBenefitsRequest { Dependents = 2, EmployeeName = "A" };
        _benefitsOrchestrator.Create(request)
            .Returns(new Benefits("A", 2));

        var result = await _controller.CreateBenefits(request);
        
        Assert.NotNull(result.Value);
        Assert.Equal("A", result.Value!.EmployeeName);
        Assert.Equal(2, result.Value.Dependents);
    }

    [Theory]
    [InlineData(-2, "name")]
    [InlineData(3, "")]
    public async Task InvalidDataReturnsBadRequest(int dependents, string employeeName)
    {
        var request = new CreateBenefitsRequest { Dependents = dependents, EmployeeName = employeeName };

        var result = await _controller.CreateBenefits(request);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        
        Assert.IsType<ErrorResponse>(badRequestResult.Value);
    }

    [Fact]
    public async Task CanGetBenefits()
    {
        var benefitId = Guid.NewGuid();
        _benefitsOrchestrator.Get(benefitId).Returns(new Benefits("Steve", 1));

        var result = await _controller.Get(benefitId);
        
        Assert.NotNull(result.Value);
        Assert.Equal("Steve", result.Value!.EmployeeName);
        Assert.Equal(1, result.Value.Dependents);
    }

    [Fact]
    public async Task CanUpdateBenefits()
    {
        var benefitId = Guid.NewGuid();
        _benefitsOrchestrator.Update(benefitId, Arg.Any<UpdateBenefitsRequest>())
            .Returns(new Benefits("Perry", 3));

        var result = await _controller.Update(benefitId, new UpdateBenefitsRequest());

        Assert.NotNull(result.Value);
        Assert.Equal("Perry", result.Value!.EmployeeName);
        Assert.Equal(3, result.Value.Dependents);
    }
}