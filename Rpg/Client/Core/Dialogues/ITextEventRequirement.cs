namespace Rpg.Client.Core.Dialogues
{
    internal interface ITextEventRequirement
    {
        bool IsApplicableFor(Globe globe, GlobeNode targetNode);
    }
}