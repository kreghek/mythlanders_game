using JetBrains.Annotations;

using Microsoft.Xna.Framework.Content.Pipeline;

namespace GameContent.Extensions;

[UsedImplicitly]
[ContentProcessor]
public class RawStringProcessor : ContentProcessor<string, string>
{
    /// <summary>
    /// Process the input content item.
    /// </summary>
    public override string Process(string input, ContentProcessorContext context)
    {
        context.Logger.LogMessage($"Processing raw {context.OutputFilename}");

        return input;
    }
}