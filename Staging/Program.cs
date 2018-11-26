using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeBinder.Java;
using CodeBinder;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using CodeBinder.JNI;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            MSBuildLocator.RegisterDefaults();
            MSBuildWorkspace workspace = MSBuildWorkspace.Create();

            GeneratorArgs genargs = new GeneratorArgs();

            //Solution solution = workspace.OpenSolutionAsync(@"D:\Staging\Euronovate\ENLibPdf\ENLibPdfNet.sln").Result;
            //var conv = SolutionConverter.CreateFor<CSToJavaConversion>(solution);
            //Project project = workspace.OpenProjectAsync(@"D:\Staging\TestJava\ENLibPdfNet.csproj").Result;
            //Project project = workspace.OpenProjectAsync(@"D:\Staging\TestJava\ENLibPdfNetLite.csproj").Result;
            Project project = workspace.OpenProjectAsync(@"D:\Staging\Euronovate\ENLibPdf\ENLibPdfNet\ENLibPdfNet.csproj").Result;


            if (true)
            {
                // Java conversion
                var conv = ProjectConverter.CreateFor<CSToJavaConversion>(project);
                conv.Conversion.SkipBody = true;
                conv.Conversion.BaseNamespace = "com.euronovate.libpdf";
                genargs.SourceRootPath = @"D:\Staging\Euronovate\ENLibPdfJar\src\main\java";
                genargs.EagerStringConversion = true;
                conv.ConvertAndWrite(genargs);
            }
            else
            {
                // JNI conversion
                var conv2 = ProjectConverter.CreateFor<CSToJNIConversion>(project);
                conv2.Conversion.BaseNamespace = "com.euronovate.libpdf";
                genargs.SourceRootPath = @"D:\Staging\Euronovate\ENLibPdf\ENLibPdfJni";
                genargs.EagerStringConversion = true;
                conv2.ConvertAndWrite(genargs);
            }
        }
    }
}
