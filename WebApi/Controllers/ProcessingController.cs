using Microsoft.AspNetCore.Mvc;
using POCO.Domain.Dto;
using WebApi.Interfaces.MainProcessing;

namespace WebApi.Controllers
{
    /// <summary>
    /// TODO
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessingController : ControllerBase
    {
        private readonly IMainProcessor _processor;

        public ProcessingController(IMainProcessor processor)
        {
            _processor = processor;
        }

        /// <summary>
        /// Process for all
        /// </summary>
        [HttpGet("ProcessForAllPatients")]
        public void ProcessForAllPatients()
        {
            _processor.ProcessForAllPatients();
        }
    }
}
