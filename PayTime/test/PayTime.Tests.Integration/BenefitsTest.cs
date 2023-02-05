using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using PayTime.Models;

namespace PayTime.Tests.Integration;

public class BenefitsTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    
    public BenefitsTest(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();

    }
    [Fact]
    public async Task CanCreateAndUpdateBenefits()
    {
        var createRequest = new CreateBenefitsRequest { Dependents = 4, EmployeeName = "Ann" };

        var httpResult = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Post, "api/benefits")
        {
            Content = new StringContent(JsonConvert.SerializeObject(createRequest), Encoding.UTF8, "application/json")
        });

        Assert.Equal(HttpStatusCode.OK, httpResult.StatusCode);

        var createResult = JsonConvert.DeserializeObject<Benefits>(await httpResult.Content.ReadAsStringAsync());

        Assert.Equal("Ann", createResult.EmployeeName);
        Assert.Equal(103.85m, createResult.CostPerPaycheck);
        Assert.Equal(4, createResult.Dependents);

        var updateRequest = new UpdateBenefitsRequest { EmployeeName = "Jerry" };
        
        var updateHttpResult = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Patch, $"api/benefits/{createResult.BenefitsId}")
        {
            Content = new StringContent(JsonConvert.SerializeObject(updateRequest), Encoding.UTF8, "application/json")
        });

        Assert.Equal(HttpStatusCode.OK, updateHttpResult.StatusCode);

        var updateResult = JsonConvert.DeserializeObject<Benefits>(await updateHttpResult.Content.ReadAsStringAsync());

        Assert.Equal("Jerry", updateResult.EmployeeName);
        Assert.Equal(115.38m, updateResult.CostPerPaycheck);
        Assert.Equal(4, updateResult.Dependents);
    }
}