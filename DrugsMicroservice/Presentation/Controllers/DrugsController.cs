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
    
    [HttpGet("GetAllDrugs")]
    public ActionResult<IEnumerable<Drug>> GetAllDrugs()
    {
        var drugs = _drugsService.GetAllDrugs();
        return Ok(drugs); 
    }

    [HttpGet("GetAllDrugsById/{id}")]
    public ActionResult<Drug> GetAllDrugsById(Guid id)
    {
        var drug = _drugsService.GetDrugById(id);
        if (drug == null)
        {
            return NotFound(); 
        }
        return Ok(drug); 
    }


    [HttpPost("AddDrug")]
    public ActionResult<Drug> AddDrug([FromBody] DrugCreateDTO newDrugDto)
    {
        if (newDrugDto == null)
        {
            return BadRequest("Drug data is null.");
        }

        Drug newDrug = new Drug
        {
            Name = newDrugDto.Name,
            Manufacturer = newDrugDto.Manufacturer,
            Price = newDrugDto.Price
        };
        
        foreach (var substanceName in newDrugDto.Substances)
        {
            var substance = _substancesService.GetSubstanceByName(substanceName);
        
            if (substance == null)
            {
                return NotFound($"Substance '{substanceName}' not found.");
            }

            newDrug.Substances.Add(substance);
        }
        var createdDrug = _drugsService.AddDrug(newDrug);
        if (createdDrug == null)
        {
            return BadRequest("There was an error while adding the drug.");
        }

        return createdDrug;
    }

 
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
            return NotFound();
        }

        return Ok(updatedDrug); 
    }


    [HttpDelete("DeleteDrug/{id}")]
    public ActionResult DeleteDrug(Guid id)
    {
        var result = _drugsService.DeleteDrug(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent(); 
    }
}