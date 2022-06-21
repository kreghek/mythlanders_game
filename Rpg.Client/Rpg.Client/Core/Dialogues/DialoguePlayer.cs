using System.Collections.Generic;
using System.Linq;

using Rpg.Client.GameScreens.Speech;

namespace Rpg.Client.Core.Dialogues
{
    internal sealed class DialoguePlayer
    {
        private EventNode _currentNode;
        private readonly DialogueContextFactory _contextFactory;

        public DialoguePlayer(Dialogue dialogue, DialogueContextFactory contextFactory)
        {
            _currentNode = dialogue.Root;
            _contextFactory = contextFactory;

            CurrentTextFragments = _currentNode.TextBlock.Fragments;
            CurrentOptions = _currentNode.Options.OrderBy(x => x.TextSid).ToArray();
        }

        public IReadOnlyList<EventTextFragment> CurrentTextFragments { get; private set; }

        public IReadOnlyCollection<EventOption> CurrentOptions { get; private set; }

        public void SelectOption(EventOption option)
        {
            _currentNode = option.Next;
            
            CurrentTextFragments = _currentNode.TextBlock.Fragments;
            CurrentOptions = _currentNode.Options.OrderBy(x => x.TextSid).ToArray();

            var context = _contextFactory.Create();
            option.Aftermath?.Apply(context);
        }

        public bool IsEnd => _currentNode == EventNode.EndNode;
    }
}