using System;
using System.IO;
using System.Reflection;

namespace Rpg.Client.Assets.Catalogs
{
    internal sealed class DialogueResourceProvider : IDialogueResourceProvider
    {
        public string GetResource(string resourceSid)
        {
            var assembly = Assembly.GetExecutingAssembly();
            const string RESOURCE_PATH = "Rpg.Client.Resources.Dialogues";

            var dialogueSourcePath = RESOURCE_PATH + "." + resourceSid + ".json";

            using var stream = assembly.GetManifestResourceStream(dialogueSourcePath);

            if (stream is not null)
            {
                using var reader = new StreamReader(stream);
                var json = reader.ReadToEnd();

                return json;
            }
            else
            {
                throw new InvalidOperationException($"Dialogue resource with sid {resourceSid} not found");
            }
        }
    }
}