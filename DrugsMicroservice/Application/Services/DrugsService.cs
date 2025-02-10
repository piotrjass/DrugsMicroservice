using DrugsMicroservice.Application.IServices;
using DrugsMicroservice.BusinessLogic.Models;
using DrugsMicroservice.Application.DTOs;
using DrugsMicroservice.BusinessLogic.IRepositories;

namespace DrugsMicroservice.Application.Services
{
    public class DrugsService : IDrugsService
    {
        private readonly IDrugsRepository _drugsRepository;
        private readonly ISubstancesRepository _substancesRepository;

        public DrugsService(IDrugsRepository drugsRepository, ISubstancesRepository substancesRepository)
        {
            _drugsRepository = drugsRepository;
            _substancesRepository = substancesRepository;
        }

        public async Task<IEnumerable<Drug>> GetAllDrugsAsync()
        {
            return await _drugsRepository.GetAllDrugsAsync();
        }

        public async Task<Drug> GetDrugByIdAsync(Guid id)
        {
            return await _drugsRepository.GetDrugByIdAsync(id);
        }

        public async Task<Drug> AddDrugAsync(DrugCreateDTO newDrugDto)
        {
          
           var existingDrug = await _drugsRepository.GetDrugByNameAsync(newDrugDto.Name);
            if (existingDrug != null)
            {
                return null; 
            }
            
            Drug newDrug = new Drug
            {
                Name = newDrugDto.Name,
                Manufacturer = newDrugDto.Manufacturer,
                Price = newDrugDto.Price
            };

          
            foreach (var substanceName in newDrugDto.Substances)
            {
                var substance = await  _substancesRepository.GetSubstanceByNameAsync(substanceName);
                if (substance == null)
                {
                    return null; 
                }
                newDrug.Substances.Add(substance);
            }


            return await _drugsRepository.AddDrugAsync(newDrug);
        }

        public async Task<Drug> UpdateDrugAsync(Guid id, Drug drug)
        {
            return await _drugsRepository.UpdateDrugAsync(drug);
        }

        public async Task<bool> DeleteDrugAsync(Guid id)
        {
            return await _drugsRepository.DeleteDrugAsync(id);
        }
    }
}