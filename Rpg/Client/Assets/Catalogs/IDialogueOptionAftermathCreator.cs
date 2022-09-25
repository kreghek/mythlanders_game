using Rpg.Client.Core.Dialogues;

namespace Rpg.Client.Assets.Catalogs
{
    internal interface IDialogueOptionAftermathCreator
    {
        IOptionAftermath Create(string aftermathTypeSid, string data);
    }
}