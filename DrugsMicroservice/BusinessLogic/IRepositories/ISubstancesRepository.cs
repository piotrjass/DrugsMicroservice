using DrugsMicroservice.BusinessLogic.Models;

namespace DrugsMicroservice.BusinessLogic.IRepositories
{
    public interface ISubstancesRepository
    {
        Task<IEnumerable<Substance>> GetAllSubstancesAsync();  // Asynchroniczne pobieranie wszystkich substancji
        Task<Substance> GetSubstanceByIdAsync(Guid id);  // Asynchroniczne pobieranie substancji po ID
        Task<Substance> AddSubstanceAsync(Substance substance);  // Asynchroniczne dodawanie substancji
        Task<Substance> UpdateSubstanceAsync(Guid id, Substance substance);  // Asynchroniczna aktualizacja substancji
        Task<bool> DeleteSubstanceAsync(Guid id);  // Asynchroniczne usuwanie substancji
        Task<Substance> GetSubstanceByNameAsync(string name);  // Asynchroniczne pobieranie substancji po nazwie
    }
}