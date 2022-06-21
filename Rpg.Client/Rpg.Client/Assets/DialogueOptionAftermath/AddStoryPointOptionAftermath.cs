﻿using Rpg.Client.Core;

namespace Rpg.Client.Assets.DialogueOptionAftermath
{
    internal class AddStoryPointOptionAftermath : IOptionAftermath
    {
        private readonly string _storyPointSid;

        public AddStoryPointOptionAftermath(string storyPointSid)
        {
            _storyPointSid = storyPointSid;
        }

        public void Apply(IEventContext dialogContext)
        {
            dialogContext.AddStoryPoint(_storyPointSid);
        }
    }
}
