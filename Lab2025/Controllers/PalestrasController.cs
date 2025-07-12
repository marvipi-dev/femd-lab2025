using Lab2025.Data;
using Lab2025.Models;
using Lab2025.Views;
using Microsoft.AspNetCore.Mvc;

namespace Lab2025.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PalestrasController : ControllerBase
    {
        private readonly IPalestrasRepository _palestrasRepository;

        public PalestrasController(IPalestrasRepository palestrasRepository)
        {
            _palestrasRepository = palestrasRepository;
        }
        
        [HttpGet]
        [ProducesResponseType(statusCode: StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PalestrasGetResponse>>> Get()
        {
            IEnumerable<PalestraModel> palestras;
            try
            {
                palestras = await _palestrasRepository.ReadAsync();
            }
            catch
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
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
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
            var unavailable = StatusCode(StatusCodes.Status503ServiceUnavailable);
            
            try
            {
                successfulWrite = await _palestrasRepository.WriteAsync(newPalestra);
            }
            catch
            {
                return unavailable;
            }

            if (!successfulWrite)
            {
                return unavailable;
            }

            return Ok();
        }
    }
}
