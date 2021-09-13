using System;

using Microsoft.Xna.Framework.Media;

namespace Rpg.Client.Engine
{
    internal sealed class SoundtrackManager
    {
        private bool _backgroundTrackStarted;
        private bool _changeTrack;

        private string? _state;
        private IUiContentStorage _uiContentStorage;

        public bool IsInitialized { get; private set; }

        public void Initialize(IUiContentStorage uiContentStorage)
        {
            _uiContentStorage = uiContentStorage;
            IsInitialized = true;
        }

        public void PlayTitleTrack()
        {
            ChangeState("title");
        }

        private void ChangeState(string targetState)
        {
            if (_state != targetState)
            {
                _changeTrack = true;
            }

            _state = targetState;
        }

        public void PlayMapTrack()
        {
            ChangeState("map");
        }

        public void PlayBattleTrack()
        {
            ChangeState("battle");
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
                    if (_changeTrack)
                    {
                        _backgroundTrackStarted = true;
                        _changeTrack = false;

                        if (_uiContentStorage is not null)
                        {
                            MediaPlayer.IsRepeating = true;
                            MediaPlayer.Volume = 0.75f;
                            MediaPlayer.Play(_uiContentStorage.GetTitleSong(), TimeSpan.Zero);
                        }

                    }

                    break;

                case "map":
                    if (_changeTrack)
                    {
                        _backgroundTrackStarted = true;
                        _changeTrack = false;

                        if (_uiContentStorage is not null)
                        {
                            MediaPlayer.IsRepeating = true;
                            MediaPlayer.Volume = 0.75f;
                            MediaPlayer.Play(_uiContentStorage.GetMapSong(), TimeSpan.Zero);
                        }

                    }

                    break;

                case "battle":
                    if (_changeTrack)
                    {
                        _backgroundTrackStarted = true;
                        _changeTrack = false;

                        if (_uiContentStorage is not null)
                        {
                            MediaPlayer.IsRepeating = true;
                            MediaPlayer.Volume = 0.75f;
                            MediaPlayer.Play(_uiContentStorage.GetBattleSong(), TimeSpan.Zero);
                        }

                    }

                    break;
            }
        }
    }
}
