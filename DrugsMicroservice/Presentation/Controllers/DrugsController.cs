using DrugsMicroservice.Application.DTOs;
using DrugsMicroservice.Application.IServices;
using DrugsMicroservice.BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;

namespace DrugsMicroservice.Presentation.Controllers;


[Route("api/[controller]")]
[ApiController]
public class DrugsController : ControllerBase
{
    private readonly IDrugsService _drugsService;
    private readonly ISubstancesService _substancesService;
    
    public DrugsController(IDrugsService drugsService, ISubstancesService substancesService)
    {
        _drugsService = drugsService;
        _substancesService = substancesService;
    }
    
    // GET: api/drugs
    [HttpGet("GetAllDrugs")]
    public ActionResult<IEnumerable<Drug>> GetAllDrugs()
    {
        var drugs = _drugsService.GetAllDrugs();
        return Ok(drugs); // Zwracamy listę leków
    }

    // GET: api/drugs/5
    [HttpGet("GetAllDrugsById/{id}")]
    public ActionResult<Drug> GetAllDrugsById(Guid id)
    {
        var drug = _drugsService.GetDrugById(id);
        if (drug == null)
        {
            return NotFound(); // Zwrócenie 404, jeśli lek nie został znaleziony
        }
        return Ok(drug); // Zwracamy lek w odpowiedzi
    }

    // POST: api/drugs/AddDrug
    [HttpPost("AddDrug")]
    public ActionResult<Drug> AddDrug([FromBody] DrugCreateDTO newDrugDto)
    {
        if (newDrugDto == null)
        {
            return BadRequest("Drug data is null.");
        }

        // Tworzenie nowego obiektu Drug z DTO
        var drug = new Drug
        {
            Name = newDrugDto.Name,
            Manufacturer = newDrugDto.Manufacturer,
            Price = newDrugDto.Price
        };
        
        // Dodanie nowego leku przez serwis
        var createdDrug = _drugsService.AddDrug(drug);

        // Zwrócenie odpowiedzi z kodem 201 i lokalizacją nowo dodanego leku
        return CreatedAtAction(nameof(GetAllDrugsById), new { id = createdDrug.Id }, createdDrug);
    }

    // PUT: api/drugs/UpdateDrug/5
    [HttpPut("UpdateDrug/{id}")]
    public ActionResult<Drug> UpdateDrug(Guid id, [FromBody] DrugUpdateDTO drugUpdateDto)
    {
        if (drugUpdateDto == null)
        {
            return BadRequest("Drug data is null.");
        }

        var updatedDrug = _drugsService.UpdateDrug(id, new Drug
        {
            Name = drugUpdateDto.Name,
            Manufacturer = drugUpdateDto.Manufacturer,
            Price = drugUpdateDto.Price
        });

        if (updatedDrug == null)
        {
            return NotFound(); // Zwrócenie 404, jeśli nie znaleziono leku
        }

        return Ok(updatedDrug); // Zwrócenie zaktualizowanego leku
    }

    // DELETE: api/drugs/DeleteDrug/5
    [HttpDelete("DeleteDrug/{id}")]
    public ActionResult DeleteDrug(Guid id)
    {
        var result = _drugsService.DeleteDrug(id);
        if (!result)
        {
            return NotFound(); // Zwrócenie 404, jeśli nie znaleziono leku do usunięcia
        }
        return NoContent(); // Zwrócenie 204, jeśli usunięcie powiodło się
    }
}