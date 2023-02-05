using NSubstitute;
using PayTime.Models;
using PayTime.Orchestrators;
using PayTime.Repositories;

namespace PayTime.Tests.Orchestrators;

public class BenefitsOrchestratorTests
{
    private readonly IBenefitsOrchestrator _orchestrator;
    private readonly IBenefitsRepository _repository;

    public BenefitsOrchestratorTests()
    {
        _repository = Substitute.For<IBenefitsRepository>();
        _orchestrator = new BenefitsOrchestrator(_repository);
    }
    
    [Theory]
    [InlineData("Ann", 4, 103.85)]
    [InlineData("Jerry", 4, 115.38)]
    [InlineData("Tom", 0, 38.46)]
    public async Task CanCreateBenefits(string name, int dependents, decimal expectedCostPerPaycheck)
    {
        _repository.Create(Arg.Is<Benefits>(x => x.EmployeeName == name && x.Dependents == dependents))
            .Returns(x => x.Arg<Benefits>());
        
        var result = await _orchestrator.Create(new CreateBenefitsRequest { EmployeeName = name, Dependents = dependents });
        
        Assert.Equal(name, result.EmployeeName);
        Assert.Equal(dependents, result.Dependents);
        Assert.Equal(expectedCostPerPaycheck, result.CostPerPaycheck);
    }

    [Fact]
    public async Task CanGetBenefits()
    {
        var benefitId = Guid.NewGuid();
        _repository.Get(benefitId).Returns(new Benefits("A", 2));

        var result = await _orchestrator.Get(benefitId);

        Assert.NotNull(result);
        Assert.Equal("A", result!.EmployeeName);
        Assert.Equal(2, result.Dependents);
    }

    [Fact]
    public async Task CanUpdateBenefits()
    {
        var benefitId = Guid.NewGuid();
        _repository.Get(benefitId).Returns(new Benefits("Old", 2) {BenefitsId = benefitId});
        _repository.Update(Arg.Is<Benefits>(x => x.BenefitsId == benefitId))
            .Returns(x => x.Arg<Benefits>());

        var result = await _orchestrator.Update(benefitId, new UpdateBenefitsRequest {EmployeeName = "New"});
        
        Assert.NotNull(result);
        Assert.Equal("New", result!.EmployeeName);
        Assert.Equal(2, result.Dependents);
        Assert.Equal(76.92m, result.CostPerPaycheck);
    }
}