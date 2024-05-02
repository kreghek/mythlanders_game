using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Core;

public static class StringHelper
{
    private static readonly IReadOnlyList<char> _wordBreakers = new[] { ' ' };
    private static readonly IReadOnlyList<char> _sourceNewLineCharacters = new[] { '\n' };

    public static string LineBreaking(string text, int maxInLine)
    {
        var items = text.Split(_sourceNewLineCharacters.ToArray());
        var mainSb = new StringBuilder();
        var singleSb = new StringBuilder();
        foreach (var item in items)
        {
            var words = item.Split(_wordBreakers.ToArray(), StringSplitOptions.RemoveEmptyEntries);

            singleSb.Clear();
            var isFirstInLine = true;
            foreach (var word in words)
            {
                AppendWord(word, singleSb, isFirstInLine);

                if (singleSb.Length <= maxInLine)
                {
                    AppendWord(word, mainSb, isFirstInLine);
                }
                else
                {
                    StartNewLine(mainSb, singleSb);

                    AppendWord(word, singleSb, isFirstInLine: true);
                    AppendWord(word, mainSb, isFirstInLine: true);
                }

                isFirstInLine = false;
            }

            StartNewLine(mainSb, singleSb: null);
        }

        return mainSb.ToString().Trim();
    }
    
    public static string RichLineBreaking(string text, int maxInLine)
    {
        var items = text.Split(_sourceNewLineCharacters.ToArray());
        var mainSb = new StringBuilder();
        var singleSb = new StringBuilder();
        foreach (var item in items)
        {
            var countOfStylesInLine = GetStyleCount(item);
            
            var words = item.Split(_wordBreakers.ToArray(), StringSplitOptions.RemoveEmptyEntries);

            singleSb.Clear();
            var isFirstInLine = true;
            foreach (var word in words)
            {
                AppendWord(word, singleSb, isFirstInLine);

                if (singleSb.Length - countOfStylesInLine <= maxInLine)
                {
                    AppendWord(word, mainSb, isFirstInLine);
                }
                else
                {
                    StartNewLine(mainSb, singleSb);

                    AppendWord(word, singleSb, isFirstInLine: true);
                    AppendWord(word, mainSb, isFirstInLine: true);
                }

                isFirstInLine = false;
            }

            StartNewLine(mainSb, singleSb: null);
        }

        return mainSb.ToString().Trim();
    }

    private static int GetStyleCount(string line)
    {
        var sum = 0;
        if (line.Contains("<style=color1>") || line.Contains("<style=color2>"))
        {
            sum += 14;
        }

        if (line.Contains("</style>"))
        {
            sum += 8;
        }

        return sum;
    }

    /// <summary>
    /// Replaces characters that are not in the font table with the correct ones.
    /// </summary>
    internal static string FixText(string text)
    {
        return text.Replace('�', '�').Replace('�', '�').Replace("�", "...");
    }

    private static void AppendWord(string word, StringBuilder sb, bool isFirstInLine)
    {
        if (!isFirstInLine)
        {
            sb.Append(' ');
        }

        sb.Append(word);
    }

    private static void StartNewLine(StringBuilder mainSb, StringBuilder? singleSb)
    {
        singleSb?.Clear();
        mainSb.AppendLine();
    }
}