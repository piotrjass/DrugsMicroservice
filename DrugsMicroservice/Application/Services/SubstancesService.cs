using DrugsMicroservice.Application.IServices;
using DrugsMicroservice.BusinessLogic.Models;
using DrugsMicroservice.DataAccess.IRepositories;

namespace DrugsMicroservice.Application.Services;

public class SubstancesService : ISubstancesService
{
    private readonly ISubstancesRepository _substancesRepository;

    public SubstancesService(ISubstancesRepository substancesRepository)
    {
        _substancesRepository = substancesRepository;
    }
    
    public IEnumerable<Substance> GetAllSubstances()
    {
        return _substancesRepository.GetAllSubstances();
    }
    
    public Substance GetSubstanceById(Guid id)
    {
        return _substancesRepository.GetSubstanceById(id);
    }
    
    public Substance AddSubstance(Substance substance)
    {
        return _substancesRepository.AddSubstance(substance);
    }
    
    public Substance UpdateSubstance(Guid id, Substance substance)
    {
        return _substancesRepository.UpdateSubstance(id, substance);
    }
    
    public bool DeleteSubstance(Guid id)
    {
        return _substancesRepository.DeleteSubstance(id);
    }
    
    public Substance GetSubstanceByName(string name)
    {
        return _substancesRepository.GetSubstanceByName(name);
    }
}
