using Client.Core;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Dialogues;

using Core.Crises;

namespace Client.GameScreens;

internal static class GameObjectHelper
{
    public static string GetLocalized(UnitName unitName)
    {
        return GetLocalizedInner(unitName.ToString());
    }

    public static string GetLocalized(IDialogueSpeaker unitName)
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
            return GetLocalized(relation.Character);
        }

        return GetLocalizedInner($"{relation.Character}_{relation.Level}");
    }

    public static string GetLocalized(EquipmentSid equipmentSid)
    {
        return GetLocalizedInner(equipmentSid.ToString());
    }

    public static string GetLocalized(IPerk perk)
    {
        return GetLocalizedInner(perk.GetType().Name);
    }

    public static string GetLocalized(ILocationSid locationSid)
    {
        return GetLocalizedInner(locationSid.ToString() ?? string.Empty);
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

    public static string GetLocalized(CrisisSid crisisSid)
    {
        return GetLocalizedInner(crisisSid.ResourceName);
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

    public static string GetLocalizedProp(string sid)
    {
        return GetLocalizedInner($"{sid}_prop");
    }

    public static string GetLocalizedVoiceCombatMove(string sid)
    {
        return GetLocalizedInner($"{sid}_voice");
    }

    internal static string GetLocalized(ICombatantStatusSid sid)
    {
        return GetLocalizedInner(sid.ToString() ?? "Unknown");
    }

    private static string GetLocalizedInner(string sid)
    {
        var rm = GameObjectResources.ResourceManager;
        var name = rm.GetString(sid) ?? sid;
        return name;
    }
}