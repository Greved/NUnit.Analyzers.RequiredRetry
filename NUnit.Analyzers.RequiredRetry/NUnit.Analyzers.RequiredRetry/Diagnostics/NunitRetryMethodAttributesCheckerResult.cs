using Microsoft.CodeAnalysis;

namespace NUnit.Analyzers.RequiredRetry.Diagnostics
{
    public class NunitRetryMethodAttributesCheckerResult
    {
        public NunitRetryMethodAttributesCheckerResultType Type { get; set; }
        public Location RetryAttributeLocation { get; set; }
    }
}