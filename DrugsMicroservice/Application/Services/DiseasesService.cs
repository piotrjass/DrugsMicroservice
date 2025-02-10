using DrugsMicroservice.Application.IServices;
using DrugsMicroservice.BusinessLogic.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using DrugsMicroservice.BusinessLogic.IRepositories;

namespace DrugsMicroservice.Application.Services;

public class DiseasesService : IDiseasesService
{
    private readonly IDiseasesRepository _diseasesRepository;

    public DiseasesService(IDiseasesRepository diseasesRepository)
    {
        _diseasesRepository = diseasesRepository;
    }

    public async Task<IEnumerable<Disease>> GetAllDiseasesAsync()
    {
        return await _diseasesRepository.GetAllAsync(); 
    }

    public async Task<Disease> GetDiseaseByIdAsync(Guid id)
    {
        return await _diseasesRepository.GetByIdAsync(id); 
    }

    public async Task<Disease> GetDiseaseByNameAsync(string name)
    {
        return await _diseasesRepository.GetDiseaseByNameAsync(name);  
    }

    public async Task<Disease> AddDiseaseAsync(Disease disease)
    {
        return await _diseasesRepository.AddAsync(disease);  
    }

    public async Task<Disease> UpdateDiseaseAsync(Guid id, Disease disease)
    {
        var existingDisease = await _diseasesRepository.GetByIdAsync(id);  
        if (existingDisease == null)
        {
            return null;  
        }

        existingDisease.Name = disease.Name; 

        return await _diseasesRepository.UpdateAsync(existingDisease);  
    }

    public async Task<bool> DeleteDiseaseAsync(Guid id)
    {
        var disease = await _diseasesRepository.GetByIdAsync(id);  
        if (disease == null)
        {
            return false;  
        }

        return await _diseasesRepository.DeleteAsync(id); 
    }
}
