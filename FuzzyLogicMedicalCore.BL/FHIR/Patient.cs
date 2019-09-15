using System;
using System.Collections.Generic;
using System.Data;
using FuzzyLogicMedicalCore.BL.FuzzyLogic;

namespace FuzzyLogicMedicalCore.BL.FHIR
{
    public class Patient
    {        
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public List<AnalysisResult> AnalysisResults { get; set; }
        public List<Diagnosis> Diagnoses { get; set; }

        public Patient()
        {
            AnalysisResults = new List<AnalysisResult>();
        }
    }
}
