using DrugsMicroservice.Application.IServices;
using DrugsMicroservice.BusinessLogic.Models;
using DrugsMicroservice.DataAccess.IRepositories;

namespace DrugsMicroservice.DataAccess.Repositories;

public class DiseasesService : IDiseasesService
{
    private readonly IDiseasesRepository _diseasesRepository;

    public DiseasesService(IDiseasesRepository diseasesRepository)
    {
        _diseasesRepository = diseasesRepository;
    }

    public IEnumerable<Disease> GetAllDiseases()
    {
        return _diseasesRepository.GetAll();
    }

    public Disease GetDiseaseById(Guid id)
    {
        return _diseasesRepository.GetById(id);
    }

    public Disease AddDisease(Disease disease)
    {
        return _diseasesRepository.Add(disease);
    }

    public Disease UpdateDisease(Guid id, Disease disease)
    {
        var existingDisease = _diseasesRepository.GetById(id);
        if (existingDisease == null)
        {
            return null;
        }

        existingDisease.Name = disease.Name;

        return _diseasesRepository.Update(existingDisease);
    }

    public bool DeleteDisease(Guid id)
    {
        var disease = _diseasesRepository.GetById(id);
        if (disease == null)
        {
            return false;
        }
        return _diseasesRepository.Delete(id);
    }
}
