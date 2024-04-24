using System.Text.RegularExpressions;

namespace CombatDicesTeam.Engine.Ui;

public static class TextParser
{
    public static IReadOnlyList<RichTextNode> ParseText(string inputText)
    {
        // Parsing of the tags to change text color.
        var pattern = "(<style=(?:.*?)>(?:[\\s\\S]*?)<\\/style>)";
        var splitText = Regex.Split(inputText, pattern);

        var list = new List<RichTextNode>();

        foreach (var part in splitText)
        {
            if (string.IsNullOrWhiteSpace(part))
            {
                continue;
            }

            if (Regex.IsMatch(part, pattern))
            {
                var pattern2 = "<style=(.*?)>([\\s\\S]*?)<\\/style>";
                var match = Regex.Match(part, pattern2);
                var styleDescription = match.Groups[1].Value;

                var style = ParseStyle(styleDescription);

                var content = match.Groups[2].Value;
                list.Add(new RichTextNode(content, style));
            }
            else
            {
                // regular text
                list.Add(new RichTextNode(part, new RichTextNodeStyle(null, null)));
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
                var s = item["color".Length..];
                colorIndex = int.Parse(s);
            }
            else if (item.StartsWith("ani"))
            {
                var s = item["ani".Length..];
                animationIndex = int.Parse(s);
            }
        }

        return new RichTextNodeStyle(colorIndex, animationIndex);
    }
}