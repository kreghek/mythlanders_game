using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rpg.Client.Core
{
    public static class StringHelper
    {
        public static readonly IReadOnlyList<char> WordBreakers = new[] { ' ' };

        public static string LineBreaking(string text, int maxInLine)
        {
            var items = text.Split(Environment.NewLine);
            var mainSb = new StringBuilder();
            var singleSb = new StringBuilder();
            foreach (var item in items)
            {
                var words = item.Split(WordBreakers.ToArray(), StringSplitOptions.RemoveEmptyEntries);

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

        private static void AppendWord(string word, StringBuilder sb, bool isFirstInLine)
        {
            if (!isFirstInLine)
            {
                sb.Append(" ");
            }

            sb.Append(word);
        }

        private static void StartNewLine(StringBuilder mainSb, StringBuilder? singleSb)
        {
            singleSb?.Clear();
            mainSb.AppendLine();
        }
    }
}