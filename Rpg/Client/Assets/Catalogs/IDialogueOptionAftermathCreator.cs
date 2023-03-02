using Rpg.Client.Core.Dialogues;

namespace Rpg.Client.Assets.Catalogs
{
    internal interface IDialogueOptionAftermathCreator
    {
        IDialogueOptionAftermath Create(string aftermathTypeSid, string data);
    }
}