using System.IO;

using Microsoft.Xna.Framework.Content.Pipeline;

namespace GameContent.Extensions;

public class TtfImporter : ContentImporter<byte[]>
{
    public override byte[] Import(string filename, ContentImporterContext context)
    {
        return File.ReadAllBytes(Path.ChangeExtension(filename, ".ttf"));
    }
}