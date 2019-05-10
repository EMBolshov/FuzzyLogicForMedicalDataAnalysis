﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using FuzzyLogicMedicalCore.FHIR;
using FuzzyLogicMedicalCore.FuzzyLogic;
using FuzzyLogicMedicalCore.MedicalFuzzyDataModel;
using Newtonsoft.Json;

namespace FuzzyLogicTestingConsole
{
    public class MedicalDataFakeManager
    {
        private readonly string _pathToFolder;

        public MedicalDataFakeManager()
        {
            _pathToFolder = ConfigurationManager.AppSettings["PathToFolderWithGeneratedData"];
        }

        public List<Patient> GetFakePatientList()
        {
            List<Patient> patientList;
            using (var file = File.OpenText(_pathToFolder + "Patients.json"))
            {
                var serializer = new JsonSerializer();
                patientList = (List<Patient>) serializer.Deserialize(file, typeof(List<Patient>));
            }

            return patientList;
        }

        public List<AnalysisResult> GetFakeAnalysisResults(Guid patientGuid)
        {
            List<AnalysisResult> resultList;
            using (var file = File.OpenText(_pathToFolder + $"Analyses_{patientGuid}.json"))
            {
                var serializer = new JsonSerializer();
                resultList = (List<AnalysisResult>)serializer.Deserialize(file, typeof(List<AnalysisResult>));
            }

            return resultList;
        }

        public List<Diagnosis> GetFakeDiagnoses(Guid patientGuid)
        {
            List<Diagnosis> diagnoses;
            using (var file = File.OpenText(_pathToFolder + $"Diagnoses.json"))
            {
                var serializer = new JsonSerializer();
                diagnoses = (List<Diagnosis>)serializer.Deserialize(file, typeof(List<Diagnosis>));
            }
            diagnoses.ForEach(x => x.PatientGuid = patientGuid);
            
            return diagnoses;
        }
       
        public List<Rule> GetAllFakeRules()
        {
            List<Rule> diagnoses;
            using (var file = File.OpenText(_pathToFolder + $"Rules.json"))
            {
                var serializer = new JsonSerializer();
                diagnoses = (List<Rule>)serializer.Deserialize(file, typeof(List<Rule>));
            }

            return diagnoses;
        }

        public void GetPowerOfRules(List<Rule> rules, List<AnalysisResult> fakeResults)
        {
            foreach (var rule in rules)
            {
                rule.GetPower(fakeResults);
            }
        }

        public void GetDiagnosisAffiliation(List<Diagnosis> diagnoses, Rule rule)
        {
            foreach (var diagnosis in diagnoses)
            {
                var outputTerms = rule.OutputTerms.Split(';').ToList();
                
                foreach (var outputTerm in outputTerms)
                {
                    if (diagnosis.Name == outputTerm)
                    {
                        diagnosis.Rules.Add(rule);
                    }
                }
            }
        }
    }
}
