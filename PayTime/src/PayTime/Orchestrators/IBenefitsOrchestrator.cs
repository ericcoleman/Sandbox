using PayTime.Models;

namespace PayTime.Orchestrators;

public interface IBenefitsOrchestrator
{
    Task<Benefits> Create(CreateBenefitsRequest request);
    Task<Benefits?> Get(Guid benefitsId);
    Task<Benefits?> Update(Guid benefitsId, UpdateBenefitsRequest request);
}