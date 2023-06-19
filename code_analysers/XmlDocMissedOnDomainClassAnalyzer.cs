using System.Collections.Immutable;

using CodeAnalysers.Utils;

using JetBrains.Annotations;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CodeAnalysers;

/// <summary>
/// Анализатор проверяет наличие документации для публичных не статических свойств и методов.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
[UsedImplicitly]
public sealed class XmlDocMissedOnDomainClassAnalyzer : DiagnosticAnalyzer
{
    private const string DiagnosticId = "O20001";
    private const string Title = "Non-static public property should have xmldoc comment";
    private const string MessageFormat = "Xmldoc comment missed on property \"{0}.{1}\"";
    private const string Description = "Add xmldoc comment.";
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
        context.RegisterSyntaxNodeAction(
            AnalyzeNode,
            SyntaxKind.PropertyDeclaration,
            SyntaxKind.MethodDeclaration);
    }

    private static void AnalyzeMethod(SyntaxNodeAnalysisContext context,
        MethodDeclarationSyntax methodDeclarationSyntax)
    {
        if (!SyntaxNodeHelper.TryGetParentSyntax(
                methodDeclarationSyntax,
                out ClassDeclarationSyntax? classDeclarationSyntax))
        {
            return;
        }

        var isPublic = methodDeclarationSyntax.Modifiers.Any(SyntaxKind.PublicKeyword);

        var hasDocumentationComment = methodDeclarationSyntax.HasStructuredTrivia || methodDeclarationSyntax.HasLeadingTrivia;

        if (isPublic && !hasDocumentationComment)
        {
            context.ReportDiagnostic(
                Diagnostic.Create(
                    Rule,
                    context.Node.GetLocation(),
                    classDeclarationSyntax.Identifier.ToString(),
                    methodDeclarationSyntax.Identifier.ToString()));
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

            case MethodDeclarationSyntax methodDeclarationSyntax:
                AnalyzeMethod(context, methodDeclarationSyntax);
                return;
            
            case FieldDeclarationSyntax fieldDeclarationSyntax:
                AnalyzeField(context, fieldDeclarationSyntax);
                return;

            default: return;
        }
    }

    private static void AnalyzeProperty(SyntaxNodeAnalysisContext context,
        PropertyDeclarationSyntax propertyDeclaration)
    {
        if (!SyntaxNodeHelper.TryGetParentSyntax(
                propertyDeclaration,
                out ClassDeclarationSyntax classDeclarationSyntax))
        {
            return;
        }

        var isPublic = propertyDeclaration.Modifiers.Any(SyntaxKind.PublicKeyword);

        var hasDocumentationComment = propertyDeclaration.HasStructuredTrivia;

        if (isPublic && !hasDocumentationComment)
        {
            context.ReportDiagnostic(
                Diagnostic.Create(
                    Rule,
                    context.Node.GetLocation(),
                    classDeclarationSyntax.Identifier.ToString(),
                    propertyDeclaration.Identifier.ToString()));
        }
    }
    
    private static void AnalyzeField(SyntaxNodeAnalysisContext context,
        FieldDeclarationSyntax fieldDeclaration)
    {
        if (!SyntaxNodeHelper.TryGetParentSyntax(
                fieldDeclaration,
                out ClassDeclarationSyntax classDeclarationSyntax))
        {
            return;
        }

        var isPublic = fieldDeclaration.Modifiers.Any(SyntaxKind.PublicKeyword);

        var hasDocumentationComment = fieldDeclaration.HasStructuredTrivia;

        if (isPublic && !hasDocumentationComment)
        {
            context.ReportDiagnostic(
                Diagnostic.Create(
                    Rule,
                    context.Node.GetLocation(),
                    classDeclarationSyntax.Identifier.ToString(),
                    fieldDeclaration.GetText()));
        }
    }
}