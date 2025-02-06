using DrugsMicroservice.BusinessLogic.Models;

namespace DrugsMicroservice.DataAccess.IRepositories;

public interface ISubstancesRepository
{
    IEnumerable<Substance> GetAllSubstances(); // Pobiera wszystkie substancje
    Substance GetSubstanceById(Guid id); // Pobiera substancję po ID
    Substance AddSubstance(Substance substance); // Dodaje nową substancję
    Substance UpdateSubstance(Guid id, Substance substance); // Aktualizuje substancję po ID
    bool DeleteSubstance(Guid id); // Usuwa substancję po ID
}
