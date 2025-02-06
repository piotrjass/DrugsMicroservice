using DrugsMicroservice.Application.DTOs.Diseases;
using DrugsMicroservice.Application.IServices;
using DrugsMicroservice.BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;

namespace DrugsMicroservice.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DiseasesController : ControllerBase
{
    private readonly IDiseasesService _diseasesService;

    public DiseasesController(IDiseasesService diseasesService)
    {
        _diseasesService = diseasesService;
    }
    
    [HttpGet("GetAllDiseases")]
    public ActionResult<IEnumerable<Disease>> GetAllDiseases()
    {
        var diseases = _diseasesService.GetAllDiseases();
        return Ok(diseases); 
    }
    
    [HttpGet("GetDiseaseById/{id}")]
    public ActionResult<Disease> GetDiseaseById(Guid id)
    {
        var disease = _diseasesService.GetDiseaseById(id);
        if (disease == null)
        {
            return NotFound();
        }
        return Ok(disease); 
    }
    
    [HttpPost("AddDisease")]
    public ActionResult<Disease> AddDisease([FromBody] DiseaseCreateDTO newDiseaseDto)
    {
        if (newDiseaseDto == null)
        {
            return BadRequest("Disease data is null.");
        }
        
        var disease = new Disease
        {
            Name = newDiseaseDto.Name,
        };

        var createdDisease = _diseasesService.AddDisease(disease);

        return CreatedAtAction(nameof(GetDiseaseById), new { id = createdDisease.Id }, createdDisease);
    }
    
    [HttpPut("UpdateDisease/{id}")]
    public ActionResult<Disease> UpdateDisease(Guid id, [FromBody] DiseaseUpdateDTO diseaseUpdateDto)
    {
        if (diseaseUpdateDto == null)
        {
            return BadRequest("Disease data is null.");
        }

        var updatedDisease = _diseasesService.UpdateDisease(id, new Disease
        {
            Name = diseaseUpdateDto.Name,
        
        });

        if (updatedDisease == null)
        {
            return NotFound(); 
        }

        return Ok(updatedDisease); 
    }
    
    [HttpDelete("DeleteDisease/{id}")]
    public ActionResult DeleteDisease(Guid id)
    {
        var result = _diseasesService.DeleteDisease(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent(); 
    }
}
