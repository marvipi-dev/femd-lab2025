using Lab2025.Models;
using Lab2025.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Lab2025.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantesController : ControllerBase
    {
        private readonly IParticipantesRepository _repository;

        public ParticipantesController(IParticipantesRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(statusCode: StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ParticipantesGetResponse>>> GetAsync()
        {
            IEnumerable<ParticipanteModel> participantes;
            try
            {
                participantes = await _repository.ReadAsync();
            }
            catch (SqlException _)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }

            var response = participantes.Select(participanteModel => new ParticipantesGetResponse()
            {
                Id = participanteModel.Id,
                PalestraId = participanteModel.PalestraId,
                Nome = participanteModel.Nome,
                Email = participanteModel.Email,
                Telefone = participanteModel.Telefone
            });
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(statusCode: StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(statusCode: StatusCodes.Status201Created)]
        public async Task<ActionResult> PostAsync(ParticipantesPostRequest postRequest)
        {
            var unavailable = StatusCode(StatusCodes.Status503ServiceUnavailable);

            bool palestraExists;
            try
            {
                palestraExists = await _repository.PalestraExistsAsync(postRequest.PalestraId);
            }
            catch (SqlException _)
            {
                return unavailable;
            }

            if (!palestraExists)
            {
                return BadRequest();
            }

            var newParticipante = new ParticipanteModel()
            {
                Id = Guid.NewGuid(),
                PalestraId = postRequest.PalestraId,
                Nome = postRequest.Nome,
                Email = postRequest.Email,
                Telefone = postRequest.Telefone
            };
            bool successfulWrite;
            try
            {
                successfulWrite = await _repository.WriteAsync(newParticipante);
            }
            catch (SqlException _)
            {
                return unavailable;
            }

            if (!successfulWrite)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Created();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(statusCode: StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        public async Task<ActionResult> PutAsync(Guid id, ParticipantesPutRequest putRequest)
        {
            var unavailable = StatusCode(StatusCodes.Status503ServiceUnavailable);

            bool participanteExists;
            try
            {
                participanteExists = await _repository.ParticipanteExistsAsync(id);
            }
            catch (SqlException _)
            {
                return unavailable;
            }

            if (!participanteExists)
            {
                return NotFound();
            }

            bool palestraExists;
            try
            {
                palestraExists = await _repository.PalestraExistsAsync(putRequest.PalestraId);
            }
            catch (SqlException _)
            {
                return unavailable;
            }

            if (!palestraExists)
            {
                return BadRequest();
            }

            var updatedParticipante = new ParticipanteModel()
            {
                Id = id,
                PalestraId = putRequest.PalestraId,
                Nome = putRequest.Nome,
                Email = putRequest.Email,
                Telefone = putRequest.Telefone
            };
            bool successfulUpdate;
            try
            {
                successfulUpdate = await _repository.UpdateAsync(updatedParticipante);
            }
            catch (SqlException _)
            {
                return unavailable;
            }

            if (!successfulUpdate)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(statusCode: StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            var unavailable = StatusCode(StatusCodes.Status503ServiceUnavailable);

            bool participanteExists;
            try
            {
                participanteExists = await _repository.ParticipanteExistsAsync(id);
            }
            catch (SqlException _)
            {
                return unavailable;
            }

            if (!participanteExists)
            {
                return NotFound();
            }

            bool successfulDelete;
            try
            {
                successfulDelete = await _repository.DeleteAsync(id);
            }
            catch (SqlException _)
            {
                return unavailable;
            }

            if (!successfulDelete)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }
    }
}