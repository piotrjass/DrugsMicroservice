using System.Text;
using AutoFixture;
using DrugsMicroservice.Application.DTOs.Diseases;
using DrugsMicroservice.BusinessLogic.Models;
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
        var response = await _client.PostAsync($"{_client.BaseAddress}/Diseases", content);
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
        var createResponse = await _client.PostAsync($"{_client.BaseAddress}/Diseases", content);
        createResponse.EnsureSuccessStatusCode();

        var dbDiseaseBeforeUpdate = await _context.Diseases
            .FirstOrDefaultAsync(d => d.Name == disease.Name);

        Assert.NotNull(dbDiseaseBeforeUpdate);

   
        dbDiseaseBeforeUpdate.Name = "Updated Name";  
        var updateContent = new StringContent(JsonConvert.SerializeObject(dbDiseaseBeforeUpdate), Encoding.UTF8, "application/json");

        var updateResponse = await _client.PutAsync($"{_client.BaseAddress}/Diseases/{dbDiseaseBeforeUpdate.Id}", updateContent);

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
        var createResponse = await _client.PostAsync($"{_client.BaseAddress}/Diseases", content);
        createResponse.EnsureSuccessStatusCode();

        var dbDiseaseBeforeDelete = await _context.Diseases
            .FirstOrDefaultAsync(d => d.Name == disease.Name);
    
        Assert.NotNull(dbDiseaseBeforeDelete); // Ensure it exists before deletion

        // Now delete the disease
        var deleteResponse = await _client.DeleteAsync($"{_client.BaseAddress}/Diseases/{dbDiseaseBeforeDelete.Id}");

        // Assert: Ensure the delete was successful
        deleteResponse.EnsureSuccessStatusCode(); // Ensure 200-299 status code range

        var dbDiseaseAfterDelete = await _context.Diseases
            .FirstOrDefaultAsync(d => d.Id == dbDiseaseBeforeDelete.Id);

        Assert.Null(dbDiseaseAfterDelete); // Ensure the disease was deleted from DB
    }

    [Fact]
    public async Task GetAllDiseases_ReturnsAllDiseases()
    {
        // Arrange
        var disease1 = _fixture.Create<DiseaseCreateDTO>();
        var disease2 = _fixture.Create<DiseaseCreateDTO>();

        var content1 = new StringContent(JsonConvert.SerializeObject(disease1), Encoding.UTF8, "application/json");
        var content2 = new StringContent(JsonConvert.SerializeObject(disease2), Encoding.UTF8, "application/json");

        // Act: Create two diseases
        await _client.PostAsync($"{_client.BaseAddress}/Diseases", content1);
        await _client.PostAsync($"{_client.BaseAddress}/Diseases", content2);

        // Act: Fetch all diseases
        var response = await _client.GetAsync($"{_client.BaseAddress}/Diseases");

        // Assert: 
        response.EnsureSuccessStatusCode(); // Ensure 200-299 status code range
        var responseContent = await response.Content.ReadAsStringAsync();
        var diseases = JsonConvert.DeserializeObject<List<Disease>>(responseContent);

        Assert.NotNull(diseases);
        Assert.True(diseases.Count >= 2); 
    }
    
    [Fact]
    public async Task GetDiseaseById_ReturnsCorrectDisease()
    {
        // Arrange
        var disease = _fixture.Create<DiseaseCreateDTO>();
        var content = new StringContent(JsonConvert.SerializeObject(disease), Encoding.UTF8, "application/json");

        // Act: Create a disease
        var createResponse = await _client.PostAsync($"{_client.BaseAddress}/Diseases", content);
        createResponse.EnsureSuccessStatusCode();

        var dbDisease = await _context.Diseases
            .FirstOrDefaultAsync(d => d.Name == disease.Name);

        // Act: Get the disease by ID
        var response = await _client.GetAsync($"{_client.BaseAddress}/Diseases/{dbDisease.Id}");

        // Assert: Ensure the response contains the correct disease
        response.EnsureSuccessStatusCode(); // Ensure 200-299 status code range
        var responseContent = await response.Content.ReadAsStringAsync();
        var fetchedDisease = JsonConvert.DeserializeObject<Disease>(responseContent);

        Assert.NotNull(fetchedDisease);
        Assert.Equal(disease.Name, fetchedDisease.Name);
    }
    
    [Fact]
