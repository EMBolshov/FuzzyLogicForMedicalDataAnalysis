using WebApi.Implementations.Helpers;

namespace WebApi.Interfaces.Helpers
{
    //TODO: DTO for ReportGenerator
    public interface IReportGenerator
    {
        void GenerateReport(ReportModel model);
    }
}
