using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using NUnit.Framework;
using NUnit.Analyzers.RequiredRetry.Diagnostics;
using NUnit.Analyzers.RequiredRetry.Diagnostics.RequiredRetry;
using TestHelper;

namespace NUnit.Analyzers.RequiredRetry.Test
{
    [TestFixture]
    public class UnitTest : CodeFixVerifier
    {
        [TestCase("", TestName = "Empty source")]
        [TestCase(@"
using NUnit.Framework;
namespace LibWithTestsToAnalyze
{
    public class CLASS1
    {
        [Test]
        [Retry(2)]
        public void Test1()
        {
        }
    }
}", TestName = "Retry in separate attributes list")]
        [TestCase(@"
using NUnit.Framework;
namespace LibWithTestsToAnalyze
{
    public class CLASS1
    {
        [Test, Retry(2)]
        public void Test1()
        {
        }
    }
}", TestName = "Retry in single attributes list")]
        [TestCase(@"
using NUnit.Framework;
namespace LibWithTestsToAnalyze
{
    public class CLASS1
    {
        [Test, Retry(88)]
        public void Test1()
        {
        }
    }
}", TestName = "Retry count is greater than 2")]
        public void No_Diagnostic(string test)
        {
            VerifyCSharpDiagnostic(test);
        }

        [TestCaseSource(nameof(When_Diagnostic_Is_Raised_Fix_Updates_Code_Cases))]
        public void When_Diagnostic_Is_Raised_Fix_Updates_Code(string test, string fixTest, int line, int column)
        {
            var expected = new DiagnosticResult
            {
                Id = NunitRequiredRetryDiagnostic.Rule.Id,
                Message = new LocalizableResourceString(nameof(Resources.RequiredRetryAnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources), "Test1").ToString(),
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
        [Test]
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
                8, 21).SetName("No Retry attribute");

            yield return new TestCaseData(
                @"
using NUnit.Framework;
namespace LibWithTestsToAnalyze
{
    public class CLASS1
    {
        [TestCaseSource(nameof(DivideCases))]
        public void Test1()
        {
        }

        static object[] DivideCases =
        {
            new object[] { 12, 3 },
            new object[] { 12, 2 },
        };
    }
}",
                @"
using NUnit.Framework;
namespace LibWithTestsToAnalyze
{
    public class CLASS1
    {
        [TestCaseSource(nameof(DivideCases)), Retry(2)]
        public void Test1()
        {
        }

        static object[] DivideCases =
        {
            new object[] { 12, 3 },
            new object[] { 12, 2 },
        };
    }
}",
                8, 21).SetName("TestCaseSource conversion");

            yield return new TestCaseData(
    @"
using NUnit.Framework;
namespace LibWithTestsToAnalyze
{
    public class CLASS1
    {
        [TestCase(1, 2)]
        [TestCase(4, 10)]
        public void Test1()
        {
        }
    }
}",
    @"
using NUnit.Framework;
namespace LibWithTestsToAnalyze
{
    public class CLASS1
    {
        [TestCase(1, 2), Retry(2)]
        [TestCase(4, 10)]
        public void Test1()
        {
        }
    }
}",
            9, 21).SetName("TestCase should add Retry only to first AttributesList");
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new NunitRequiredRetryCodeFixProvider();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new NunitRetryAnalyzer();
        }
    }
}
