using System.Text.RegularExpressions;

namespace CombatDicesTeam.Engine.Ui;

public static class TextParser
{
    public static IReadOnlyList<RichTextCommand> ParseText(string inputText)
    {
        // Parsing of the tags to change text color.
        string pattern = "(<style=(?:.*?)>(?:[\\s\\S]*?)<\\/style>)";
        string[] splitText = Regex.Split(inputText, pattern);

        var list = new List<RichTextCommand>();
        
        foreach (string part in splitText)
        {
            if (string.IsNullOrWhiteSpace(part))
            {
                continue;
            }

            if (Regex.IsMatch(part, pattern))
            {
                string pattern2 = "<style=(.*?)>(.*?)<\\/style>";
                Match match = Regex.Match(part, pattern2);
                string styleDescription = match.Groups[1].Value;

                var style = ParseStyle(styleDescription);
                
                string content = match.Groups[2].Value;
                list.Add(new RichTextCommand(content, style));
            }
            else
            {
                // regular text
                list.Add(new RichTextCommand(part, new RichTextNodeStyle(null, null)));
            }
        }

        return list;
    }

    private static RichTextNodeStyle ParseStyle(string styleDescription)
    {
        var items = styleDescription.Split(',');

        int? colorIndex = null;
        int? animationIndex = null;
        foreach (var item in items)
        {
            if (item.StartsWith("color"))
            {
                var s = item[("color".Length)..];
                colorIndex = int.Parse(s);
            }
            else if (item.StartsWith("ani"))
            {
                var s = item[("ani".Length)..];
                animationIndex = int.Parse(s);
            }
        }

        return new RichTextNodeStyle(colorIndex, animationIndex);
    }
}