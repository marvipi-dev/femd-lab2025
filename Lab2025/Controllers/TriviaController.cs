using System.Net;
using System.Web;
using Lab2025.Views;
using Microsoft.AspNetCore.Mvc;

namespace Lab2025.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TriviaController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TriviaController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        
        [HttpGet]
        [ProducesResponseType(statusCode: StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(statusCode: StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<TriviaQuestion>> GetAsync()
        {
            var httpClient = _httpClientFactory.CreateClient();

            TriviaGetResponse? response;
            try
            {
                response = await httpClient.GetFromJsonAsync<TriviaGetResponse>("https://opentdb.com/api.php?amount=1");
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.TooManyRequests)
            {
                return StatusCode(StatusCodes.Status429TooManyRequests);
            }
            catch (HttpRequestException _)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }

            if (response == null)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }
            
            var dirtyTriviaQuestion = response.Results.First();
            var cleanTriviaQuestion = new TriviaQuestion()
            {
                Question = HttpUtility.HtmlDecode(dirtyTriviaQuestion.Question),
                CorrectAnswer = HttpUtility.HtmlDecode(dirtyTriviaQuestion.CorrectAnswer)
            };
            
            return Ok(cleanTriviaQuestion);
        }
    }
}
