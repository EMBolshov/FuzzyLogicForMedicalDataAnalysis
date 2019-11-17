using System;
using Microsoft.AspNetCore.Mvc;
using POCO.Domain.Dto;
using WebApi.Interfaces;

namespace WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AnalysisResultLoadController : ControllerBase
    {
        private readonly IAnalysisResultProvider _analysisResultProvider;

        public AnalysisResultLoadController(IAnalysisResultProvider analysisResultProvider)
        {
            _analysisResultProvider = analysisResultProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        [HttpGet("LoadAnalysisResultsFromFile")]
        public void LoadAnalysisResultsFromFile(string path)
        {
            _analysisResultProvider.LoadAnalysisResultsFromFile(path);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        [HttpGet("LoadPatientsFromFile")]
        public void LoadPatientsFromFile(string path)
        {
            _analysisResultProvider.LoadPatientsFromFile(path);
        }
    }
}
