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

    // 1. GetAllSubstances
    public IEnumerable<Substance> GetAllSubstances()
    {
        return _substancesRepository.GetAllSubstances();
    }

    // 2. GetSubstanceById
    public Substance GetSubstanceById(Guid id)
    {
        return _substancesRepository.GetSubstanceById(id);
    }

    // 3. AddSubstance
    public Substance AddSubstance(Substance substance)
    {
        return _substancesRepository.AddSubstance(substance);
    }

    // 4. UpdateSubstance
    public Substance UpdateSubstance(Guid id, Substance substance)
    {
        return _substancesRepository.UpdateSubstance(id, substance);
    }

    // 5. DeleteSubstance
    public bool DeleteSubstance(Guid id)
    {
        return _substancesRepository.DeleteSubstance(id);
    }
}
