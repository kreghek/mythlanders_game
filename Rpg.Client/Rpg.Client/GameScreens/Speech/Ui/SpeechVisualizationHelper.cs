using System.Reflection;
using System.Resources;

using Rpg.Client.Core;

namespace Rpg.Client.GameScreens.Speech.Ui
{
    public static class SpeechVisualizationHelper
    {
        private const int MAX_IN_LINE = 60;
        
        public static string PrepareLocalizedText(string dialogueResourceSid)
        {
            var fullText = GetLocalizedText(dialogueResourceSid);
            var fixedFullText = StringHelper.FixText(fullText);
            var wordBreakFullText = StringHelper.LineBreaking(fixedFullText, MAX_IN_LINE);

            return wordBreakFullText;
        }
        
        private static string GetLocalizedText(string textSid)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var rm = new ResourceManager("Rpg.Client.DialogueResources", assembly);

            var localizedText = rm.GetString(textSid);
            return localizedText ?? $"#{textSid}";
        }
    }
}