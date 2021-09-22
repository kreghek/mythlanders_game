﻿using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Rpg.Client.Engine
{
    internal interface IUiSoundStorage
    {
        bool ContentWasLoaded { get; }

        SoundEffect GetButtonClickEffect();
        SoundEffect GetButtonHoverEffect();

        void LoadContent(ContentManager contentManager);
    }
}