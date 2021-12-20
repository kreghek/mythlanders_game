using System;

namespace Rpg.Client.Engine
{
    internal static class UiThemeManager
    {
        private static IUiContentStorage? _uiContentStorage;
        private static IUiSoundStorage? _soundStorage;

        public static IUiSoundStorage? SoundStorage
        {
            get => _soundStorage;
            set
            {
                if (_soundStorage?.ContentWasLoaded == false)
                {
                    throw new InvalidOperationException(
                        "Sound Storage must load content before assigning in Ui theme manager.");
                }

                _soundStorage = value;
            }
        }

        public static IUiContentStorage? UiContentStorage
        {
            get => _uiContentStorage;
            set
            {
                _uiContentStorage = value;
            }
        }
    }
}