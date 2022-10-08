using System.Diagnostics;

using Client;

namespace Rpg.Client.GameScreens.Map
{
    internal static class BiomeScreenTextHelper
    {
        public static float GetCombatSequenceSizeBonus(int combatSize)
        {
            switch (combatSize)
            {
                case 1:
                    return 1;

                case 3:
                    return 1.25f;

                case 5:
                    return 1.5f;

                default:
                    Debug.Fail("Unknown size");
                    return 1;
            }
        }

        public static string GetCombatSequenceSizeText(int combatSize)
        {
            var bonus = GetCombatSequenceSizeBonus(combatSize);
            var bonusInPercents = bonus * 100;

            switch (combatSize)
            {
                case 1:
                    return UiResource.ShortCombatSequenceText;

                case 3:
                    return $"{UiResource.MediumCombatSequenceText} (+{bonusInPercents}% {UiResource.XpRewardText})";

                case 5:
                    return $"{UiResource.LongCombatSequenceText} (+{bonusInPercents}% {UiResource.XpRewardText})";

                default:
                    Debug.Fail("Unknown size");
                    return string.Empty;
            }
        }
    }
}