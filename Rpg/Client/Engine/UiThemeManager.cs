using System;

namespace Rpg.Client.Engine
{
    internal static class UiThemeManager
    {
        private static IUiSoundStorage? _soundStorage;
        private static IUiContentStorage? _uiContentStorage;

        public static IUiSoundStorage SoundStorage
        {
            get => _soundStorage ?? throw new InvalidOperationException(
                $"{nameof(IUiSoundStorage)} must load content before assigning in Ui theme manager.");
            set
            {
                if (value?.ContentWasLoaded != true)
                {
                    throw new InvalidOperationException(
                        $"{nameof(IUiSoundStorage)} must load content before assigning in Ui theme manager.");
                }

                _soundStorage = value;
            }
        }

        public static IUiContentStorage UiContentStorage
        {
            get => _uiContentStorage ?? throw new InvalidOperationException(
                $"{nameof(IUiSoundStorage)} must load content before assigning in Ui theme manager.");
            set
            {
                if (value?.ContentWasLoaded != true)
                {
                    throw new InvalidOperationException(
                        $"{nameof(IUiContentStorage)} must load content before assigning in Ui theme manager.");
                }

                _uiContentStorage = value;
            }
        }
    }
}