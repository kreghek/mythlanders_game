﻿using System;
using System.IO;
using System.Linq;
using System.Text;

using Rpg.Client.Assets.Catalogs;

namespace DialoguePlayer;

public class LocalDialogueResourceProvider : IDialogueResourceProvider
{
    public string GetResource(string resourceSid)
    {
        int n = 1;
        string[] lines = File.ReadAllText(Path.Combine("Content", "Dialogues", resourceSid + ".xnb"), Encoding.UTF8)
            .Split(Environment.NewLine.ToCharArray())
            .Skip(n)
            .ToArray();

        string output = string.Join(Environment.NewLine, lines);

        return output;
    }
}