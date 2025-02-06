using DrugsMicroservice.Application.IServices;
using DrugsMicroservice.BusinessLogic.Models;
using DrugsMicroservice.DataAccess.IRepositories;

namespace DrugsMicroservice.Application.Services;

public class DrugsService : IDrugsService
{
    private readonly IDrugsRepository _drugsRepository;

    public DrugsService(IDrugsRepository drugsRepository)
    {
        _drugsRepository = drugsRepository;
    }
    
    public IEnumerable<Drug> GetAllDrugs()
    {
        return _drugsRepository.GetAllDrugs();
    }
    
    public Drug GetDrugById(Guid id)
    {
        return _drugsRepository.GetDrugById(id);
    }
    
    public Drug AddDrug(Drug drug)
    {
        return _drugsRepository.AddDrug(drug);
    }
    
    public Drug UpdateDrug(Guid id, Drug drug)
    {
        return _drugsRepository.UpdateDrug(drug);
    }
    
    public bool DeleteDrug(Guid id)
    {
        return _drugsRepository.DeleteDrug(id);
    }
}
