using DrugsMicroservice.BusinessLogic.Models;

namespace DrugsMicroservice.Application.IServices;

public interface ISubstancesService
{
    IEnumerable<Substance> GetAllSubstances(); 
    Substance GetSubstanceById(Guid id);
    Substance AddSubstance(Substance substance); 
    Substance UpdateSubstance(Guid id, Substance substance); 
    bool DeleteSubstance(Guid id);
    
    Substance GetSubstanceByName(string name);
}
