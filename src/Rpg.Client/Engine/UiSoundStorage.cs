using System;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Rpg.Client.Engine
{
    internal sealed class UiSoundStorage : IUiSoundStorage
    {
        private SoundEffect? _buttonClickSoundEffect;
        private SoundEffect? _buttonHoverSoundEffect;

        public bool ContentWasLoaded { get; private set; }

        public SoundEffect GetButtonClickEffect()
        {
            return _buttonClickSoundEffect ?? throw new InvalidOperationException("Sound must be loaded before using.");
        }

        public SoundEffect GetButtonHoverEffect()
        {
            return _buttonHoverSoundEffect ?? throw new InvalidOperationException("Sound must be loaded before using.");
        }

        public void LoadContent(ContentManager contentManager)
        {
            _buttonClickSoundEffect = contentManager.Load<SoundEffect>("Audio/Ui/ButtonClick");
            _buttonHoverSoundEffect = contentManager.Load<SoundEffect>("Audio/Ui/ButtonHover");

            ContentWasLoaded = true;
        }
    }
}