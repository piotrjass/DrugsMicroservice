using DrugsMicroservice.BusinessLogic.Models;
using DrugsMicroservice.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace DrugsMicroservice.DataAccess.Repositories
{
    public class DiseasesRepository : IDiseasesRepository
    {
        private readonly ApplicationDbContext _context;  

        public DiseasesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Disease> GetAll()
        {
            return _context.Diseases.ToList();  
        }

        public Disease GetById(Guid id)
        {
            return _context.Diseases.FirstOrDefault(d => d.Id == id);  
        }
        
        public Disease GetDiseaseByName(string name)
        {
            return _context.Diseases.FirstOrDefault(d => d.Name == name);
        }

        public Disease Add(Disease disease)
        {
            _context.Diseases.Add(disease);  
            _context.SaveChanges();
            return disease;
        }

        public Disease Update(Disease disease)
        {
            _context.Diseases.Update(disease);  
            _context.SaveChanges();
            return disease;
        }

        public bool Delete(Guid id)
        {
            var disease = _context.Diseases.FirstOrDefault(d => d.Id == id);  
            if (disease == null)
            {
                return false;
            }

            _context.Diseases.Remove(disease); 
            _context.SaveChanges();
            return true;
        }
    }
}
