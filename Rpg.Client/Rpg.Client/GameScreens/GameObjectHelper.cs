﻿using Rpg.Client.Core;

namespace Rpg.Client.GameScreens
{
    internal class GameObjectHelper
    {
        public static string GetLocalized(UnitName unitName)
        {
            return GetLocalizedInner(unitName.ToString());
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

            return GetLocalizedInner($"{equipmentType}Equipment");
        }

        private static string GetLocalizedInner(string sid)
        {
            var rm = GameObjectResources.ResourceManager;
            var name = rm.GetString(sid) ?? sid;
            return name;
        }
    }
}