public async Task GetDiseaseById_WhenDiseaseDoesNotExist_ReturnsNotFound()
{
    // Act: Attempt to fetch a non-existent disease by ID
    var response = await _client.GetAsync($"{_client.BaseAddress}/Diseases/{Guid.NewGuid()}");

    // Assert: Ensure the response status is 404 Not Found
    Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
}

[Fact]
public async Task GetDiseaseByName_ReturnsCorrectDisease()
{
    // Arrange
    var disease = _fixture.Create<DiseaseCreateDTO>();
    var content = new StringContent(JsonConvert.SerializeObject(disease), Encoding.UTF8, "application/json");

    // Act: Create a disease
    var createResponse = await _client.PostAsync($"{_client.BaseAddress}/Diseases", content);
    createResponse.EnsureSuccessStatusCode();

    var dbDisease = await _context.Diseases
        .FirstOrDefaultAsync(d => d.Name == disease.Name);

    // Act: Get the disease by name
    var response = await _client.GetAsync($"{_client.BaseAddress}/Diseases/search/{dbDisease.Name}");

    // Assert: Ensure the response contains the correct disease
    response.EnsureSuccessStatusCode(); // Ensure 200-299 status code range
    var responseContent = await response.Content.ReadAsStringAsync();
    var fetchedDisease = JsonConvert.DeserializeObject<Disease>(responseContent);

    Assert.NotNull(fetchedDisease);
    Assert.Equal(disease.Name, fetchedDisease.Name);
}

[Fact]
public async Task GetDiseaseByName_WhenDiseaseDoesNotExist_ReturnsNotFound()
{
    // Act: Attempt to fetch a non-existent disease by name
    var response = await _client.GetAsync($"{_client.BaseAddress}/Diseases/search/NonExistentDisease");

    // Assert: Ensure the response status is 404 Not Found
    Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
}

[Fact]
public async Task CreateDisease_WhenDataIsInvalid_ReturnsBadRequest()
{
    // Arrange: Invalid data (e.g. missing required fields)
    var invalidDisease = new DiseaseCreateDTO(); // Empty DTO
    var content = new StringContent(JsonConvert.SerializeObject(invalidDisease), Encoding.UTF8, "application/json");

    // Act: Attempt to create an invalid disease
    var response = await _client.PostAsync($"{_client.BaseAddress}/Diseases", content);

    // Assert: Ensure the response status is 400 Bad Request
    Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
}

[Fact]
public async Task UpdateDisease_WhenDataIsInvalid_ReturnsBadRequest()
{
    // Arrange: Create a valid disease
    var disease = _fixture.Create<DiseaseCreateDTO>();
    var content = new StringContent(JsonConvert.SerializeObject(disease), Encoding.UTF8, "application/json");
    var createResponse = await _client.PostAsync($"{_client.BaseAddress}/Diseases", content);
    createResponse.EnsureSuccessStatusCode();

    var dbDiseaseBeforeUpdate = await _context.Diseases
        .FirstOrDefaultAsync(d => d.Name == disease.Name);

    Assert.NotNull(dbDiseaseBeforeUpdate);

    // Invalid update data 
    dbDiseaseBeforeUpdate.Name = string.Empty; // Invalid name (empty string)
    var updateContent = new StringContent(JsonConvert.SerializeObject(dbDiseaseBeforeUpdate), Encoding.UTF8, "application/json");

    // Act: Attempt to update with invalid data
    var updateResponse = await _client.PutAsync($"{_client.BaseAddress}/Diseases/{dbDiseaseBeforeUpdate.Id}", updateContent);

    // Assert: Ensure the response status is 400 Bad Request
    Assert.Equal(System.Net.HttpStatusCode.BadRequest, updateResponse.StatusCode);
}

[Fact]
public async Task DeleteDisease_WhenDiseaseDoesNotExist_ReturnsNotFound()
{
    // Act: Attempt to delete a non-existent disease
    var response = await _client.DeleteAsync($"{_client.BaseAddress}/Diseases/{Guid.NewGuid()}");

    // Assert: Ensure the response status is 404 Not Found
    Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
}
    
}