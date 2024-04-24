using Client.Assets.Catalogs.Dialogues;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.Catalogs.DialogueStoring;

internal interface IDialogueParagraphEffectCreator
{
    IDialogueOptionAftermath<CampaignAftermathContext> Create(string typeSid, string data);
}