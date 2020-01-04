using System;
using System.IO;
using WebApi.Interfaces.Helpers;

namespace WebApi.Implementations.Helpers
{
    public class HtmlReportGenerator : IReportGenerator
    {
        public void GenerateReport(ReportModel model)
        {
            var path = @"C:\Users\ПК\source\repos\FuzzyLogicForMedicalDataAnalysis\WebApi\Templates\Report.cshtml";
            var template = System.Text.Encoding.UTF8.GetString(File.ReadAllBytes(path));
            //var returnedView = Engine.Razor.RunCompile(template, "report", typeof(ReportModel), model, null);
            //File.WriteAllText(Path.Combine("TestReports", $"{Guid.NewGuid()}.html"), returnedView);
        }
    }
}
