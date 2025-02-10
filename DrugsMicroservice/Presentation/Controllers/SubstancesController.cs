using DrugsMicroservice.Application.DTOs.Substances;
using DrugsMicroservice.Application.IServices;
using DrugsMicroservice.BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace DrugsMicroservice.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubstancesController : ControllerBase
{
    private readonly ISubstancesService _substancesService;
    private readonly IDiseasesService _diseasesService;
    
    public SubstancesController(ISubstancesService substancesService, IDiseasesService diseasesService)
    {
        _substancesService = substancesService;
        _diseasesService = diseasesService;
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
        
        foreach (var diseaseName in newSubstanceDto.Diseases )
        {
            var disease = _diseasesService.GetDiseaseByName(diseaseName);
        
            if (disease == null)
            {
                return NotFound($"Substance '{diseaseName}' not found.");
            }

            newSubstance.Diseases.Add(disease);
        }
        
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
    
    [HttpGet("GetSubstanceByName/{name}")]
    public ActionResult<Substance> GetSubstanceByName(string name)
    {
        var substance = _substancesService.GetSubstanceByName(name);
        if (substance == null)
        {
            return NotFound(); 
        }
        return Ok(substance); 
    }
}
