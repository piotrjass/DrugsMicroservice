using DrugsMicroservice.BusinessLogic.Models;

namespace DrugsMicroservice.BusinessLogic.IRepositories
{
    public interface ISubstancesRepository
    {
        Task<IEnumerable<Substance>> GetAllSubstancesAsync();  
        Task<Substance> GetSubstanceByIdAsync(Guid id);  
        Task<Substance> AddSubstanceAsync(Substance substance); 
        Task<Substance> UpdateSubstanceAsync(Guid id, Substance substance);  
        Task<bool> DeleteSubstanceAsync(Guid id);

        Task<IEnumerable<Substance>> GetSubstancesByDiseaseAsync(string diseaseName);
        Task<Substance> GetSubstanceByNameAsync(string name); 
    }
}