using System.Collections.Generic;
using POCO.Domain;

namespace WebApi.Interfaces.MainProcessing
{
    public interface IProcessedResultProvider
    {
        void SaveProcessedResult(ProcessedResult result);
        List<ProcessedResult> GetAllPositiveResults();
    }
}
