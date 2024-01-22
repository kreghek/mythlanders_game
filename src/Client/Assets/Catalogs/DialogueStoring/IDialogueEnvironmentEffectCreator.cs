using Client.Assets.Catalogs.Dialogues;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.Catalogs.DialogueStoring;

internal interface IDialogueEnvironmentEffectCreator
{
    IDialogueOptionAftermath<CampaignAftermathContext> Create(string typeSid, string data);
}