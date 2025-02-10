using DrugsMicroservice.BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrugsMicroservice.BusinessLogic.IRepositories;

namespace DrugsMicroservice.DataAccess.Repositories
{
    public class DrugsRepository : IDrugsRepository
    {
        private readonly ApplicationDbContext _context;

        public DrugsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Drug>> GetAllDrugsAsync()
        {
            return await _context.Drugs.ToListAsync();
        }

        public async Task<Drug> GetDrugByIdAsync(Guid id)
        {
            return await _context.Drugs
                .FirstOrDefaultAsync(d => d.Id == id);
        }
        
        public async Task<Drug> GetDrugByNameAsync(string name)
        {
            return await _context.Drugs
                .FirstOrDefaultAsync(d => d.Name.ToLower() == name.ToLower());
        }

        public async Task<Drug> AddDrugAsync(Drug drug)
        {
            await _context.Drugs.AddAsync(drug);
            await _context.SaveChangesAsync();
            return drug;
        }

        public async Task<Drug> UpdateDrugAsync(Drug drug)
        {
            _context.Drugs.Update(drug);
            await _context.SaveChangesAsync();
            return drug;
        }

        public async Task<bool> DeleteDrugAsync(Guid id)
        {
            var drug = await _context.Drugs.FindAsync(id);
            if (drug == null)
            {
                return false;
            }

            _context.Drugs.Remove(drug);
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<IEnumerable<Drug>> GetDrugsByDiseaseAsync(string diseaseName)
        {
            
            var disease = await _context.Diseases
                .Include(d => d.Substances) 
                .Where(d => d.Name.ToLower() == diseaseName.ToLower())
                .FirstOrDefaultAsync();

            if (disease == null)
            {
                return Enumerable.Empty<Drug>();  
            }

            var drugs = await _context.Drugs
                .Where(d => d.Substances.Any(s => disease.Substances.Contains(s)))  
                .ToListAsync();

            return drugs;  
        }
    }
}