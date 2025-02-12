using System.Net;
using System.Text;
using AutoFixture;
using DrugsMicroservice.Application.DTOs;
using DrugsMicroservice.Application.DTOs.Diseases;
using DrugsMicroservice.Application.DTOs.Substances;
using DrugsMicroservice.BusinessLogic.Models;
using DrugsMicroservice.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace DrugsMicroservice.Api.Tests.Integration;

public class DrugsApiEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly IServiceScope _scope;
    private readonly HttpClient _client;
    private readonly ApplicationDbContext _context;
    private readonly IFixture _fixture;


    public DrugsApiEndpointsTests(CustomWebApplicationFactory factory)
    {
        _scope = factory.Services.CreateScope();
        _context = _scope.ServiceProvider.GetService<ApplicationDbContext>();
        _fixture = new Fixture();
        _client = factory.CreateClient();
        _client.BaseAddress = new Uri("http://localhost:5105/api"); 
    }
    
    [Fact]
    public async Task AddDrug_WhenInvalidData_ReturnsBadRequest()
    {
        // Arrange
        var invalidDrugDto = new DrugCreateDTO { Name = "", Substances = new List<string>() }; 
        var contentDrug = new StringContent(JsonConvert.SerializeObject(invalidDrugDto), Encoding.UTF8, "application/json");
        // Act
        var response = await _client.PostAsync($"{_client.BaseAddress}/Drugs", contentDrug);
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode); // Sprawdzamy, czy zwrócono status 400
    }
    
    [Fact]
    public async Task AddDrug_WhenValidData_ReturnsCreatedDrug()
    {
        // Arrange
        
        var disease = _fixture.Create<DiseaseCreateDTO>();
        var contentDisease = new StringContent(JsonConvert.SerializeObject(disease), Encoding.UTF8, "application/json");
        var responseDisease = await _client.PostAsync($"{_client.BaseAddress}/Diseases", contentDisease);
  
        var newSubstance = _fixture.Create<SubstanceCreateDTO>();
        newSubstance.Diseases = new List<string> { disease.Name };
        var content = new StringContent(JsonConvert.SerializeObject(newSubstance), Encoding.UTF8, "application/json");
        var responseSubstance = await _client.PostAsync($"{_client.BaseAddress}/Substances", content);
        
        var newDrugDto = _fixture.Create<DrugCreateDTO>();
        newDrugDto.Substances = new List<string> { newSubstance.Name };
        var contentDrug = new StringContent(JsonConvert.SerializeObject(newDrugDto), Encoding.UTF8, "application/json");
        
        // Act
        var responseDrug = await _client.PostAsync($"{_client.BaseAddress}/Drugs", contentDrug);
        responseDrug.EnsureSuccessStatusCode(); 
        var createdDrug = JsonConvert.DeserializeObject<Drug>(await responseDrug.Content.ReadAsStringAsync());
        
        // Assert
        Assert.NotNull(createdDrug); 
        Assert.Equal(newDrugDto.Name, createdDrug.Name); 
    }
    
    [Fact]
    public async Task DeleteDrug_WhenDrugExists_ReturnsNoContent()
    {
        // Arrange
        var disease = _fixture.Create<DiseaseCreateDTO>();
        var contentDisease = new StringContent(JsonConvert.SerializeObject(disease), Encoding.UTF8, "application/json");
        var responseDisease = await _client.PostAsync($"{_client.BaseAddress}/Diseases", contentDisease);
        responseDisease.EnsureSuccessStatusCode(); 

        var newSubstance = _fixture.Create<SubstanceCreateDTO>();
        newSubstance.Diseases = new List<string> { disease.Name };
        var contentSubstance = new StringContent(JsonConvert.SerializeObject(newSubstance), Encoding.UTF8, "application/json");
        var responseSubstance = await _client.PostAsync($"{_client.BaseAddress}/Substances", contentSubstance);
        responseSubstance.EnsureSuccessStatusCode(); 

        var newDrugDto = _fixture.Create<DrugCreateDTO>();
        newDrugDto.Substances = new List<string> { newSubstance.Name };
        var contentDrug = new StringContent(JsonConvert.SerializeObject(newDrugDto), Encoding.UTF8, "application/json");

        // Act 
        var responseDrug = await _client.PostAsync($"{_client.BaseAddress}/Drugs", contentDrug);
        responseDrug.EnsureSuccessStatusCode(); 
        var createdDrug = JsonConvert.DeserializeObject<Drug>(await responseDrug.Content.ReadAsStringAsync());

  
        var responseDelete = await _client.DeleteAsync($"{_client.BaseAddress}/Drugs/{createdDrug.Id}");
        responseDelete.EnsureSuccessStatusCode(); // Upewnij się, że odpowiedź ma status 204 (No Content)
        
        var responseGetDeletedDrug = await _client.GetAsync($"{_client.BaseAddress}/Drugs/{createdDrug.Id}");
    
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, responseGetDeletedDrug.StatusCode); // Sprawdzamy, czy zwrócono status 404 (Not Found)
    }

    [Fact]
public async Task UpdateDrug_WhenDrugExists_ReturnsUpdatedDrug()
{
    // Arrange
    var disease = _fixture.Create<DiseaseCreateDTO>();
    var contentDisease = new StringContent(JsonConvert.SerializeObject(disease), Encoding.UTF8, "application/json");
    var responseDisease = await _client.PostAsync($"{_client.BaseAddress}/Diseases", contentDisease);
    responseDisease.EnsureSuccessStatusCode(); // Upewnij się, że odpowiedź jest poprawna

    var newSubstance = _fixture.Create<SubstanceCreateDTO>();
    newSubstance.Diseases = new List<string> { disease.Name };
    var contentSubstance = new StringContent(JsonConvert.SerializeObject(newSubstance), Encoding.UTF8, "application/json");
    var responseSubstance = await _client.PostAsync($"{_client.BaseAddress}/Substances", contentSubstance);
    responseSubstance.EnsureSuccessStatusCode(); // Upewnij się, że odpowiedź jest poprawna

    var newDrugDto = _fixture.Create<DrugCreateDTO>();
    newDrugDto.Substances = new List<string> { newSubstance.Name };
    var contentDrug = new StringContent(JsonConvert.SerializeObject(newDrugDto), Encoding.UTF8, "application/json");

    // Act
    var responseDrug = await _client.PostAsync($"{_client.BaseAddress}/Drugs", contentDrug);
    responseDrug.EnsureSuccessStatusCode();
    var createdDrug = JsonConvert.DeserializeObject<Drug>(await responseDrug.Content.ReadAsStringAsync());
    
    var updatedDrugDto = new DrugUpdateDTO()
    {
        Name = "Updated Drug Name", 
        Manufacturer = "Updated Manufacturer",
        Price = 1234,
        Substances= new List<string> { newSubstance.Name }
    };
    var contentUpdatedDrug = new StringContent(JsonConvert.SerializeObject(updatedDrugDto), Encoding.UTF8, "application/json");
    
    var responseUpdate = await _client.PutAsync($"{_client.BaseAddress}/Drugs/{createdDrug.Id}", contentUpdatedDrug);
    responseUpdate.EnsureSuccessStatusCode(); // Upewnij się, że odpowiedź ma status 200 (OK)
    var updatedDrug = JsonConvert.DeserializeObject<Drug>(await responseUpdate.Content.ReadAsStringAsync());

    // Assert
    Assert.NotNull(updatedDrug); 
    Assert.Equal(updatedDrugDto.Name, updatedDrug.Name); 
}

}