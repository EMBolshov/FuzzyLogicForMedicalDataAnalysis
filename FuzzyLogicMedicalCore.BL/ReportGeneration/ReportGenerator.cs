using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FuzzyLogicMedicalCore.BL.FHIR;
using FuzzyLogicMedicalCore.BL.MedicalFuzzyDataModel;

namespace FuzzyLogicMedicalCore.BL.ReportGeneration
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

                var isPositive = false;

                foreach (var diagnosis in diagnoses)
                {
                    var probability = decimal.Round(diagnosis.Affiliation, 2, MidpointRounding.AwayFromZero);
                    builder.AppendLine($"Диагноз {diagnosis.Name}, вероятность {probability}%");
                    if (probability > 0)
                    {
                        isPositive = true;
                    }
                }
                
                var report = builder.ToString();

                if (isPositive)
                {
                    file.Write(report);
                }
                else
                {
                    file.Dispose();
                    File.Delete(_reportPath);
                }
            }
        }

        public void GenerateStatistics(List<Patient> patients, List<Diagnosis> diagnoses)
        {
            using (var file = File.AppendText(_reportPath))
            {
                var builder = new StringBuilder();
                builder.AppendLine($"Всего пациентов: {patients.Count}");
                builder.AppendLine("Подозрения на диагнозы:");

                var diagnosisNames = diagnoses.Select(x => x.Name).Distinct().ToList();

                foreach (var diagnosisName in diagnosisNames)
                {
                    var diagnosisSetCount = diagnoses.Count(x => x.Name == diagnosisName && x.Affiliation > 0);
                    builder.AppendLine($"Подозрения на диагноз {diagnosisName} замечены у {diagnosisSetCount} пациентов.");
                }

                var report = builder.ToString();
                file.Write(report);
            }
        }
    }
}
