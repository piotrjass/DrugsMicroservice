using DrugsMicroservice.BusinessLogic.Models;

namespace DrugsMicroservice.DataAccess.IRepositories;

public interface IDrugsRepository
{
    IEnumerable<Drug> GetAllDrugs();
    Drug GetDrugById(Guid id);
    Drug AddDrug(Drug drug);
    Drug UpdateDrug(Drug drug);
    bool DeleteDrug(Guid id);
}