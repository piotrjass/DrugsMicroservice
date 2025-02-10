using DrugsMicroservice.BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;
using DrugsMicroservice.BusinessLogic.IRepositories;

namespace DrugsMicroservice.DataAccess.Repositories
{
    public class SubstancesRepository : ISubstancesRepository
    {
        private readonly ApplicationDbContext _context;

        public SubstancesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Substance>> GetAllSubstancesAsync()
        {
            return await _context.Substances.ToListAsync(); 
        }

        public async Task<Substance> GetSubstanceByIdAsync(Guid id)
        {
            return await _context.Substances
                .FirstOrDefaultAsync(s => s.Id == id); 
        }

        public async Task<Substance> AddSubstanceAsync(Substance substance)
        {
            await _context.Substances.AddAsync(substance); 
            await _context.SaveChangesAsync();
            return substance;
        }

        public async Task<Substance> UpdateSubstanceAsync(Guid id, Substance substance)
        {
            var existingSubstance = await _context.Substances
                .FirstOrDefaultAsync(s => s.Id == id);

            if (existingSubstance == null)
                return null;

            existingSubstance.SubstanceName = substance.SubstanceName;
            existingSubstance.Dosage = substance.Dosage;

            await _context.SaveChangesAsync();
            return existingSubstance;
        }

        public async Task<bool> DeleteSubstanceAsync(Guid id)
        {
            var substance = await _context.Substances
                .FirstOrDefaultAsync(s => s.Id == id);

            if (substance == null)
                return false;

            _context.Substances.Remove(substance);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Substance> GetSubstanceByNameAsync(string name)
        {
            return await _context.Substances
                .FirstOrDefaultAsync(s => s.SubstanceName == name); 
        }
        
        public async Task<IEnumerable<Substance>> GetSubstancesByDiseaseAsync(string diseaseName)
        {
            var disease = await _context.Diseases
                .Include(d => d.Substances)  
                .Where(d => d.Name.ToLower() == diseaseName.ToLower())
                .FirstOrDefaultAsync();

            if (disease == null)
            {
                return Enumerable.Empty<Substance>();  
            }

            return disease.Substances;  
        }
    }
}
