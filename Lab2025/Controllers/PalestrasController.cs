using Lab2025.Data;
using Lab2025.Models;
using Lab2025.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Lab2025.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PalestrasController : ControllerBase
    {
        private readonly IPalestrasRepository _repository;

        public PalestrasController(IPalestrasRepository repository)
        {
            _repository = repository;
        }
        
        [HttpGet]
        [ProducesResponseType(statusCode: StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PalestrasGetResponse>>> Get()
        {
            IEnumerable<PalestraModel> palestras;
            try
            {
                palestras = await _repository.ReadAsync();
            }
            catch (SqlException _)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }

            var response = palestras.Select(palestraModel => new PalestrasGetResponse()
            {
                Id = palestraModel.Id,
                Titulo = palestraModel.Titulo,
                Descricao = palestraModel.Descricao,
                DataHora = palestraModel.DataHora
            });
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(statusCode: StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(statusCode: StatusCodes.Status201Created)]
        public async Task<ActionResult> Post(PalestrasPostRequest postRequest)
        {
            bool successfulWrite;
            var newPalestra = new PalestraModel()
            {
                Id = Guid.NewGuid(),
                Titulo = postRequest.Titulo,
                Descricao = postRequest.Descricao,
                DataHora = postRequest.DataHora.ToUniversalTime()
            };
            
            try
            {
                successfulWrite = await _repository.WriteAsync(newPalestra);
            }
            catch (SqlException _)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }

            if (!successfulWrite)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Created();
        }
    }
}
