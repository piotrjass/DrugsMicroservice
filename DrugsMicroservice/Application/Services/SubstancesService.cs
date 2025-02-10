using DrugsMicroservice.Application.IServices;
using DrugsMicroservice.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DrugsMicroservice.Application.DTOs.Substances;
using DrugsMicroservice.BusinessLogic.IRepositories;

namespace DrugsMicroservice.Application.Services
{
    public class SubstancesService : ISubstancesService
    {
        private readonly ISubstancesRepository _substancesRepository;
        private readonly IDiseasesService _diseasesService;

        public SubstancesService(ISubstancesRepository substancesRepository, IDiseasesService diseasesService)
        {
            _substancesRepository = substancesRepository;
            _diseasesService = diseasesService;
        }

        public async Task<IEnumerable<Substance>> GetAllSubstancesAsync()
        {
            return await _substancesRepository.GetAllSubstancesAsync(); 
        }

        public async Task<Substance> GetSubstanceByIdAsync(Guid id)
        {
            return await _substancesRepository.GetSubstanceByIdAsync(id);
        }

        public async Task<Substance> AddSubstanceAsync(SubstanceCreateDTO newSubstanceDto)
        {
            var existingSubstance = await _substancesRepository.GetSubstanceByNameAsync(newSubstanceDto.Name);
            if (existingSubstance != null)
            {
                return null; 
            }
            
            var newSubstance = new Substance
            {
                Id = Guid.NewGuid(),
                SubstanceName = newSubstanceDto.Name,
                Dosage = newSubstanceDto.Dosage
            };
            
            foreach (var diseaseName in newSubstanceDto.Diseases)
            {
                var disease = await _diseasesService.GetDiseaseByNameAsync(diseaseName);
                if (disease == null)
                {
                    return null;
                }

                newSubstance.Diseases.Add(disease);
            }

       
            return await _substancesRepository.AddSubstanceAsync(newSubstance);
        }

        public async Task<Substance> UpdateSubstanceAsync(Guid id, Substance substance)
        {
            return await _substancesRepository.UpdateSubstanceAsync(id, substance); 
        }

        public async Task<bool> DeleteSubstanceAsync(Guid id)
        {
            return await _substancesRepository.DeleteSubstanceAsync(id); 
        }

        public async Task<Substance> GetSubstanceByNameAsync(string name)
        {
            return await _substancesRepository.GetSubstanceByNameAsync(name); 
        }
    }
}