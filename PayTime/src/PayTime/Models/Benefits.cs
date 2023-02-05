namespace PayTime.Models;

public class Benefits
{
    public Benefits(string employeeName, int dependents)
    {
        EmployeeName = employeeName;
        Dependents = dependents;
    }
    
    public Guid BenefitsId { get; set; } = Guid.NewGuid();
    public int Dependents { get; set; }
    public string EmployeeName { get; set; }
    public decimal CostPerPaycheck { get; set; }
    public int PayCycleCount { get; set; } = 26;
    public decimal Salary { get; set; } = 52000;
}