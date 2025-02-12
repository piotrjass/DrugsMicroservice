using System.Net;
using System.Text;
using AutoFixture;
using DrugsMicroservice.Application.DTOs.Diseases;
using DrugsMicroservice.Application.DTOs.Substances;
using DrugsMicroservice.BusinessLogic.Models;
using DrugsMicroservice.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace DrugsMicroservice.Api.Tests.Integration;

public class SubstancesApiEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly IServiceScope _scope;
    private readonly HttpClient _client;
    private readonly ApplicationDbContext _context;
    private readonly IFixture _fixture;


    public SubstancesApiEndpointsTests(CustomWebApplicationFactory factory)
    {
        _scope = factory.Services.CreateScope();
        _context = _scope.ServiceProvider.GetService<ApplicationDbContext>();
        _fixture = new Fixture();
        _client = factory.CreateClient();
        _client.BaseAddress = new Uri("http://localhost:5105/api"); 
    }
    
    [Fact]
    public async Task AddSubstance_WhenDataIsValid()
    {
        // Arrange
     
        var disease = _fixture.Create<DiseaseCreateDTO>();
        var contentDisease = new StringContent(JsonConvert.SerializeObject(disease), Encoding.UTF8, "application/json");
        var responseDisease = await _client.PostAsync($"{_client.BaseAddress}/Diseases", contentDisease);
  
        var newSubstance = _fixture.Create<SubstanceCreateDTO>();
        newSubstance.Diseases = new List<string> { disease.Name };
        var content = new StringContent(JsonConvert.SerializeObject(newSubstance), Encoding.UTF8, "application/json");
        // Act
        var response = await _client.PostAsync($"{_client.BaseAddress}/Substances", content);
        // Assert
        response.EnsureSuccessStatusCode(); // Ensure 200-299 status code range
        var dbSubstance = await _context.Substances
            .FirstOrDefaultAsync(s => s.SubstanceName == newSubstance.Name); 
    
        Assert.NotNull(dbSubstance); 
        Assert.Equal(newSubstance.Name, dbSubstance.SubstanceName); 
        Assert.Equal(newSubstance.Dosage, dbSubstance.Dosage); 
    }
    
    [Fact]
    public async Task GetAllSubstances_WhenSubstancesExist()
    {
        // Arrange
        var substance1 = _fixture.Create<SubstanceCreateDTO>();
        var content1 = new StringContent(JsonConvert.SerializeObject(substance1), Encoding.UTF8, "application/json");
        await _client.PostAsync($"{_client.BaseAddress}/Substances", content1); 
    
        var substance2 = _fixture.Create<SubstanceCreateDTO>();
        var content2 = new StringContent(JsonConvert.SerializeObject(substance2), Encoding.UTF8, "application/json");
        await _client.PostAsync($"{_client.BaseAddress}/Substances", content2); 

        // Act
        var response = await _client.GetAsync($"{_client.BaseAddress}/Substances");
        response.EnsureSuccessStatusCode(); // Ensure 200-299 status code range
        var substances = JsonConvert.DeserializeObject<IEnumerable<Substance>>(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.NotEmpty(substances);
    }

    [Fact]
public async Task GetSubstanceById_WhenSubstanceExists()
{
    // Arrange

    var disease = _fixture.Create<DiseaseCreateDTO>();
    var contentDisease = new StringContent(JsonConvert.SerializeObject(disease), Encoding.UTF8, "application/json");
    var responseDisease = await _client.PostAsync($"{_client.BaseAddress}/Diseases", contentDisease);
    responseDisease.EnsureSuccessStatusCode(); // Upewnij się, że odpowiedź jest w zakresie 200-299


    var newSubstance = _fixture.Create<SubstanceCreateDTO>();
    newSubstance.Diseases = new List<string> { disease.Name };
    var content = new StringContent(JsonConvert.SerializeObject(newSubstance), Encoding.UTF8, "application/json");

    // Act

    var response = await _client.PostAsync($"{_client.BaseAddress}/Substances", content);
    response.EnsureSuccessStatusCode(); // Upewnij się, że odpowiedź ma status 200-299


    var substanceSearchResponse = await _client.GetAsync($"{_client.BaseAddress}/Substances/search/{newSubstance.Name}");
    substanceSearchResponse.EnsureSuccessStatusCode();
    var substanceSearchResult = JsonConvert.DeserializeObject<Substance>(await substanceSearchResponse.Content.ReadAsStringAsync());
 
    var responseById = await _client.GetAsync($"{_client.BaseAddress}/Substances/{substanceSearchResult.Id}");
    responseById.EnsureSuccessStatusCode(); 
    var dbSubstance = JsonConvert.DeserializeObject<Substance>(await responseById.Content.ReadAsStringAsync());

    // Assert
    
    Assert.Equal(newSubstance.Name, dbSubstance.SubstanceName); 
 
}

    [Fact]
public async Task UpdateSubstance_WhenSubstanceExists()
{
    // Arrange
    var disease = _fixture.Create<DiseaseCreateDTO>();
    var contentDisease = new StringContent(JsonConvert.SerializeObject(disease), Encoding.UTF8, "application/json");
    var responseDisease = await _client.PostAsync($"{_client.BaseAddress}/Diseases", contentDisease);
    responseDisease.EnsureSuccessStatusCode(); // Upewnij się, że odpowiedź jest w zakresie 200-299

    var newSubstance = _fixture.Create<SubstanceCreateDTO>();
    newSubstance.Diseases = new List<string> { disease.Name };
    var content = new StringContent(JsonConvert.SerializeObject(newSubstance), Encoding.UTF8, "application/json");
    
    // Act
    
    var response = await _client.PostAsync($"{_client.BaseAddress}/Substances", content);
    response.EnsureSuccessStatusCode(); // Upewnij się, że odpowiedź ma status 200-299
    var substanceSearchResponse = await _client.GetAsync($"{_client.BaseAddress}/Substances/search/{newSubstance.Name}");
    substanceSearchResponse.EnsureSuccessStatusCode(); // Upewnij się, że odpowiedź ma status 200-299
    var substanceSearchResult = JsonConvert.DeserializeObject<Substance>(await substanceSearchResponse.Content.ReadAsStringAsync());
    
    var updateDto = new SubstanceUpdateDTO
    {
        Name = "Updated Substance Name",
        Dosage = "Updated Dosage"
    };
    var updateContent = new StringContent(JsonConvert.SerializeObject(updateDto), Encoding.UTF8, "application/json");
    
    var responseUpdate = await _client.PutAsync($"{_client.BaseAddress}/Substances/{substanceSearchResult.Id}", updateContent);
    responseUpdate.EnsureSuccessStatusCode(); 
    var responseById = await _client.GetAsync($"{_client.BaseAddress}/Substances/{substanceSearchResult.Id}");
    responseById.EnsureSuccessStatusCode(); 

    // Assert:
    var updatedSubstance = JsonConvert.DeserializeObject<Substance>(await responseById.Content.ReadAsStringAsync());
    Assert.Equal(updateDto.Name, updatedSubstance.SubstanceName); 
    Assert.Equal(updateDto.Dosage, updatedSubstance.Dosage);
}
    [Fact]
    public async Task DeleteSubstance_WhenSubstanceExists()
    {
        // Arrange
        var disease = _fixture.Create<DiseaseCreateDTO>();
        var contentDisease = new StringContent(JsonConvert.SerializeObject(disease), Encoding.UTF8, "application/json");
       
        var responseDisease = await _client.PostAsync($"{_client.BaseAddress}/Diseases", contentDisease);
        responseDisease.EnsureSuccessStatusCode(); // Upewnij się, że odpowiedź jest w zakresie 200-299
       
        var newSubstance = _fixture.Create<SubstanceCreateDTO>();
        newSubstance.Diseases = new List<string> { disease.Name };
        
        var content = new StringContent(JsonConvert.SerializeObject(newSubstance), Encoding.UTF8, "application/json");
        // Act
        var response = await _client.PostAsync($"{_client.BaseAddress}/Substances", content);
        response.EnsureSuccessStatusCode(); // Upewnij się, że odpowiedź ma status 200-299
        var substanceSearchResponse =
            await _client.GetAsync($"{_client.BaseAddress}/Substances/search/{newSubstance.Name}");
        substanceSearchResponse.EnsureSuccessStatusCode(); // Upewnij się, że odpowiedź ma status 200-299
        var substanceSearchResult =
            JsonConvert.DeserializeObject<Substance>(await substanceSearchResponse.Content.ReadAsStringAsync());
        // Act
        var responseDelete = await _client.DeleteAsync($"{_client.BaseAddress}/Substances/{substanceSearchResult.Id}");
        responseDelete.EnsureSuccessStatusCode(); // Upewnij się, że odpowiedź ma status 204 (No Content)
        var responseGettingIDDeletedSubstance =
            await _client.GetAsync($"{_client.BaseAddress}/Substances/{substanceSearchResult.Id}");
        // Assert: 
        Assert.Equal(HttpStatusCode.NotFound,
            responseGettingIDDeletedSubstance.StatusCode); 
    }

   [Fact]
public async Task GetSubstanceByName_WhenSubstanceExists()
{
    // Arrange
    var disease = _fixture.Create<DiseaseCreateDTO>();
    var contentDisease = new StringContent(JsonConvert.SerializeObject(disease), Encoding.UTF8, "application/json");
    var responseDisease = await _client.PostAsync($"{_client.BaseAddress}/Diseases", contentDisease);
    responseDisease.EnsureSuccessStatusCode(); // Upewnij się, że odpowiedź jest w zakresie 200-29
    var newSubstance = _fixture.Create<SubstanceCreateDTO>();
    newSubstance.Diseases = new List<string> { disease.Name };
    var content = new StringContent(JsonConvert.SerializeObject(newSubstance), Encoding.UTF8, "application/json");
    // Act
    var response = await _client.PostAsync($"{_client.BaseAddress}/Substances", content);
    response.EnsureSuccessStatusCode(); // Upewnij się, że odpowiedź ma status 200-299
    var substanceSearchResponse = await _client.GetAsync($"{_client.BaseAddress}/Substances/search/{newSubstance.Name}");
    substanceSearchResponse.EnsureSuccessStatusCode(); // Upewnij się, że odpowiedź ma status 200-299
    var substanceSearchResult = JsonConvert.DeserializeObject<Substance>(await substanceSearchResponse.Content.ReadAsStringAsync());
    var responseById = await _client.GetAsync($"{_client.BaseAddress}/Substances/{substanceSearchResult.Id}");
    responseById.EnsureSuccessStatusCode(); // Upewnij się, że odpowiedź ma status 200-299

    // Assert: 
    var dbSubstance = JsonConvert.DeserializeObject<Substance>(await responseById.Content.ReadAsStringAsync());

    Assert.Equal(newSubstance.Name, dbSubstance.SubstanceName);
    Assert.Equal(substanceSearchResult.Id, dbSubstance.Id); 
}

}