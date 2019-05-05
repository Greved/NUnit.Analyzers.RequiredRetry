using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NUnit.Analyzers.RequiredRetry.Diagnostics.RetryCountGreaterThatOne
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NunitRetryCountGreaterThatOneCodeFixProvider)), Shared]
    public class NunitRetryCountGreaterThatOneCodeFixProvider: CodeFixProvider
    {
        private const string title = "Set Retry count to '2'";

        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(NunitRetryCountGreaterThatOneDiagnostic.Rule.Id); }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the method declaration identified by the diagnostic.
            var attribute = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<AttributeSyntax>().First();

            // Register a code action that will invoke the fix.
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: title,
                    createChangedDocument: c => SetRetryCountToTwoAttributeAsync(context.Document, attribute, c),
                    equivalenceKey: title),
                diagnostic);
        }

        private static async Task<Document> SetRetryCountToTwoAttributeAsync(Document document, AttributeSyntax attributeSyntax, CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken);
            var newRetryAttributeArguments = SyntaxFactory.AttributeArgumentList(
                SyntaxFactory.SingletonSeparatedList(
                    SyntaxFactory.AttributeArgument(SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(2))))
            );
            var newRetryAttribute = attributeSyntax.WithArgumentList(newRetryAttributeArguments);

            return document.WithSyntaxRoot(
                root.ReplaceNode(
                    attributeSyntax,
                    newRetryAttribute
                ));
        }
    }
}