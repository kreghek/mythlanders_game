using System.Reflection;
using System.Resources;

using Client.Core;

namespace Client.GameScreens.TextDialogue.Ui;

public static class SpeechVisualizationHelper
{
    private const int MAX_IN_LINE = 60;

    private static readonly ResourceManager _dialogueResourceManager;

    static SpeechVisualizationHelper()
    {
        var assembly = Assembly.GetExecutingAssembly();

        _dialogueResourceManager = new ResourceManager("Client.DialogueResources", assembly);
    }

    public static (string text, bool isLocalized) PrepareLocalizedText(string dialogueResourceSid)
    {
        var (fullText, isLocalized) = GetLocalizedText(dialogueResourceSid);
        var fixedFullText = StringHelper.FixText(fullText);
        var wordBreakFullText = StringHelper.LineBreaking(fixedFullText, MAX_IN_LINE);

        return (wordBreakFullText, isLocalized);
    }

    private static (string text, bool isLocalized) GetLocalizedText(string textSid)
    {
        var localizedText = _dialogueResourceManager.GetString(textSid);
        if (localizedText is not null)
        {
            return (localizedText, true);
        }

        return ($"#{textSid}", false);
    }
}