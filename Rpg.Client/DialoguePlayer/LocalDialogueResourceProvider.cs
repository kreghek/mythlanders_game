using System;
using System.IO;
using System.Linq;
using System.Text;

using Rpg.Client.Assets.Catalogs;

namespace DialoguePlayer;

public class LocalDialogueResourceProvider : IDialogueResourceProvider
{
    public string GetResource(string resourceSid)
    {
        var n = 1;
        var lines = File.ReadAllText(Path.Combine("Content", "Dialogues", resourceSid + ".xnb"), Encoding.UTF8)
            .Split(Environment.NewLine.ToCharArray())
            .Skip(n)
            .ToArray();

        var output = string.Join(Environment.NewLine, lines);

        return output;
    }
}