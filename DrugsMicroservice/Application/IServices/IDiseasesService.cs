using DrugsMicroservice.BusinessLogic.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DrugsMicroservice.Application.IServices;

public interface IDiseasesService
{
    Task<IEnumerable<Disease>> GetAllDiseasesAsync();  
    Task<Disease> GetDiseaseByIdAsync(Guid id);  
    Task<Disease> GetDiseaseByNameAsync(string name);  
    Task<Disease> AddDiseaseAsync(Disease disease); 
    Task<Disease> UpdateDiseaseAsync(Guid id, Disease disease); 
    Task<bool> DeleteDiseaseAsync(Guid id);  
}