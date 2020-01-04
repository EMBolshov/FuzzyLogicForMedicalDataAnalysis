﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using POCO.Domain;
using WebApi.Interfaces.Helpers;
using Guid = System.Guid;

namespace WebApi.Implementations.Helpers
{
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public class TxtReportGenerator : IReportGenerator
    {
        public void GenerateReport(ProcessedResult processedResult, Patient patient, List<AnalysisResult> analysisResults, List<Diagnosis> diagnoses, string path)
        {
            var newPath = Path.Combine(path, $"_{Guid.NewGuid()}.txt");
            using (var file = File.AppendText(newPath))
            {
                var builder = new StringBuilder();
                builder.AppendLine($"Пациент: {patient.FirstName} {patient.MiddleName} {patient.LastName}");
                builder.AppendLine("Результаты анализов:");
                foreach (var analysis in analysisResults)
                {
                    builder.AppendLine($"Тест: {analysis.TestName}, LOINC: {analysis.Loinc}");
                    builder.AppendLine($"Ваш показатель: {decimal.Round(analysis.Entry, 2, MidpointRounding.AwayFromZero)} ед. изм.");
                    builder.AppendLine($"Нижняя граница нормы: {decimal.Round(analysis.ReferenceLow, 2, MidpointRounding.AwayFromZero)} ед. изм.");
                    builder.AppendLine($"Верхняя граница нормы: {decimal.Round(analysis.ReferenceHigh, 2, MidpointRounding.AwayFromZero)} ед. изм.");
                }

                builder.AppendLine("Вероятности диагнозов:");

                var positiveResult = false;

                foreach (var diagnosis in diagnoses)
                {
                    var probability = decimal.Round(processedResult.Value, 2, MidpointRounding.AwayFromZero);
                    builder.AppendLine($"Диагноз {diagnosis.Name}, код МКБ-10 {diagnosis.MkbCode}");
                    var thisDiagnosisProbability = processedResult.DiagnosisGuid == diagnosis.Guid ? probability : 0m;
                    builder.AppendLine($"Вероятность {thisDiagnosisProbability} относительных единиц.");
                    positiveResult = thisDiagnosisProbability > 0 && diagnosis.Name == "Анемия хронических заболеваний";
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
    }
}
