using Microsoft.CodeAnalysis;

namespace NUnit.Analyzers.RequiredRetry.Diagnostics.RequiredRetry
{
    public static class NunitRequiredRetryDiagnostic
    {
        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.RequiredRetryAnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.RequiredRetryAnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.RequiredRetryAnalyzerDescription), Resources.ResourceManager, typeof(Resources));

        public static DiagnosticDescriptor Rule = new DiagnosticDescriptor(id: "NunitRequiredRetry", 
            title: Title, 
            messageFormat: MessageFormat, 
            category: "Design", 
            defaultSeverity: DiagnosticSeverity.Warning, 
            isEnabledByDefault: true, 
            description: Description);
    }
}