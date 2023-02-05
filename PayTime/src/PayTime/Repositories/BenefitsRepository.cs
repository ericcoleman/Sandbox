using PayTime.Models;

namespace PayTime.Repositories;

public class BenefitsRepository : IBenefitsRepository
{
    // In the real world this would be some kind of shared datasource,
    // Going to keep the Task interface bc that's what it would really be
    private readonly List<Benefits> _benefitsCollection;

    public BenefitsRepository()
    {
        _benefitsCollection = new List<Benefits>();
    }
    
    public Task<Benefits> Create(Benefits benefits)
    {
        _benefitsCollection.Add(benefits);
        return Task.FromResult(benefits);
    }

    public Task<Benefits?> Get(Guid benefitsId)
    {
        return Task.FromResult(_benefitsCollection.FirstOrDefault(x => x.BenefitsId == benefitsId));
    }

    public Task<Benefits> Update(Benefits benefits)
    {
        _benefitsCollection.RemoveAll(x => x.BenefitsId == benefits.BenefitsId);
        _benefitsCollection.Add(benefits);
        return Task.FromResult(benefits);
    }
}