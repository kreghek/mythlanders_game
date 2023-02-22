using System;
using System.Collections.Generic;
using System.Linq;

using Client.Core.Dialogues;

using Rpg.Client.GameScreens.Speech;

namespace Rpg.Client.Core.Dialogues
{
    internal sealed class DialoguePlayer
    {
        private readonly DialogueContextFactory _contextFactory;
        private DialogueNode _currentNode;

        public DialoguePlayer(Dialogue dialogue, DialogueContextFactory contextFactory)
        {
            _currentNode = dialogue.Root;
            _contextFactory = contextFactory;

            CurrentTextFragments = _currentNode.TextBlock.Fragments;
            CurrentOptions = _currentNode.Options.ToArray();
        }

        public IReadOnlyCollection<DialogueOption> CurrentOptions { get; private set; }

        public IReadOnlyList<EventTextFragment> CurrentTextFragments { get; private set; }

        public bool IsEnd => _currentNode == DialogueNode.EndNode;

        public void SelectOption(DialogueOption option)
        {
            _currentNode = option.Next;

            if (_currentNode != DialogueNode.EndNode)
            {
                CurrentTextFragments = _currentNode.TextBlock.Fragments;
                CurrentOptions = _currentNode.Options.ToArray();
            }
            else
            {
                CurrentTextFragments = ArraySegment<EventTextFragment>.Empty;
                CurrentOptions = ArraySegment<DialogueOption>.Empty;
            }

            var context = _contextFactory.Create();
            option.Aftermath?.Apply(context);
        }
    }
}