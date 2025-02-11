using System.Text;
using AutoFixture;
using DrugsMicroservice.Application.DTOs.Diseases;
using DrugsMicroservice.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace DrugsMicroservice.Api.Tests.Integration;
public class DiseaseApiEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly IServiceScope _scope;
    private readonly HttpClient _client;
    private readonly ApplicationDbContext _context;
    private readonly IFixture _fixture;
    public DiseaseApiEndpointsTests(CustomWebApplicationFactory factory)
    {
        _scope = factory.Services.CreateScope();
        _context = _scope.ServiceProvider.GetService<ApplicationDbContext>();
        _fixture = new Fixture();
        _client = factory.CreateClient();
        _client.BaseAddress = new Uri("http://localhost:5105/api"); 
    }
    [Fact]
    public async Task CreateDisease_WhenDataIsValid()
    {
        // Arrange
        var disease = _fixture.Create<DiseaseCreateDTO>();
        var content = new StringContent(JsonConvert.SerializeObject(disease), Encoding.UTF8, "application/json");
        // Act
        var response = await _client.PostAsync($"{_client.BaseAddress}/Diseases/AddDisease", content);
        // Assert
        response.EnsureSuccessStatusCode(); // Ensure 200-299 status code range
        var dbDisease = await _context.Diseases
            .FirstOrDefaultAsync(d => d.Name == disease.Name); 
        Assert.NotNull(dbDisease); 
        Assert.Equal(disease.Name, dbDisease.Name); 
    }
    
    [Fact]
    public async Task UpdateDisease_WhenDataIsValid()
    {
        // Arrange
        var disease = _fixture.Create<DiseaseCreateDTO>();
        var content = new StringContent(JsonConvert.SerializeObject(disease), Encoding.UTF8, "application/json");

        // Act
        var createResponse = await _client.PostAsync($"{_client.BaseAddress}/Diseases/AddDisease", content);
        createResponse.EnsureSuccessStatusCode();

        var dbDiseaseBeforeUpdate = await _context.Diseases
            .FirstOrDefaultAsync(d => d.Name == disease.Name);

        Assert.NotNull(dbDiseaseBeforeUpdate);

   
        dbDiseaseBeforeUpdate.Name = "Updated Name";  
        var updateContent = new StringContent(JsonConvert.SerializeObject(dbDiseaseBeforeUpdate), Encoding.UTF8, "application/json");

        var updateResponse = await _client.PutAsync($"{_client.BaseAddress}/Diseases/UpdateDisease/{dbDiseaseBeforeUpdate.Id}", updateContent);

        // Assert:
        updateResponse.EnsureSuccessStatusCode(); // Ensure 200-299 status code range

        var dbDiseaseAfterUpdate = await _context.Diseases
            .FirstOrDefaultAsync(d => d.Id == dbDiseaseBeforeUpdate.Id);
    
        Assert.NotNull(dbDiseaseAfterUpdate); // Ensure the disease is still in the DB
        Assert.Equal("Updated Name", dbDiseaseAfterUpdate.Name); // Ensure the name was updated
    }

    [Fact]
    public async Task DeleteDisease_WhenDataIsValid()
    {
        // Arrange
        var disease = _fixture.Create<DiseaseCreateDTO>();
        var content = new StringContent(JsonConvert.SerializeObject(disease), Encoding.UTF8, "application/json");

        // Act: First, create the disease
        var createResponse = await _client.PostAsync($"{_client.BaseAddress}/Diseases/AddDisease", content);
        createResponse.EnsureSuccessStatusCode();

        var dbDiseaseBeforeDelete = await _context.Diseases
            .FirstOrDefaultAsync(d => d.Name == disease.Name);
    
        Assert.NotNull(dbDiseaseBeforeDelete); // Ensure it exists before deletion

        // Now delete the disease
        var deleteResponse = await _client.DeleteAsync($"{_client.BaseAddress}/Diseases/DeleteDisease/{dbDiseaseBeforeDelete.Id}");

        // Assert: Ensure the delete was successful
        deleteResponse.EnsureSuccessStatusCode(); // Ensure 200-299 status code range

        var dbDiseaseAfterDelete = await _context.Diseases
            .FirstOrDefaultAsync(d => d.Id == dbDiseaseBeforeDelete.Id);

        Assert.Null(dbDiseaseAfterDelete); // Ensure the disease was deleted from DB
    }

    
    
}