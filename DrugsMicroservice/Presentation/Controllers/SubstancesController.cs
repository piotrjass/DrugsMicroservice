using DrugsMicroservice.Application.DTOs.Substances;
using DrugsMicroservice.Application.IServices;
using DrugsMicroservice.BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;


namespace DrugsMicroservice.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubstancesController : ControllerBase
    {
        private readonly ISubstancesService _substancesService;
        public SubstancesController(ISubstancesService substancesService)
        {
            _substancesService = substancesService;
        }

        /// <summary>
        /// Gets all substances.
        /// </summary>
        /// <returns>A list of substances.</returns>
        [HttpGet("GetAllSubstances")]
        [ProducesResponseType(typeof(IEnumerable<Substance>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<Substance>>> GetAllSubstances()
        {
            var substances = await _substancesService.GetAllSubstancesAsync();
            if (substances == null)
            {
                return NotFound();
            }
            return Ok(substances);
        }

        /// <summary>
        /// Gets a substance by its ID.
        /// </summary>
        /// <param name="id">The ID of the substance.</param>
        /// <returns>A substance object.</returns>
        [HttpGet("GetSubstanceById/{id}")]
        [ProducesResponseType(typeof(Substance), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Substance>> GetSubstanceById(Guid id)
        {
            var substance = await _substancesService.GetSubstanceByIdAsync(id);
            if (substance == null)
            {
                return NotFound();
            }
            return Ok(substance);
        }

        /// <summary>
        /// Adds a new substance.
        /// </summary>
        /// <param name="newSubstanceDto">The substance data to add.</param>
        /// <returns>The created substance.</returns>
        [HttpPost("AddSubstance")]
        [ProducesResponseType(typeof(Substance), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Substance>> AddSubstance([FromBody] SubstanceCreateDTO newSubstanceDto)
        {
            if (newSubstanceDto == null)
            {
                return BadRequest("Substance data is null.");
            }

            var createdSubstance = await _substancesService.AddSubstanceAsync(newSubstanceDto);

            if (createdSubstance == null)
            {
                return BadRequest("There was an error while adding the substance.");
            }

            return CreatedAtAction(nameof(GetSubstanceById), new { id = createdSubstance.Id }, createdSubstance);
        }


        /// <summary>
        /// Updates a substance by its ID.
        /// </summary>
        /// <param name="id">The ID of the substance to update.</param>
        /// <param name="substanceUpdateDto">The updated substance data.</param>
        /// <returns>The updated substance.</returns>
        [HttpPut("UpdateSubstance/{id}")]
        [ProducesResponseType(typeof(Substance), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Substance>> UpdateSubstance(Guid id, [FromBody] SubstanceUpdateDTO substanceUpdateDto)
        {
            if (substanceUpdateDto == null)
            {
                return BadRequest("Substance data is null.");
            }

            var updatedSubstance = await _substancesService.UpdateSubstanceAsync(id, new Substance
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

        /// <summary>
        /// Deletes a substance by its ID.
        /// </summary>
        /// <param name="id">The ID of the substance to delete.</param>
        [HttpDelete("DeleteSubstance/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteSubstance(Guid id)
        {
            var result = await _substancesService.DeleteSubstanceAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        /// <summary>
        /// Gets a substance by its name.
        /// </summary>
        /// <param name="name">The name of the substance.</param>
        /// <returns>A substance object.</returns>
        [HttpGet("GetSubstanceByName/{name}")]
        [ProducesResponseType(typeof(Substance), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Substance>> GetSubstanceByName(string name)
        {
            var substance = await _substancesService.GetSubstanceByNameAsync(name);
            if (substance == null)
            {
                return NotFound();
            }
            return Ok(substance);
        }
    }
}
