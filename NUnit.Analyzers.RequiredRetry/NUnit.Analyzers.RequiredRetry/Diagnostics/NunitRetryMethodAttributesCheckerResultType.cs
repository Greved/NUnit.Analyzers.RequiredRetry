namespace NUnit.Analyzers.RequiredRetry.Diagnostics
{
    public enum NunitRetryMethodAttributesCheckerResultType
    {
        Correct,
        NoRetry,
        RetryBelowOrEqualsOne
    }
}