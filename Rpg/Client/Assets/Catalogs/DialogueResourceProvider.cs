using System;
using System.IO;
using System.Reflection;

using Microsoft.Xna.Framework.Content;

using Rpg.Client.Assets.Catalogs;

namespace Client.Assets.Catalogs;

internal sealed class DialogueResourceProvider : IDialogueResourceProvider
{
    private readonly ContentManager _contentManager;

    public DialogueResourceProvider(ContentManager contentManager)
    {
        _contentManager = contentManager;
    }

    public string GetResource(string resourceSid)
    {
        return _contentManager.Load<string>($"Dialogues/{resourceSid}");
    }
}