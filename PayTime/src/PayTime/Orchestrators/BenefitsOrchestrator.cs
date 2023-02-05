using PayTime.Models;
using PayTime.Repositories;

namespace PayTime.Orchestrators;

public class BenefitsOrchestrator : IBenefitsOrchestrator
{
    private readonly IBenefitsRepository _benefitsRepository;

    public BenefitsOrchestrator(IBenefitsRepository benefitsRepository)
    {
        _benefitsRepository = benefitsRepository;
    }

    public async Task<Benefits> Create(CreateBenefitsRequest request)
    {
        var benefits = new Benefits(request.EmployeeName!, request.Dependents);
        benefits.CostPerPaycheck = CalculateCostPerPaycheck(benefits);
        
        return await _benefitsRepository.Create(benefits);
    }

    public async Task<Benefits?> Get(Guid benefitsId)
    {
        return await _benefitsRepository.Get(benefitsId);
    }

    public async Task<Benefits?> Update(Guid benefitsId, UpdateBenefitsRequest request)
    {
        var benefits = await _benefitsRepository.Get(benefitsId);

        if (benefits == null)
            return null;

        benefits.Dependents = request.Dependents ?? benefits.Dependents;
        benefits.EmployeeName = request.EmployeeName ?? benefits.EmployeeName;
        benefits.CostPerPaycheck = CalculateCostPerPaycheck(benefits);

        return await _benefitsRepository.Update(benefits);
    }
    
    private static decimal CalculateCostPerPaycheck(Benefits benefits)
    {
        var totalBenefitCost = (decimal) 500 * benefits.Dependents + 1000;

        if (benefits.EmployeeName.StartsWith("A", StringComparison.OrdinalIgnoreCase))
            totalBenefitCost *= .9m;

        return Math.Round(totalBenefitCost / benefits.PayCycleCount, 2, MidpointRounding.AwayFromZero);
    }
}