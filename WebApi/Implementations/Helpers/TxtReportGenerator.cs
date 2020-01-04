using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using WebApi.Interfaces.Helpers;
using Guid = System.Guid;

namespace WebApi.Implementations.Helpers
{
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public class TxtReportGenerator : IReportGenerator
    {
        public void GenerateReport(ReportModel model)
        {
            var newPath = Path.Combine(model.Path, $"_{Guid.NewGuid()}.txt");
            using (var file = File.AppendText(newPath))
            {
                var builder = new StringBuilder();
                builder.AppendLine($"Пациент: {model.Patient.FirstName} {model.Patient.MiddleName} {model.Patient.LastName}");
                builder.AppendLine("Результаты анализов:");
                foreach (var analysis in model.AnalysisResults)
                {
                    builder.AppendLine($"Тест: {analysis.TestName}, LOINC: {analysis.Loinc}");
                    var resultValue = decimal.Round(analysis.Entry, 2, MidpointRounding.AwayFromZero);
                    var referenceLow = decimal.Round(analysis.ReferenceLow, 2, MidpointRounding.AwayFromZero);
                    var referenceHigh = decimal.Round(analysis.ReferenceHigh, 2, MidpointRounding.AwayFromZero);
                    builder.AppendLine($"Ваш показатель: {resultValue} ед. изм.");
                    var resultValueInterpretation = GetResultValueInterpretation(resultValue, referenceLow, referenceHigh);
                    builder.AppendLine(resultValueInterpretation);
                    builder.AppendLine($"Нижняя граница нормы: {referenceLow} ед. изм.");
                    builder.AppendLine($"Верхняя граница нормы: {referenceHigh} ед. изм.");
                }

                builder.AppendLine("Вероятности диагнозов:");

                var positiveResult = false;

                foreach (var diagnosis in model.Diagnoses)
                {
                    var processedResult = model.ProcessedResults.First(x => x.DiagnosisGuid == diagnosis.Guid);
                    var probability = decimal.Round(processedResult.Value, 2, MidpointRounding.AwayFromZero);
                    builder.AppendLine($"Диагноз {diagnosis.Name}, код МКБ-10 {diagnosis.MkbCode}");
                    builder.AppendLine($"Вероятность {probability} относительных единиц.");
                    positiveResult = probability > 0 || positiveResult;
                }

                var report = builder.ToString();
                if (positiveResult)
                {
                    file.Write(report);
                }
                else
                {
                    file.Dispose();
                    File.Delete(Path.Combine(newPath));
                }
            }
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
