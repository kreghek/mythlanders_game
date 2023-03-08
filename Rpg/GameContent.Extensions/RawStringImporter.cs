using System.IO;

using JetBrains.Annotations;

using Microsoft.Xna.Framework.Content.Pipeline;

namespace GameContent.Extensions;

[UsedImplicitly]
[ContentImporter(".json", DefaultProcessor = "RawStringProcessor")]
public class RawStringImporter : ContentImporter<string>
{
    public override string Import(string filename, ContentImporterContext context)
    {
        context.Logger.LogMessage("Importing raw file: {0}", filename);

        return File.ReadAllText(filename);
    }
}