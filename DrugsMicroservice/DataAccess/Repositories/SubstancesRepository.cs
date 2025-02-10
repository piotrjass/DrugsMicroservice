using DrugsMicroservice.BusinessLogic.Models;
using DrugsMicroservice.DataAccess.IRepositories;

namespace DrugsMicroservice.DataAccess.Repositories;

public class SubstancesRepository : ISubstancesRepository
{
    private readonly ApplicationDbContext _context;

    public SubstancesRepository(ApplicationDbContext context)
    {
        _context = context;
    }


    public IEnumerable<Substance> GetAllSubstances()
    {
        return _context.Substances.ToList(); 
    }
    
    public Substance GetSubstanceById(Guid id)
    {
        return _context.Substances.FirstOrDefault(s => s.Id == id); 
    }
    
    public Substance AddSubstance(Substance substance)
    {
        _context.Substances.Add(substance);  
        _context.SaveChanges();             
        return substance;                  
    }
    
    public Substance UpdateSubstance(Guid id, Substance substance)
    {
        var existingSubstance = _context.Substances.FirstOrDefault(s => s.Id == id);
        if (existingSubstance != null)
        {
            existingSubstance.SubstanceName = substance.SubstanceName;
            existingSubstance.Dosage = substance.Dosage;
            _context.SaveChanges(); 
            return existingSubstance; 
        }
        return null;  
    }
    
    public bool DeleteSubstance(Guid id)
    {
        var substance = _context.Substances.FirstOrDefault(s => s.Id == id);
        if (substance != null)
        {
            _context.Substances.Remove(substance); 
            _context.SaveChanges();               
            return true;  
        }
        return false; 
    }
    
    public Substance GetSubstanceByName(string name)
    {
        return _context.Substances
            .FirstOrDefault(s => s.SubstanceName.ToLower() == name.ToLower());
    }
}
