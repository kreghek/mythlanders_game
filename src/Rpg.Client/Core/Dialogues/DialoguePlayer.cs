using System;
using System.Collections.Generic;
using System.Linq;

using Rpg.Client.GameScreens.Speech;

namespace Rpg.Client.Core.Dialogues
{
    internal sealed class DialoguePlayer
    {
        private readonly DialogueContextFactory _contextFactory;
        private EventNode _currentNode;

        public DialoguePlayer(Dialogue dialogue, DialogueContextFactory contextFactory)
        {
            _currentNode = dialogue.Root;
            _contextFactory = contextFactory;

            CurrentTextFragments = _currentNode.TextBlock.Fragments;
            CurrentOptions = _currentNode.Options.ToArray();
        }

        public IReadOnlyCollection<EventOption> CurrentOptions { get; private set; }

        public IReadOnlyList<EventTextFragment> CurrentTextFragments { get; private set; }

        public bool IsEnd => _currentNode == EventNode.EndNode;

        public void SelectOption(EventOption option)
        {
            _currentNode = option.Next;

            if (_currentNode != EventNode.EndNode)
            {
                CurrentTextFragments = _currentNode.TextBlock.Fragments;
                CurrentOptions = _currentNode.Options.ToArray();
            }
            else
            {
                CurrentTextFragments = ArraySegment<EventTextFragment>.Empty;
                CurrentOptions = ArraySegment<EventOption>.Empty;
            }

            var context = _contextFactory.Create();
            option.Aftermath?.Apply(context);
        }
    }
}