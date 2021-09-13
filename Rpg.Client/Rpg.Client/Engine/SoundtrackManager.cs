using System;

using Microsoft.Xna.Framework.Media;

namespace Rpg.Client.Engine
{
    internal sealed class SoundtrackManager
    {
        private bool _backgroundTrackStarted;

        private string? _state;
        private IUiContentStorage _uiContentStorage;

        public bool IsInitialized { get; private set; }

        public void Initialize(IUiContentStorage uiContentStorage)
        {
            _uiContentStorage = uiContentStorage;
            IsInitialized = true;
        }

        public void PlayBackgroundTrack()
        {
            _state = "title";
        }

        public void PlaySilence()
        {
            _state = null; // means silence.
        }

        public void Update()
        {
            if (!IsInitialized)
            {
                return;
            }

            switch (_state)
            {
                case null:
                    _backgroundTrackStarted = false;
                    MediaPlayer.Stop();
                    break;

                case "title":
                    if (!_backgroundTrackStarted)
                    {
                        _backgroundTrackStarted = true;
                        if (MediaPlayer.State != MediaState.Playing)
                        {
                            if (_uiContentStorage is not null)
                            {
                                MediaPlayer.IsRepeating = true;
                                MediaPlayer.Volume = 0.75f;
                                MediaPlayer.Play(_uiContentStorage.GetTitleSong(), TimeSpan.Zero);
                            }
                        }
                    }

                    break;

                case "map":
                    if (!_backgroundTrackStarted)
                    {
                        _backgroundTrackStarted = true;
                        if (MediaPlayer.State != MediaState.Playing)
                        {
                            if (_uiContentStorage is not null)
                            {
                                MediaPlayer.IsRepeating = true;
                                MediaPlayer.Volume = 0.75f;
                                MediaPlayer.Play(_uiContentStorage.GetMapSong(), TimeSpan.Zero);
                            }
                        }
                    }

                    break;

                case "battle":
                    if (!_backgroundTrackStarted)
                    {
                        _backgroundTrackStarted = true;
                        if (MediaPlayer.State != MediaState.Playing)
                        {
                            if (_uiContentStorage is not null)
                            {
                                MediaPlayer.IsRepeating = true;
                                MediaPlayer.Volume = 0.75f;
                                MediaPlayer.Play(_uiContentStorage.GetBattleSong(), TimeSpan.Zero);
                            }
                        }
                    }

                    break;
            }
        }
    }
}
