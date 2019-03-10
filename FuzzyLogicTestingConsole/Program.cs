using System;
using FuzzyLogicMedicalCore;
using FuzzyLogicTestingConsole.Manager;
using MedicalDataCore;

namespace FuzzyLogicTestingConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //todo remove this
            /* Есть несколько различных медицинских показателей - результаты выполнения анализов.
             * Подготовлена база с описанием референесных значений для этих показателей,
             * составленная с учетом возраста и пола пациента.
             * Есть список результатов исследований пациентов с информацией по пациенту - возраст, пол.
             * По результатам исследования конкретного пациента определяется вероятность постановки
             * диагноза.
             */

            //TODO make this test

            var patientId = new Guid();

            var temperature = new Observation()
            {
                IndicationName = "Temperature",
                PatientReference = patientId,
                ReferenceHigh = 37.0m,
                ReferenceLow = 35.0m,
                Value = 36.6m
            };

            //I can get reference values from database or with analysis results in FHIR resource

            var iron = new Observation()
            {
                IndicationName = "Iron in blood",
                PatientReference = patientId,
                ReferenceHigh = 1.0m,
                ReferenceLow = 0.0m,
                Value = 1.1m
            };

            var whiteBloodCells = new Observation()
            {
                IndicationName = "White blood cells",
                PatientReference = patientId,
                ReferenceHigh = 10.0m,
                ReferenceLow = 0.0m,
                Value = 5m
            }; 

            var medicalDataManager = new MedicalDataManager();

            //calculate affilation to each term in set
            //in fuzzy51 assembler example used look-up table
            //in my work I can calculate affiliation by myself using function kinda y = f(x)
            //use rule engine to make decision for probability of diagnosis

            var bloodCancer = new Diagnosis()
            {
                Name = "Blood cancer"
            };

            var anemia = new Diagnosis()
            {
                Name = "Anemia"
            };

            var flu = new Diagnosis()
            {
                Name = "Flu"
            };

        }
    }
}
