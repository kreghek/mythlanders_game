using System.Collections.Immutable;

using CodeAnalysers.Utils;

using JetBrains.Annotations;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CodeAnalysers;

/// <summary>
/// The analyzer checks that the names of the test methods correspond to the accepted coding convention (underscore).
/// In this case, it is allowed that part of the name will be in camelCase (for example, to indicate the name of the methods being tested).
/// The analysis includes the names of methods from the namespace containing '.Test' and marked with an attribute (any).
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
[UsedImplicitly]
public sealed class TestNamesAnalyzer : DiagnosticAnalyzer
{
    private const string DIAGNOSTIC_ID = "O20003";
    private const string TITLE = "Test name must be in snakecase";
    private const string MESSAGE_FORMAT = "Test name must be in snakecase \"{0}\"";
    private const string DESCRIPTION = "Transform test name to snakecase.";
    private const string CATEGORY = "CodeQuality";

    private static readonly DiagnosticDescriptor _rule = new(
        DIAGNOSTIC_ID,
        TITLE,
        MESSAGE_FORMAT,
        CATEGORY,
        DiagnosticSeverity.Warning,
        true,
        DESCRIPTION);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(_rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.MethodDeclaration);
    }

    private static void AnalyzeNode(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not MethodDeclarationSyntax methodDeclaration)
        {
            return;
        }

        if (!SyntaxNodeHelper.TryGetParentSyntax(
                methodDeclaration,
                out BaseNamespaceDeclarationSyntax? namespaceDeclaration))
        {
            return;
        }

        var methodName = methodDeclaration.Identifier.ToString();

        var isPublic = methodDeclaration.Modifiers.Any(SyntaxKind.PublicKeyword);
        var isTest = namespaceDeclaration?.Name.ToString().Contains(".Tests");
        var hasAttributes = methodDeclaration.AttributeLists.Any();
        var isSnakeCase = IsSnakeCase(methodName);

        if (isPublic && isTest.GetValueOrDefault() && hasAttributes && !isSnakeCase)
        {
            context.ReportDiagnostic(
                Diagnostic.Create(
                    _rule,
                    context.Node.GetLocation(),
                    methodName));
        }
    }

    private static bool IsSnakeCase(string methodName)
    {
        return methodName.Contains('_');
    }
}