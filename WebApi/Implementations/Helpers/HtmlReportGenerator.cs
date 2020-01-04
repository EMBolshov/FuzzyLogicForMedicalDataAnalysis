using System;
using System.IO;
using System.Linq;
using System.Text;
using WebApi.Interfaces.Helpers;

namespace WebApi.Implementations.Helpers
{
    public class HtmlReportGenerator : IReportGenerator
    {
        public void GenerateReport(ReportModel model)
        {
            var generatedHtml = HtmlInit();
            generatedHtml += GetHeader(model);
            generatedHtml += GetBody(model);
            generatedHtml += GetFooter();

            File.WriteAllText(Path.Combine("TestReports", $"{Guid.NewGuid()}.html"), generatedHtml);
        }

        private string HtmlInit()
        {
            var builder = new StringBuilder();
            builder.AppendLine("<!DOCTYPE html>");
            builder.AppendLine("<html lang=\"ru\")");
            var rendered = builder.ToString();
            rendered += GetCss();
            return rendered;
        }

        private string GetCss()
        {
            return "";
        }

        private string GetHeader(ReportModel model)
        {
            var builder = new StringBuilder();
            builder.AppendLine("<head>");
            builder.AppendLine("<meta charset=\"UTF-8\">");
            builder.AppendLine($"<title>{model.Patient.FirstName} {model.Patient.MiddleName} {model.Patient.LastName}</title>");
            builder.AppendLine("</head>");
            var rendered = builder.ToString();
            return rendered;
        }

        private string GetBody(ReportModel model)
        {
            var builder = new StringBuilder();
            builder.AppendLine("<body>");
            builder.AppendLine("<p>");
            builder.AppendLine($"<h2>{model.Patient.FirstName} {model.Patient.MiddleName} {model.Patient.LastName}</h2>");
            builder.AppendLine("</p>");
            builder.AppendLine("<br><h3>Результаты анализов:</h3>");
            builder.AppendLine("<ul>");
            foreach (var analysis in model.AnalysisResults)
            {
                builder.AppendLine("<li>");
                builder.AppendLine($"Тест: {analysis.TestName}, LOINC: {analysis.Loinc}");
                var resultValue = decimal.Round(analysis.Entry, 2, MidpointRounding.AwayFromZero);
                var referenceLow = decimal.Round(analysis.ReferenceLow, 2, MidpointRounding.AwayFromZero);
                var referenceHigh = decimal.Round(analysis.ReferenceHigh, 2, MidpointRounding.AwayFromZero);
                builder.AppendLine($"Ваш показатель: {resultValue} ед. изм.");
                var resultValueInterpretation = GetResultValueInterpretation(resultValue, referenceLow, referenceHigh);
                builder.AppendLine(resultValueInterpretation);
                builder.AppendLine($"Нижняя граница нормы: {referenceLow} ед. изм.");
                builder.AppendLine($"Верхняя граница нормы: {referenceHigh} ед. изм.");
                builder.AppendLine("</li>");
            }
            builder.AppendLine("</ul>");
            builder.AppendLine("<br>");
            builder.AppendLine("<h3>Возможные диагнозы:</h3>");
            builder.AppendLine("<ul>");
            foreach (var diagnosis in model.Diagnoses)
            {
                var processedResult = model.ProcessedResults.First(x => x.DiagnosisGuid == diagnosis.Guid);
                var probability = decimal.Round(processedResult.Value, 2, MidpointRounding.AwayFromZero);
                builder.AppendLine("<li>");
                builder.AppendLine($"Диагноз {diagnosis.Name}, код МКБ-10 {diagnosis.MkbCode}");
                builder.AppendLine($"Вероятность {probability} относительных единиц.");
                builder.AppendLine("</li>");
            }
            builder.AppendLine("</ul>");
            builder.AppendLine("</body>");
            var rendered = builder.ToString();
            return rendered;
        }

        private string GetFooter()
        {
            return "</html>";
        }

        private string GetResultValueInterpretation(decimal resultValue, decimal referenceLow, decimal referenceHigh)
        {
            if (resultValue < referenceLow)
            {
                return "Ниже нормы";
            }
            return resultValue < referenceHigh ? "В норме" : "Выше нормы";
        }
    }
}
