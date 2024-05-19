using System;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Client.GameScreens;

public static class LocalizationHelper
{
    public static void SetLanguage(string twoLetters)
    {
        var langSequence = new[]
        {
            (TwoLetters: "ru", Culture: CultureInfo.GetCultureInfo("ru-RU")),
            (TwoLetters: "en", Culture: CultureInfo.GetCultureInfo("en-US")),
            (TwoLetters: "zh", Culture: CultureInfo.GetCultureInfo("zh"))
        };

        var newCulture = langSequence.FirstOrDefault(x => x.TwoLetters == twoLetters).Culture;

        Thread.CurrentThread.CurrentCulture = newCulture;
        Thread.CurrentThread.CurrentUICulture = newCulture;
    }

    public static void SwitchLanguage()
    {
        var langSequence = new[]
        {
            (TwoLetters: "ru", Culture: CultureInfo.GetCultureInfo("ru-RU")),
            (TwoLetters: "en", Culture: CultureInfo.GetCultureInfo("en-US")),
            (TwoLetters: "zh", Culture: CultureInfo.GetCultureInfo("zh"))
        };

        var currentLanguage = Thread.CurrentThread.CurrentUICulture;

        int GetCurrentLangIndex(CultureInfo currentLang)
        {
            for (var i = 0; i < langSequence.Length; i++)
            {
                if (string.Equals(
                        currentLang.TwoLetterISOLanguageName,
                        langSequence[i].TwoLetters,
                        StringComparison.InvariantCultureIgnoreCase))
                {
                    return i;
                }
            }

            return 0;
        }

        var currentLangIndex = GetCurrentLangIndex(currentLanguage);
        currentLangIndex++;
        if (currentLangIndex > langSequence.Length - 1)
        {
            currentLangIndex = 0;
        }

        var newCulture = langSequence[currentLangIndex].Culture;

        Thread.CurrentThread.CurrentCulture = newCulture;
        Thread.CurrentThread.CurrentUICulture = newCulture;
    }
}