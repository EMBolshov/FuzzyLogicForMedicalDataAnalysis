#pragma checksum "C:\Users\ПК\source\repos\FuzzyLogicForMedicalDataAnalysis\WebApi\Templates\Report.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "42444bb80815af7cd257334c934cb7fe3f3a09b7"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Templates_Report), @"mvc.1.0.view", @"/Templates/Report.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Templates/Report.cshtml", typeof(AspNetCore.Templates_Report))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"42444bb80815af7cd257334c934cb7fe3f3a09b7", @"/Templates/Report.cshtml")]
    public class Templates_Report : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 74, true);
            WriteLiteral("\r\n<!DOCTYPE html>\r\n<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">\r\n");
            EndContext();
            BeginContext(74, 85, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("head", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "0d7a5e0ccca1406f896188ea1ed6348e", async() => {
                BeginContext(80, 43, true);
                WriteLiteral("\r\n    <meta charset=\"utf-8\" />\r\n    <title>");
                EndContext();
                BeginContext(124, 18, false);
#line 6 "C:\Users\ПК\source\repos\FuzzyLogicForMedicalDataAnalysis\WebApi\Templates\Report.cshtml"
      Write(Model.Patient.Guid);

#line default
#line hidden
                EndContext();
                BeginContext(142, 10, true);
                WriteLiteral("</title>\r\n");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(159, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(161, 2396, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "25149a35dbf24177a4e6bfa089fb9300", async() => {
                BeginContext(167, 6, true);
                WriteLiteral("\r\n<h2>");
                EndContext();
                BeginContext(174, 23, false);
#line 9 "C:\Users\ПК\source\repos\FuzzyLogicForMedicalDataAnalysis\WebApi\Templates\Report.cshtml"
Write(Model.Patient.FirstName);

#line default
#line hidden
                EndContext();
                BeginContext(197, 1, true);
                WriteLiteral(" ");
                EndContext();
                BeginContext(199, 24, false);
#line 9 "C:\Users\ПК\source\repos\FuzzyLogicForMedicalDataAnalysis\WebApi\Templates\Report.cshtml"
                        Write(Model.Patient.MiddleName);

#line default
#line hidden
                EndContext();
                BeginContext(223, 1, true);
                WriteLiteral(" ");
                EndContext();
                BeginContext(225, 22, false);
#line 9 "C:\Users\ПК\source\repos\FuzzyLogicForMedicalDataAnalysis\WebApi\Templates\Report.cshtml"
                                                  Write(Model.Patient.LastName);

#line default
#line hidden
                EndContext();
                BeginContext(247, 59, true);
                WriteLiteral("</h2>\r\n<br/>\r\n    <h3>Результаты анализов:</h3>\r\n    <ul>\r\n");
                EndContext();
#line 13 "C:\Users\ПК\source\repos\FuzzyLogicForMedicalDataAnalysis\WebApi\Templates\Report.cshtml"
          
            foreach (var analysisResult in Model.AnalysisResults)
            {
                var resultValue = decimal.Round(analysisResult.Entry, 2, MidpointRounding.AwayFromZero);
                var referenceLow = decimal.Round(analysisResult.ReferenceLow, 2, MidpointRounding.AwayFromZero);
                var referenceHigh = decimal.Round(analysisResult.ReferenceHigh, 2, MidpointRounding.AwayFromZero);

                var resultInterpretation = "";
                if (resultValue < referenceLow)
                {
                    resultInterpretation = "Ниже нормы";
                }
                else if (resultValue < referenceHigh)
                {
                    resultInterpretation = "В норме";
                }
                else
                {
                    resultInterpretation = "Выше нормы";
                }

                var formattedResult = $"Тест: {analysisResult.TestName}, LOINC: {analysisResult.Loinc}.";
                

#line default
#line hidden
                BeginContext(1323, 15, false);
#line 35 "C:\Users\ПК\source\repos\FuzzyLogicForMedicalDataAnalysis\WebApi\Templates\Report.cshtml"
           Write(formattedResult);

#line default
#line hidden
                EndContext();
#line 35 "C:\Users\ПК\source\repos\FuzzyLogicForMedicalDataAnalysis\WebApi\Templates\Report.cshtml"
                                
                formattedResult = $"Ваш показатель: {resultValue} ед. изм.; \n";
                

#line default
#line hidden
                BeginContext(1439, 15, false);
#line 37 "C:\Users\ПК\source\repos\FuzzyLogicForMedicalDataAnalysis\WebApi\Templates\Report.cshtml"
           Write(formattedResult);

#line default
#line hidden
                EndContext();
#line 37 "C:\Users\ПК\source\repos\FuzzyLogicForMedicalDataAnalysis\WebApi\Templates\Report.cshtml"
                                
                formattedResult = $"Нижняя граница нормы: {referenceLow} ед. изм.;";
                

#line default
#line hidden
                BeginContext(1559, 15, false);
#line 39 "C:\Users\ПК\source\repos\FuzzyLogicForMedicalDataAnalysis\WebApi\Templates\Report.cshtml"
           Write(formattedResult);

#line default
#line hidden
                EndContext();
#line 39 "C:\Users\ПК\source\repos\FuzzyLogicForMedicalDataAnalysis\WebApi\Templates\Report.cshtml"
                                
                formattedResult = $"Верхняя граница нормы: {referenceHigh} ед. изм.";
                

#line default
#line hidden
                BeginContext(1680, 15, false);
#line 41 "C:\Users\ПК\source\repos\FuzzyLogicForMedicalDataAnalysis\WebApi\Templates\Report.cshtml"
           Write(formattedResult);

#line default
#line hidden
                EndContext();
#line 41 "C:\Users\ПК\source\repos\FuzzyLogicForMedicalDataAnalysis\WebApi\Templates\Report.cshtml"
                                
                formattedResult = $"Ваши показатели {resultInterpretation}";
                

#line default
#line hidden
                BeginContext(1792, 15, false);
#line 43 "C:\Users\ПК\source\repos\FuzzyLogicForMedicalDataAnalysis\WebApi\Templates\Report.cshtml"
           Write(formattedResult);

#line default
#line hidden
                EndContext();
#line 43 "C:\Users\ПК\source\repos\FuzzyLogicForMedicalDataAnalysis\WebApi\Templates\Report.cshtml"
                                
            }
        

#line default
#line hidden
                BeginContext(1835, 54, true);
                WriteLiteral("    </ul>\r\n<br/>\r\n<h3>Возможные диагнозы:</h3>\r\n<ul>\r\n");
                EndContext();
#line 50 "C:\Users\ПК\source\repos\FuzzyLogicForMedicalDataAnalysis\WebApi\Templates\Report.cshtml"
      
        foreach (var diagnosis in Model.Diagnoses)
        {
            foreach (var processedResult in Model.ProcessedResults)
            {
                if (processedResult.DiagnosisGuid == diagnosis.Guid)
                {
                    var probability = decimal.Round(processedResult.Value, 2, MidpointRounding.AwayFromZero);
                    var formattedResult = $"Диагноз {diagnosis.Name}, код МКБ-10 {diagnosis.MkbCode}" +
                                          $"Вероятность {probability} относительных единиц.";

#line default
#line hidden
                BeginContext(2444, 24, true);
                WriteLiteral("                    <li>");
                EndContext();
                BeginContext(2469, 15, false);
#line 60 "C:\Users\ПК\source\repos\FuzzyLogicForMedicalDataAnalysis\WebApi\Templates\Report.cshtml"
                   Write(formattedResult);

#line default
#line hidden
                EndContext();
                BeginContext(2484, 7, true);
                WriteLiteral("</li>\r\n");
                EndContext();
#line 61 "C:\Users\ПК\source\repos\FuzzyLogicForMedicalDataAnalysis\WebApi\Templates\Report.cshtml"
                }
            }
        }
    

#line default
#line hidden
                BeginContext(2543, 7, true);
                WriteLiteral("</ul>\r\n");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(2557, 11, true);
            WriteLiteral("\r\n</html>\r\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
