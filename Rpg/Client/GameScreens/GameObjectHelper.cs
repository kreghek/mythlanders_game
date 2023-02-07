using Client;
using Client.Core;

using Rpg.Client.Core;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.GameScreens
{
    internal class GameObjectHelper
    {
        public static string GetLocalized(UnitName unitName)
        {
            return GetLocalizedInner(unitName.ToString());
        }

        public static string GetLocalized(EquipmentSid equipmentSid)
        {
            return GetLocalizedInner(equipmentSid.ToString());
        }

        public static string GetLocalized(IPerk perk)
        {
            return GetLocalizedInner(perk.GetType().Name);
        }

        public static string GetLocalized(SkillSid skillSid)
        {
            return GetLocalizedInner(skillSid.ToString());
        }

        public static string GetLocalized(GlobeNodeSid locationSid)
        {
            return GetLocalizedInner(locationSid.ToString());
        }

        public static string? GetLocalized(EquipmentItemType? equipmentType)
        {
            if (equipmentType is null)
            {
                return null;
            }

            if (equipmentType == EquipmentItemType.ExperiencePoints)
            {
                return "Xp";
            }

            return GetLocalizedInner($"{equipmentType}Equipment");
        }

        public static string GetLocalizedDescription(IPerk perk)
        {
            return GetLocalizedInner($"{perk.GetType().Name}Description");
        }

        public static string GetLocalizedDescription(EquipmentSid equipmentSid)
        {
            return GetLocalizedInner($"{equipmentSid}Description");
        }

        private static string GetLocalizedInner(string sid)
        {
            var rm = GameObjectResources.ResourceManager;
            var name = rm.GetString(sid) ?? sid;
            return name;
        }
    }
}