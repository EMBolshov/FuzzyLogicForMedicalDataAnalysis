﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FuzzyLogicMedicalCore.FHIR;
using FuzzyLogicMedicalCore.MedicalFuzzyDataModel;

namespace FuzzyLogicMedicalCore.ReportGeneration
{
    public class ReportGenerator
    {
        private readonly string _reportPath;


        public ReportGenerator(string reportPath)
        {
            _reportPath = reportPath;
        }

        public void GenerateReport(Patient patient, List<AnalysisResult> analysisResults, List<Diagnosis> diagnoses)
        {
            using (var file = File.AppendText(_reportPath))
            {
                var builder = new StringBuilder();
                builder.AppendLine($"Пациент: {patient.FirstName} {patient.MiddleName} {patient.LastName}");
                builder.AppendLine("Результаты анализов:");
                foreach (var analysis in analysisResults)
                {
                    builder.AppendLine($"Анализ: {analysis.AnalysisName}, Ваш показатель: {decimal.Round(analysis.CurrentValue, 2, MidpointRounding.AwayFromZero)}");
                    builder.AppendLine($"Ниже нормы на {decimal.Round(analysis.LowResult.Affiliation, 2, MidpointRounding.AwayFromZero)}%");
                    builder.AppendLine($"Соответствует норме на {decimal.Round(analysis.MidResult.Affiliation, 2, MidpointRounding.AwayFromZero)}%");
                    builder.AppendLine($"Выше нормы на {decimal.Round(analysis.HighResult.Affiliation, 2, MidpointRounding.AwayFromZero)}%");
                }

                builder.AppendLine("Вероятности диагнозов:");

                foreach (var diagnosis in diagnoses)
                {
                    builder.AppendLine($"Диагноз {diagnosis.Name}, вероятность {decimal.Round(diagnosis.Affiliation, 2, MidpointRounding.AwayFromZero)}%");
                }
                
                var report = builder.ToString();
                file.Write(report);
            }
        }
    }
}
