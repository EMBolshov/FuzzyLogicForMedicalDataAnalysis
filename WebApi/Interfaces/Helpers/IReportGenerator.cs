using WebApi.Implementations.Helpers;

namespace WebApi.Interfaces.Helpers
{
    public interface IReportGenerator
    {
        //TODO: Return byte array
        void GenerateReport(ReportModel model);
    }
}
