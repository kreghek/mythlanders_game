using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CodeAnalysers;

/// <summary>
/// Анализатор находит строки содержащие кириллицу, которые используется в качестве аргументов методов,
/// аргументов конструкторов, свойств классов и выдает предупреждение.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class NoCyrillicStringsCodeAnalyzer : DiagnosticAnalyzer
{
    private const string DiagnosticId = "O20002";
    private const string Title = "Strings can't contains cyrillic symbols";
    private const string MessageFormat = "String can't contains cyrillic symbols {0}";
    private const string Description = "Translate string to English.";
    private const string Category = "CodeQuality";

    public static readonly DiagnosticDescriptor Rule = new(
        DiagnosticId,
        Title,
        MessageFormat,
        Category,
        DiagnosticSeverity.Error,
        true,
        Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(
            AnalyzeNode,
            SyntaxKind.PropertyDeclaration,
            SyntaxKind.ExpressionStatement,
            SyntaxKind.ObjectCreationExpression,
            SyntaxKind.FieldDeclaration
        );
    }

    private static void AnalyzeMethodCall(SyntaxNodeAnalysisContext context,
        ExpressionStatementSyntax expressionStatementSyntax)
    {
        if (expressionStatementSyntax.Expression is InvocationExpressionSyntax invocationExpressionSyntax)
        {
            foreach (var argumentSyntax in invocationExpressionSyntax.ArgumentList.Arguments)
            {
                if (argumentSyntax.Expression.Kind() is SyntaxKind.StringLiteralExpression
                    or SyntaxKind.InterpolatedStringExpression)
                {
                    var stringValue = argumentSyntax.Expression.ToString();

                    if (IsCyrillic(stringValue))
                    {
                        context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation(), stringValue));
                    }
                }
            }
        }
    }

    private static void AnalyzeNode(SyntaxNodeAnalysisContext context)
    {
        var node = context.Node;

        switch (node)
        {
            case PropertyDeclarationSyntax propertyDeclaration:
                AnalyzeProperty(context, propertyDeclaration);
                return;

            case ExpressionStatementSyntax expressionStatementSyntax:
                AnalyzeMethodCall(context, expressionStatementSyntax);
                return;

            case ObjectCreationExpressionSyntax objectCreationExpressionSyntax:
                AnalyzeObjectCreation(context, objectCreationExpressionSyntax);
                return;

            default: return;
        }
    }

    private static void AnalyzeObjectCreation(SyntaxNodeAnalysisContext context,
        ObjectCreationExpressionSyntax objectCreationExpressionSyntax)
    {
        if (objectCreationExpressionSyntax.ArgumentList?.Arguments != null)
        {
            foreach (var argumentSyntax in objectCreationExpressionSyntax.ArgumentList.Arguments)
            {
                if (argumentSyntax.Expression.Kind() is SyntaxKind.StringLiteralExpression
                    or SyntaxKind.InterpolatedStringExpression)
                {
                    var stringValue = argumentSyntax.Expression.ToString();

                    if (IsCyrillic(stringValue))
                    {
                        context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation(), stringValue));
                    }
                }
            }
        }
    }

    private static void AnalyzeProperty(SyntaxNodeAnalysisContext context,
        PropertyDeclarationSyntax propertyDeclaration)
    {
        var expression = propertyDeclaration.ExpressionBody?.Expression;

        if (expression?.Kind() is SyntaxKind.StringLiteralExpression or SyntaxKind.InterpolatedStringExpression)
        {
            var stringValue = expression.ToString();

            if (IsCyrillic(stringValue))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation(), stringValue));
            }
        }
    }

    private static bool IsCyrillic(string stringValue)
    {
        return Regex.IsMatch(stringValue, "\\p{IsCyrillic}+");
    }
}