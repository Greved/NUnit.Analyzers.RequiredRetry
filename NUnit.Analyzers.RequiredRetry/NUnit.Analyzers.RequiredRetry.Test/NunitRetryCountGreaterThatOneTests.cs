using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using NUnit.Framework;
using NUnit.Analyzers.RequiredRetry.Diagnostics;
using NUnit.Analyzers.RequiredRetry.Diagnostics.RetryCountGreaterThatOne;
using TestHelper;

namespace NUnit.Analyzers.RequiredRetry.Test
{
    [TestFixture]
    public class NunitRetryCountGreaterThatOneTests: CodeFixVerifier
    {
        [TestCaseSource(nameof(When_Diagnostic_Is_Raised_Fix_Updates_Code_Cases))]
        public void When_Diagnostic_Is_Raised_Fix_Updates_Code(string test, string fixTest, int line, int column)
        {
            var expected = new DiagnosticResult
            {
                Id = NunitRetryCountGreaterThatOneDiagnostic.Rule.Id,
                Message = new LocalizableResourceString(nameof(Resources.RetryCountGreaterThatOneAnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources), "Test1").ToString(),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                        new DiagnosticResultLocation("Test0.cs", line, column)
                    }
            };

            VerifyCSharpDiagnostic(test, expected);

            VerifyCSharpFix(test, fixTest);
        }

        private static IEnumerable<TestCaseData> When_Diagnostic_Is_Raised_Fix_Updates_Code_Cases()
        {
            yield return new TestCaseData(
                @"
using NUnit.Framework;
namespace LibWithTestsToAnalyze
{
    public class Class1
    {
        [Test, Retry(1)]
        public void Test1()
        {
        }
    }
}",
                @"
using NUnit.Framework;
namespace LibWithTestsToAnalyze
{
    public class Class1
    {
        [Test, Retry(2)]
        public void Test1()
        {
        }
    }
}",
                7, 16).SetName("Retry count is 1");
            yield return new TestCaseData(
                @"
using NUnit.Framework;
namespace LibWithTestsToAnalyze
{
    public class Class1
    {
        [Test, Retry(0)]
        public void Test1()
        {
        }
    }
}",
                @"
using NUnit.Framework;
namespace LibWithTestsToAnalyze
{
    public class Class1
    {
        [Test, Retry(2)]
        public void Test1()
        {
        }
    }
}",
                7, 16).SetName("Retry count is zero");
            yield return new TestCaseData(
                @"
using NUnit.Framework;
namespace LibWithTestsToAnalyze
{
    public class Class1
    {
        [Test, Retry()]
        public void Test1()
        {
        }
    }
}",
                @"
using NUnit.Framework;
namespace LibWithTestsToAnalyze
{
    public class Class1
    {
        [Test, Retry(2)]
        public void Test1()
        {
        }
    }
}",
                7, 16).SetName("Retry count is absent");
        
    }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new NunitRetryCountGreaterThatOneCodeFixProvider();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new NunitRetryAnalyzer();
        }
    }
}