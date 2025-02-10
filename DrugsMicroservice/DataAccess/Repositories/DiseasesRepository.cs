using DrugsMicroservice.BusinessLogic.IRepositories;
using DrugsMicroservice.BusinessLogic.Models;
using DrugsMicroservice.DataAccess;
using Microsoft.EntityFrameworkCore;

public class DiseasesRepository : IDiseasesRepository
{
    private readonly ApplicationDbContext _context;

    public DiseasesRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Disease>> GetAllAsync()
    {
        return await _context.Diseases.ToListAsync();  
    }

    public async Task<Disease> GetByIdAsync(Guid id)
    {
        return await _context.Diseases.FirstOrDefaultAsync(d => d.Id == id);  
    }

    public async Task<Disease> GetDiseaseByNameAsync(string name)
    {
        return await _context.Diseases.FirstOrDefaultAsync(d => d.Name == name); 
    }

    public async Task<Disease> AddAsync(Disease disease)
    {
        await _context.Diseases.AddAsync(disease); 
        await _context.SaveChangesAsync();  
        return disease;
    }

    public async Task<Disease> UpdateAsync(Disease disease)
    {
        _context.Diseases.Update(disease);  
        await _context.SaveChangesAsync();  
        return disease;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var disease = await _context.Diseases.FindAsync(id);  
        if (disease == null)
        {
            return false;  
        }

        _context.Diseases.Remove(disease);  
        await _context.SaveChangesAsync();  
        return true;
    }
}