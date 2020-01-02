using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Interfaces;

namespace WebApi.Implementations
{
    public class AnalysisAndTestsNamingMapper : INamingMapper
    {
        public string MapTestNameByAnalysisName(string analysisName)
        {
            switch (analysisName)
            {
                case "ФЕРРИТИН_COBAS":
                    return "Ферритин";
                case "ЖЕЛЕЗО_СЫВ_COBAS":
                case "ЖЕЛЕЗО_СЫВ_ХРОМОЛАБ":
                    return "Железо в сыворотке";
                case "ВИТАМИН_B12_ЕВРОТЕСТ":
                case "ВИТАМИН_В12_COBAS":
                    return "Витамин В12";
                case "ФОЛИЕВАЯ_КИСЛОТА_COBAS":
                    return "Фолат сыворотки";
                default:
                    throw new ArgumentOutOfRangeException(
                        $"Don't know how to map analysis with name {analysisName} with TestName");
            }
        }

        public string ChangeTestName(string testName)
        {
            switch (testName)
            {
                case "Концентрация":
                    return "Tрансферрин";
                case "Коэффициент насыщения трансферрина железом":
                    return "Насыщение трансферрина";
                default:
                    return testName;
            }
        }

        public string MapTestNameWithLoinc(string testName)
        {
            switch (testName)
            {
                case "Гемоглобин (HGB)":
                    return "718-7";
                case "Средн. сод. гемоглобина в эр-те (MCH)":
                    return "785-6";
                case "Средний объем эритроцита (MCV)":
                    return "787-2";
                case "Гематокрит (HCT)":
                    return "4544-3";
                case "Эритроциты (RBC)":
                    return "789-8";
                case "Tрансферрин":
                    return "3034-6";
                case "Насыщение трансферрина":
                    return "2502-3";
                case "Ферритин":
                    return "2276-4";
                case "Железо в сыворотке":
                    return "14798-3";
                case "Витамин В12":
                    return "2132-9";
                case "Фолат сыворотки":
                    return "14732-2";
                default:
                    throw new ArgumentOutOfRangeException(
                        $"Don't know how to map analysis with name {testName} with appropriate LOINC");
            }
        }
    }
}
