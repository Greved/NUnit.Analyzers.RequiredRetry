using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using NUnit.Analyzers.RequiredRetry.Diagnostics.RequiredRetry;
using NUnit.Analyzers.RequiredRetry.Diagnostics.RetryCountGreaterThatOne;

namespace NUnit.Analyzers.RequiredRetry.Diagnostics
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class NunitRetryAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(NunitRequiredRetryDiagnostic.Rule, NunitRetryCountGreaterThatOneDiagnostic.Rule);

        public override void Initialize(AnalysisContext context)
        {
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var methodSymbol = (IMethodSymbol)context.Symbol;
            if (methodSymbol.DeclaredAccessibility != Accessibility.Public)
            {
                return;
            }

            if (methodSymbol.MethodKind != MethodKind.Ordinary)
            {
                return;
            }

            var methodAttributes = methodSymbol.GetAttributes();
            var methodCheckerResult = NunitRetryMethodAttributesChecker.Check(methodAttributes);
            switch (methodCheckerResult.Type)
            {
                case NunitRetryMethodAttributesCheckerResultType.NoRetry:
                    context.ReportDiagnostic(Diagnostic.Create(NunitRequiredRetryDiagnostic.Rule, methodSymbol.Locations[0], methodSymbol.Name));
                    break;
                case NunitRetryMethodAttributesCheckerResultType.RetryBelowOrEqualsOne:
                    context.ReportDiagnostic(Diagnostic.Create(NunitRetryCountGreaterThatOneDiagnostic.Rule, methodCheckerResult.RetryAttributeLocation, methodSymbol.Name));
                    break;
            }
        }
    }
}
