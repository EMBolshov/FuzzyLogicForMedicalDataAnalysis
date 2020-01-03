using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces;
using WebApi.Interfaces.Learning;

namespace WebApi.Controllers
{
    /// <summary>
    /// TODO
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LearningController : ControllerBase
    {
        private readonly ILearningProcessor _learningProcessor;

        public LearningController(ILearningProcessor learningProcessor)
        {
            _learningProcessor = learningProcessor;
        }

        /// <summary>
        /// Learn
        /// </summary>
        [HttpGet("Learn")]
        public void Learn()
        {
            _learningProcessor.ProcessForAllPatients();
        }
    }
}
