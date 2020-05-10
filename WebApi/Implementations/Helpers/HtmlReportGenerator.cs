using System;
using System.IO;
using System.Linq;
using System.Text;
using POCO.Domain;
using WebApi.Interfaces.Helpers;

namespace WebApi.Implementations.Helpers
{
    //TODO: Builder
    public class HtmlReportGenerator : IReportGenerator
    {
        //TODO: Отдельный метод SaveReport
        public void GenerateReport(ReportModel model)
        {
            var generatedHtml = HtmlInit();
            generatedHtml += GetHeader(model);
            generatedHtml += GetBody(model);

            //File.WriteAllText(Path.Combine("TestReports", $"{Guid.NewGuid()}.html"), generatedHtml);
        }

        private string HtmlInit()
        {
            var builder = new StringBuilder();
            builder.AppendLine("<!DOCTYPE html>");
            builder.AppendLine("<html lang=\"ru\">");
            var rendered = builder.ToString();
            return rendered;
        }

        private string GetCss()
        {
            var builder = new StringBuilder();
            builder.AppendLine("<style>");
            builder.AppendLine("body { " +
                               "padding: 0; " +
                               "margin: 0; " +
                               "font-family: Arial, sans-serif; " +
                               "min-height: 100vh; " +
                               "width: 100%; " +
                               "display: table;}");
            builder.AppendLine("p {color: read;}");
            builder.AppendLine("h3 {color: blue;}");
            builder.AppendLine("table { " +
                               "font-family: arial, sans-serif; " +
                               "border-collapse: collapse; " +
                               "width: 100%; " +
                               "background-color: rgba(255, 255, 255, 1.0);}");

            builder.AppendLine("td, th { " +
                               "border: 1px solid #dddddd; " +
                               "text-align: left; " +
                               "padding: 8px;}");

            builder.AppendLine("tr:nth-child(even) { " +
                               "background-color: #dddddd; }");
            builder.AppendLine("main { "+
                               "display: table-row; " + 
                               "height: 100%;}");
            builder.AppendLine("</style>");
            var rendered = builder.ToString();
            return rendered;
        }

        private string GetHeader(ReportModel model)
        {
            var builder = new StringBuilder();
            builder.AppendLine("<head>");
            builder.AppendLine("<meta charset=\"UTF-8\">");
            builder.AppendLine(GetCss());
            builder.AppendLine("</head>");
            var rendered = builder.ToString();
            return rendered;
        }

        private string GetBody(ReportModel model)
        {
            var builder = new StringBuilder();
            builder.AppendLine("<body>");
            builder.AppendLine("<main role=\"main\">");
            builder.AppendLine("<p>");
            builder.AppendLine($"<h2 align=\"center\">{model.Patient.FirstName} {model.Patient.MiddleName} {model.Patient.LastName}</h2>");
            builder.AppendLine("</p>");
            builder.AppendLine("<br><h3 style=\"margin-left: 40px;\"> Результаты анализов:</h3>");
            builder.AppendLine("<div>");
            builder.AppendLine("<table>");
            builder.AppendLine("<tr> " +
                               "<th>Тест: </th> " +
                               "<th>LOINC: </th> " +
                               "<th>Ваш показатель: </th> " +
                               "<th>Интерпретация: </th> " +
                               "<th>Нижняя граница нормы: </th> " +
                               "<th>Верхняя граница нормы: </th> </tr>");
            foreach (var analysis in model.AnalysisResults)
            {
                builder.AppendLine("<tr>");
                builder.AppendLine($"<td>{analysis.TestName}</td>");
                builder.AppendLine($"<td>{ analysis.Loinc}</td>");
                var resultValue = decimal.Round(analysis.Entry, 2, MidpointRounding.AwayFromZero);
                var referenceLow = decimal.Round(analysis.ReferenceLow, 2, MidpointRounding.AwayFromZero);
                var referenceHigh = decimal.Round(analysis.ReferenceHigh, 2, MidpointRounding.AwayFromZero);
                builder.AppendLine($"<td>{resultValue} ед. изм.</td>");
                var resultValueInterpretation = GetResultValueInterpretation(resultValue, referenceLow, referenceHigh);
                var colorTag = GetColorTagForInterpretation(resultValueInterpretation);
                builder.AppendLine($"<td {colorTag}>{resultValueInterpretation}</td>");
                builder.AppendLine($"<td>{referenceLow} ед. изм.</td>");
                builder.AppendLine($"<td>{referenceHigh} ед. изм.</td>");
                builder.AppendLine("</tr>");
            }
            builder.AppendLine("</table>");
            builder.AppendLine("</div>");
            builder.AppendLine("<br>");
            builder.AppendLine("<h3 style=\"margin-left: 40px;\">Возможные диагнозы:</h3>");
            builder.AppendLine("<div>");
            builder.AppendLine("<table>");
            builder.AppendLine("<tr> " +
                               "<th>Диагноз: </th> " +
                               "<th>Код МКБ-10: </th> " +
                               "<th>Степень уверенности*: </th></tr>");
            foreach (var diagnosis in model.Diagnoses)
            {
                var reorderedResults = model.ProcessedResults.OrderByDescending(x => x.Value).ToList();
                var processedResult = reorderedResults.First(x => x.DiagnosisGuid == diagnosis.Guid);
                var probability = decimal.Round(processedResult.Value, 2, MidpointRounding.AwayFromZero);
                builder.AppendLine("<tr>");
                builder.AppendLine($"<td> {diagnosis.Name} </td>");
                builder.AppendLine($"<td> {diagnosis.MkbCode} </td>");
                builder.AppendLine($"<td> {probability} отн. ед. </td>");
                builder.AppendLine("</tr>");
            }
            builder.AppendLine("</table>");
            builder.AppendLine("</div>");
            builder.AppendLine("</main>");
            builder.AppendLine(GetFooter());
            builder.AppendLine("</body>");
            builder.AppendLine("</html>");
            var rendered = builder.ToString();
            return rendered;
        }

        private string GetFooter()
        {
            return "<footer role=\"contentinfo\">" +
                   "<h5>* Степень уверенности рассчитывается относительно других возможных диагнозов, " +
                   "предоставленных в списке</h5></footer>";
        }

        private string GetResultValueInterpretation(decimal resultValue, decimal referenceLow, decimal referenceHigh)
        {
            if (resultValue < referenceLow)
            {
                return "Ниже нормы";
            }
            return resultValue < referenceHigh ? "В норме" : "Выше нормы";
        }

        private string GetColorTagForInterpretation(string value)
        {
            switch (value)
            {
                case "Ниже нормы":
                    return " style=\"color: blue; \"";
                case "Выше нормы":
                    return " style=\"color: red; \"";
                default:
                    return " style=\"color: green; \"";
            }
        }
    }
}
