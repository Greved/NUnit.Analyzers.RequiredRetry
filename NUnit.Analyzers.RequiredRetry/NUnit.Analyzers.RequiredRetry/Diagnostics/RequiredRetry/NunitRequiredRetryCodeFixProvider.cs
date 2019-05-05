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

namespace NUnit.Analyzers.RequiredRetry.Diagnostics.RequiredRetry
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NunitRequiredRetryCodeFixProvider)), Shared]
    public class NunitRequiredRetryCodeFixProvider : CodeFixProvider
    {
        private const string NoRetryTitle = "Add Retry attribute";

        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(NunitRequiredRetryDiagnostic.Rule.Id); }
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
            var methodDeclaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<MethodDeclarationSyntax>().First();

            // Register a code action that will invoke the fix.
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: NoRetryTitle,
                    createChangedDocument: c => AddRetryAttributeAsync(context.Document, methodDeclaration, c),
                    equivalenceKey: NoRetryTitle),
                diagnostic);
        }

        private static async Task<Document> AddRetryAttributeAsync(Document document, MethodDeclarationSyntax methodDeclaration, CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken);
            // Create 'Retry(2)' attribute
            var attribute =
                            SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("Retry"))
                                .WithArgumentList(
                                    SyntaxFactory.AttributeArgumentList(
                                        SyntaxFactory.SingletonSeparatedList(
                                            SyntaxFactory.AttributeArgument(SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(2))))
                                    )
                                );
            var firstAttrList = methodDeclaration.AttributeLists[0];
            var newFirstAttrList = firstAttrList.AddAttributes(attribute);
            // Add it to attribute lists
            var attributes = methodDeclaration.AttributeLists.Replace(firstAttrList, newFirstAttrList);

            return document.WithSyntaxRoot(
                root.ReplaceNode(
                    methodDeclaration,
                    methodDeclaration.WithAttributeLists(attributes)
                ));
        }
    }
}
