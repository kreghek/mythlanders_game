using System;
using System.Collections.Generic;
using System.Linq;

using Client.Core.Dialogues;
using Client.GameScreens.TextDialogue;

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

            var context = _contextFactory.Create();
            var conditionContext = new DialogueParagraphConditionContext(context);
            CurrentTextFragments = GetTextBlockParagraphs(conditionContext);
            CurrentOptions = _currentNode.Options.ToArray();
        }

        public IReadOnlyCollection<DialogueOption> CurrentOptions { get; private set; }

        public IReadOnlyList<DialogueParagraph> CurrentTextFragments { get; private set; }

        public bool IsEnd => _currentNode == DialogueNode.EndNode;

        public void SelectOption(DialogueOption option)
        {
            var context = _contextFactory.Create();
            
            _currentNode = option.Next;

            if (_currentNode != DialogueNode.EndNode)
            {
                var conditionContext = new DialogueParagraphConditionContext(context);
                CurrentTextFragments = GetTextBlockParagraphs(conditionContext);
                CurrentOptions = _currentNode.Options.ToArray();
            }
            else
            {
                CurrentTextFragments = ArraySegment<DialogueParagraph>.Empty;
                CurrentOptions = ArraySegment<DialogueOption>.Empty;
            }

            option.Aftermath?.Apply(context);
        }

        private IReadOnlyList<DialogueParagraph> GetTextBlockParagraphs(
            DialogueParagraphConditionContext dialogueParagraphConditionContext)
        {
            var paragraphs = _currentNode.TextBlock.Paragraphs
                .Where(x => x.Conditions.All(c => c.Check(dialogueParagraphConditionContext))).ToArray();

            return paragraphs;
        }
    }
}