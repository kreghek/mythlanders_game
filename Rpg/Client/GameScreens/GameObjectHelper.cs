using Client;
using Client.Core;

using Core.Combats;

using Rpg.Client.Core;

namespace Rpg.Client.GameScreens;

internal static class GameObjectHelper
{
    public static string GetLocalized(UnitName unitName)
    {
        return GetLocalizedInner(unitName.ToString());
    }

    public static string GetLocalized(CombatMovementSid sid)
    {
        return GetLocalizedInner(sid.Value);
    }

    public static string GetLocalized(CharacterRelation relation)
    {
        if (relation.Level == CharacterKnowledgeLevel.FullName)
        {
            return GetLocalized(relation.Name);
        }

        return GetLocalizedInner($"{relation.Name}_{relation.Level}");
    }

    public static string GetLocalized(EquipmentSid equipmentSid)
    {
        return GetLocalizedInner(equipmentSid.ToString());
    }

    public static string GetLocalized(IPerk perk)
    {
        return GetLocalizedInner(perk.GetType().Name);
    }

    public static string GetLocalized(LocationSid locationSid)
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

    public static string GetLocalizedDescription(CombatMovementSid sid)
    {
        return GetLocalizedInner($"{sid.Value}_Description");
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