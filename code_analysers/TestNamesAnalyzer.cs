using System.Collections.Immutable;

using CodeAnalysers.Utils;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CodeAnalysers;

/// <summary>
/// Анализатор проверяет, что названия методов тестов соответствуют принятой конвенции кодирования (underscore).
/// При этом допускается, что часть названия будет в camelCase (например для указания названия тестируемых методов).
/// Под анализ попадают названия методов из неймспейса, содержащего '.Test' и помеченных атрибутом (любым).
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class TestNamesAnalyzer : DiagnosticAnalyzer
{
    private const string DiagnosticId = "O20003";
    private const string Title = "Test name must be in snakecase";
    private const string MessageFormat = "Test name must be in snakecase \"{0}\"";
    private const string Description = "Transform test name to snakecase.";
    private const string Category = "CodeQuality";

    public static readonly DiagnosticDescriptor Rule = new(
        DiagnosticId,
        Title,
        MessageFormat,
        Category,
        DiagnosticSeverity.Warning,
        true,
        Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.MethodDeclaration);
    }

    private static void AnalyzeNode(SyntaxNodeAnalysisContext context)
    {
        if (!(context.Node is MethodDeclarationSyntax methodDeclaration))
        {
            return;
        }

        if (!SyntaxNodeHelper.TryGetParentSyntax(
                methodDeclaration,
                out BaseNamespaceDeclarationSyntax namespaceDeclaration))
        {
            return;
        }

        var methodName = methodDeclaration.Identifier.ToString();

        var isPublic = methodDeclaration.Modifiers.Any(SyntaxKind.PublicKeyword);
        var isTest = namespaceDeclaration.Name.ToString().Contains(".Tests");
        var hasAttributes = methodDeclaration.AttributeLists.Any();
        var isSnakeCase = IsSnakeCase(methodName);

        if (isPublic && isTest && hasAttributes && !isSnakeCase)
        {
            context.ReportDiagnostic(
                Diagnostic.Create(
                    Rule,
                    context.Node.GetLocation(),
                    methodName));
        }
    }

    private static bool IsSnakeCase(string methodName)
    {
        return methodName.Contains('_');
    }
}