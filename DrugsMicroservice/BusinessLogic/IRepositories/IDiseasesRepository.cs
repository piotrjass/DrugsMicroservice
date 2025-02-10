using DrugsMicroservice.BusinessLogic.Models;

namespace DrugsMicroservice.DataAccess.IRepositories;

public interface IDiseasesRepository
{
    IEnumerable<Disease> GetAll();
    Disease GetById(Guid id);
    Disease GetDiseaseByName(string name);
    Disease Add(Disease disease);
    Disease Update(Disease disease);
    bool Delete(Guid id);
}
