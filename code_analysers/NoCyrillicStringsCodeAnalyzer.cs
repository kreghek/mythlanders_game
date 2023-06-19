using System.Collections.Immutable;
using System.Text.RegularExpressions;

using JetBrains.Annotations;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CodeAnalysers;

/// <summary>
/// The analyzer finds strings containing Non-Latin, which are used as arguments to methods,
/// constructor arguments, class properties, and issues a warning.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
[UsedImplicitly]
public sealed class LatinOnlyStringsCodeAnalyzer : DiagnosticAnalyzer
{
    private const string DIAGNOSTIC_ID = "O20002";
    private const string TITLE = "Strings can't contains cyrillic symbols";
    private const string MESSAGE_FORMAT = "String can't contains cyrillic symbols {0}";
    private const string DESCRIPTION = "Translate string to English.";
    private const string CATEGORY = "CodeQuality";

    private static readonly DiagnosticDescriptor _rule = new(
        DIAGNOSTIC_ID,
        TITLE,
        MESSAGE_FORMAT,
        CATEGORY,
        DiagnosticSeverity.Error,
        true,
        DESCRIPTION);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(_rule);

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

                    if (IsBasicLatin(stringValue))
                    {
                        context.ReportDiagnostic(Diagnostic.Create(_rule, context.Node.GetLocation(), stringValue));
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
        if (objectCreationExpressionSyntax.ArgumentList?.Arguments == null)
        {
            return;
        }

        foreach (var argumentSyntax in objectCreationExpressionSyntax.ArgumentList.Arguments)
        {
            if (argumentSyntax.Expression.Kind() is SyntaxKind.StringLiteralExpression
                or SyntaxKind.InterpolatedStringExpression)
            {
                var stringValue = argumentSyntax.Expression.ToString();

                if (IsBasicLatin(stringValue))
                {
                    context.ReportDiagnostic(Diagnostic.Create(_rule, context.Node.GetLocation(), stringValue));
                }
            }
        }
    }

    private static void AnalyzeProperty(SyntaxNodeAnalysisContext context,
        PropertyDeclarationSyntax propertyDeclaration)
    {
        var expression = propertyDeclaration.ExpressionBody?.Expression;

        if (expression?.Kind() is not (SyntaxKind.StringLiteralExpression or SyntaxKind.InterpolatedStringExpression))
        {
            return;
        }

        var stringValue = expression.ToString();

        if (!IsBasicLatin(stringValue))
        {
            context.ReportDiagnostic(Diagnostic.Create(_rule, context.Node.GetLocation(), stringValue));
        }
    }

    private static bool IsBasicLatin(string stringValue)
    {
        return Regex.IsMatch(stringValue, "\\p{IsBasicLatin}+");
    }
}