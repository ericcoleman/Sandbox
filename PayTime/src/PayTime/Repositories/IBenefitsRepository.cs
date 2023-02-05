using PayTime.Models;

namespace PayTime.Repositories;

public interface IBenefitsRepository
{
    Task<Benefits> Create(Benefits benefits);
    Task<Benefits?> Get(Guid benefitsId);
    Task<Benefits> Update(Benefits benefits);
}