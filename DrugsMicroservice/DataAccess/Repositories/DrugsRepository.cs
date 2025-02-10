using DrugsMicroservice.BusinessLogic.Models;
using DrugsMicroservice.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace DrugsMicroservice.DataAccess.Repositories;

public class DrugsRepository : IDrugsRepository

{
    private readonly ApplicationDbContext _context;

    public DrugsRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Drug> GetAllDrugs()
    {
        // return _context.Drugs.ToList();
        return null;
    }

    public Drug GetDrugById(Guid id)
    {
        return _context.Drugs.AsEnumerable().FirstOrDefault(d => d.Id == id);
    }

    public Drug AddDrug(Drug drug)
    {
        _context.Drugs.Add(drug);
        _context.SaveChanges();
        return drug;
    }

    public Drug UpdateDrug(Drug drug)
    {
        _context.Drugs.Update(drug);
        _context.SaveChanges();
        return drug;
    }

    public bool DeleteDrug(Guid id)
    {
        var drug = _context.Drugs.FirstOrDefault(d => d.Id == id);
        if (drug == null) return false;

        _context.Drugs.Remove(drug);
        _context.SaveChanges();
        return true;
    }


}