using DrugsMicroservice.Application.DTOs.Substances;
using DrugsMicroservice.Application.IServices;
using DrugsMicroservice.BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;

namespace DrugsMicroservice.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubstancesController : ControllerBase
{
    private readonly ISubstancesService _substancesService;
    
    public SubstancesController(ISubstancesService substancesService)
    {
        _substancesService = substancesService;
    }
    
    [HttpGet("GetAllSubstances")]
    public ActionResult<IEnumerable<Substance>> GetAllSubstances()
    {
        var substances = _substancesService.GetAllSubstances();
        return Ok(substances); 
    }
    
    [HttpGet("GetSubstanceById/{id}")]
    public ActionResult<Substance> GetSubstanceById(Guid id)
    {
        var substance = _substancesService.GetSubstanceById(id);
        if (substance == null)
        {
            return NotFound(); 
        }
        return Ok(substance); 
    }
    [HttpPost("AddSubstance")]
    public async Task<ActionResult<Substance>> AddSubstance([FromBody] SubstanceCreateDTO newSubstanceDto)
    {
        if (newSubstanceDto == null)
        {
            return BadRequest("Substance data is null.");
        }
        
        var newSubstance = new Substance
        {
            Id = Guid.NewGuid(),  
            SubstanceName = newSubstanceDto.Name,
            Dosage = newSubstanceDto.Dosage
        };

     
        var createdSubstance =  _substancesService.AddSubstance(newSubstance); 

        if (createdSubstance == null)
        {
            return BadRequest("There was an error while adding the substance.");
        }
        
        return CreatedAtAction(nameof(GetSubstanceById), new { id = createdSubstance.Id }, createdSubstance);
    }


    [HttpPut("UpdateSubstance/{id}")]
    public ActionResult<Substance> UpdateSubstance(Guid id, [FromBody] SubstanceUpdateDTO substanceUpdateDto)
    {
        if (substanceUpdateDto == null)
        {
            return BadRequest("Substance data is null.");
        }

        var updatedSubstance = _substancesService.UpdateSubstance(id, new Substance
        {
            SubstanceName = substanceUpdateDto.Name,
            Dosage = substanceUpdateDto.Dosage
        });

        if (updatedSubstance == null)
        {
            return NotFound(); 
        }

        return Ok(updatedSubstance); 
    }

    
    [HttpDelete("DeleteSubstance/{id}")]
    public ActionResult DeleteSubstance(Guid id)
    {
        var result = _substancesService.DeleteSubstance(id);
        if (!result)
        {
            return NotFound(); 
        }
        return NoContent(); 
    }
}
