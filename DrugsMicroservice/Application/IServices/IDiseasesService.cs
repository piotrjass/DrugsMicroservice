using DrugsMicroservice.BusinessLogic.Models;

namespace DrugsMicroservice.Application.IServices;

public interface IDiseasesService
{
    IEnumerable<Disease> GetAllDiseases();
    Disease GetDiseaseById(Guid id);
    Disease GetDiseaseByName(string name);
    Disease AddDisease(Disease disease);
    Disease UpdateDisease(Guid id, Disease disease);
    bool DeleteDisease(Guid id);
    
}
