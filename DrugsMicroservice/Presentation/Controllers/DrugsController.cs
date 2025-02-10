using DrugsMicroservice.Application.DTOs;
using DrugsMicroservice.Application.IServices;
using DrugsMicroservice.BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;

namespace DrugsMicroservice.Presentation.Controllers
{
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

        /// <summary>
        /// Gets all drugs.
        /// </summary>
        /// <returns>Returns a list of all drugs.</returns>
        /// <response code="200">Returns the list of drugs.</response>
        /// <response code="500">If there was an error retrieving the drugs.</response>
        [HttpGet("GetAllDrugs")]
        [ProducesResponseType(typeof(IEnumerable<Drug>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Drug>>> GetAllDrugs()
        {
            var drugs = await _drugsService.GetAllDrugsAsync();
            return Ok(drugs);
        }

        /// <summary>
        /// Gets a drug by its ID.
        /// </summary>
        /// <param name="id">The ID of the drug to retrieve.</param>
        /// <returns>Returns a drug object.</returns>
        /// <response code="200">Returns the drug.</response>
        /// <response code="404">If the drug with the specified ID was not found.</response>
        [HttpGet("GetAllDrugsById/{id}")]
        [ProducesResponseType(typeof(Drug), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Drug>> GetAllDrugsById(Guid id)
        {
            var drug = await _drugsService.GetDrugByIdAsync(id);
            if (drug == null)
            {
                return NotFound();
            }
            return Ok(drug);
        }

        /// <summary>
        /// Adds a new drug.
        /// </summary>
        /// <param name="newDrugDto">The drug details to be added.</param>
        /// <returns>Returns the created drug object.</returns>
        /// <response code="201">Returns the created drug.</response>
        /// <response code="400">If the drug data is invalid.</response>
        /// <response code="404">If any of the substances for the drug do not exist.</response>
        [HttpPost("AddDrug")]
        [ProducesResponseType(typeof(Drug), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Drug>> AddDrug([FromBody] DrugCreateDTO newDrugDto)
        {
            if (newDrugDto == null)
            {
                return BadRequest("Drug data is null.");
            }

            var createdDrug = await _drugsService.AddDrugAsync(newDrugDto);
            if (createdDrug == null)
            {
                return BadRequest("There was an error while adding the drug.");
            }

            return CreatedAtAction(nameof(GetAllDrugsById), new { id = createdDrug.Id }, createdDrug);
        }
        /// <summary>
        /// Updates an existing drug.
        /// </summary>
        /// <param name="id">The ID of the drug to update.</param>
        /// <param name="drugUpdateDto">The updated drug details.</param>
        /// <returns>Returns the updated drug object.</returns>
        /// <response code="200">Returns the updated drug.</response>
        /// <response code="400">If the drug data is invalid.</response>
        /// <response code="404">If the drug with the specified ID was not found.</response>
        [HttpPut("UpdateDrug/{id}")]
        [ProducesResponseType(typeof(Drug), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Drug>> UpdateDrug(Guid id, [FromBody] DrugUpdateDTO drugUpdateDto)
        {
            if (drugUpdateDto == null)
            {
                return BadRequest("Drug data is null.");
            }

            var updatedDrug = await _drugsService.UpdateDrugAsync(id, new Drug
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

        /// <summary>
        /// Deletes a drug.
        /// </summary>
        /// <param name="id">The ID of the drug to delete.</param>
        /// <returns>Returns a no content status if the drug was deleted.</returns>
        /// <response code="204">If the drug was successfully deleted.</response>
        /// <response code="404">If the drug with the specified ID was not found.</response>
        [HttpDelete("DeleteDrug/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteDrug(Guid id)
        {
            var result = await _drugsService.DeleteDrugAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
