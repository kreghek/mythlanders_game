using Microsoft.Xna.Framework.Content.Pipeline;

namespace GameContent.Extensions;

[ContentProcessor]
public class TtfProcessor : ContentProcessor<byte[], byte[]>
{
    public override byte[] Process(byte[] input, ContentProcessorContext context)
    {
        context.Logger.LogMessage("Do nothing with binary.");

        return input;
    }
}