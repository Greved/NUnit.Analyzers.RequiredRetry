using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace NUnit.Analyzers.RequiredRetry.Diagnostics
{
    public static class NunitRetryMethodAttributesChecker
    {
        private static readonly HashSet<string> TestAttributeClassNames = new HashSet<string>
        {
            "TestAttribute",
            "TestCaseAttribute",
            "TestCaseSourceAttribute",
        };

        public static NunitRetryMethodAttributesCheckerResult Check(ImmutableArray<AttributeData> attributes)
        {
            var testAttributeExists = false;
            int? retryCount = null;
            Location retryAttributeLocation = null;

            foreach (var attribute in attributes)
            {
                if (!testAttributeExists && TestAttributeClassNames.Contains(attribute.AttributeClass.Name))
                {
                    testAttributeExists = true;
                }
                if (!retryCount.HasValue && attribute.AttributeClass.Name == "RetryAttribute")
                {
                    var retryAttributeArgument = attribute.ConstructorArguments.FirstOrDefault();
                    retryCount = retryAttributeArgument.Equals(default(TypedConstant)) ? null : retryAttributeArgument.Value as int?;
                    retryAttributeLocation =
                        attribute.ApplicationSyntaxReference.SyntaxTree.GetLocation(attribute.ApplicationSyntaxReference
                            .Span);
                }
            }

            if (!testAttributeExists)
            {
                return new NunitRetryMethodAttributesCheckerResult
                {
                    Type = NunitRetryMethodAttributesCheckerResultType.Correct
                };
            }

            if (retryAttributeLocation == null)
            {
                return new NunitRetryMethodAttributesCheckerResult
                {
                    Type = NunitRetryMethodAttributesCheckerResultType.NoRetry
                };
            }

            return retryCount > 1
                ?  new NunitRetryMethodAttributesCheckerResult
                   {
                       Type = NunitRetryMethodAttributesCheckerResultType.Correct
                   }
                : new NunitRetryMethodAttributesCheckerResult
                {
                    Type = NunitRetryMethodAttributesCheckerResultType.RetryBelowOrEqualsOne,
                    RetryAttributeLocation = retryAttributeLocation
                };
        }
    }
}