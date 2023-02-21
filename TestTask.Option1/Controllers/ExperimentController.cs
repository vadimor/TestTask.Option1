using Microsoft.AspNetCore.Mvc;
using System.Net;
using TestTask.Option1.Models.Response;
using TestTask.Option1.Services.Interfaces;

namespace TestTask.Option1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExperimentController : ControllerBase
    {
        const string experimentNameButtonColor = "button-color";
        const string experimentNamePriceChange = "price-change";
        const string defaultValue = "default";

        private readonly IExperimentManageService _experimentManageService;

        public ExperimentController(IExperimentManageService experimentManageService)
        {
            _experimentManageService = experimentManageService;
        }

        // This method returns statistics about the results of experiments

        [HttpGet]
        [Route("statistics")]
        [ProducesResponseType(typeof(IEnumerable<ExperimentValueResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Statistics()
        {

            return Ok(await _experimentManageService.GetStaticticsAsync());
        }

        // Next methods return one of several options for experiment values
        // One device token always has the same experiment value for every request 


        [HttpGet]
        [Route("button-color")]
        [ProducesResponseType(typeof(ExperimentValueResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ButtonColor(string deviceToken)
        {
            var result = await _experimentManageService.GetExperimentValueAsync(experimentNameButtonColor, deviceToken);
            var value = result is null ? defaultValue : result.Value;
            return Ok(new ExperimentValueResponse
            {
                Key = experimentNameButtonColor,
                Value = value
            });
        }



        [HttpGet]
        [Route("price-change")]
        [ProducesResponseType(typeof(ExperimentValueResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> PriceChange(string deviceToken)
        {
            var result = await _experimentManageService.GetExperimentValueAsync(experimentNamePriceChange, deviceToken);
            var value = result is null ? defaultValue : result.Value;
            return Ok(new ExperimentValueResponse
            {
                Key = experimentNameButtonColor,
                Value = value
            });
        }
    }
}
