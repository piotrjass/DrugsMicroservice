using DrugsMicroservice.BusinessLogic.Models;

namespace DrugsMicroservice.Application.IServices;

public interface IDrugsService
{
    IEnumerable<Drug> GetAllDrugs();
    Drug GetDrugById(Guid id);
    Drug AddDrug(Drug drug);
    Drug UpdateDrug(Guid id, Drug drug);
    bool DeleteDrug(Guid id);
}