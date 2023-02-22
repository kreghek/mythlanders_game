using System;
using System.Linq;

using Microsoft.Xna.Framework.Media;

using Rpg.Client.Core;

namespace Rpg.Client.Engine
{
    internal sealed class SoundtrackManager
    {
        private readonly GameSettings _gameSettings;
        private readonly Random _random;
        private bool _changeTrack;

        private BiomeType _currentBiome;
        private Song? _currentSong;

        private SoundtrackType _state;

        private IUiContentStorage? _uiContentStorage;

        public SoundtrackManager(GameSettings gameSettings)
        {
            _random = new Random();
            _gameSettings = gameSettings;
        }

        public string? CurrentTrackName => _currentSong?.Name;

        public bool IsInitialized { get; private set; }

        public void Initialize(IUiContentStorage uiContentStorage)
        {
            _uiContentStorage = uiContentStorage;
            IsInitialized = true;
        }

        public void PlayCombatTrack(BiomeType type)
        {
            _currentBiome = type;
            ChangeState(SoundtrackType.Combat);
        }

        public void PlayDefeatTrack()
        {
            ChangeState(SoundtrackType.Defeat);
        }

        public void PlayMapTrack()
        {
            ChangeState(SoundtrackType.Map);
        }

        public void PlaySilence()
        {
            _state = SoundtrackType.Silence;
        }

        public void PlayTitleTrack()
        {
            ChangeState(SoundtrackType.Title);
        }

        public void PlayVictoryTrack()
        {
            ChangeState(SoundtrackType.Victory);
        }

        public void Update()
        {
            if (!IsInitialized)
            {
                return;
            }

            MediaPlayer.Volume = _gameSettings.MusicVolume;

            switch (_state)
            {
                case SoundtrackType.Silence:
                    _currentSong = null;
                    MediaPlayer.Stop();
                    break;

                case SoundtrackType.Intro:
                    if (_changeTrack)
                    {
                        _changeTrack = false;

                        if (_uiContentStorage is not null)
                        {
                            MediaPlayer.IsRepeating = true;
                            _currentSong = _uiContentStorage.GetIntroSong();
                            MediaPlayer.Play(_currentSong, TimeSpan.Zero);
                        }
                    }

                    break;

                case SoundtrackType.Title:
                    if (_changeTrack)
                    {
                        _changeTrack = false;

                        if (_uiContentStorage is not null)
                        {
                            MediaPlayer.IsRepeating = true;
                            _currentSong = _uiContentStorage.GetTitleSong();
                            MediaPlayer.Play(_currentSong, TimeSpan.Zero);
                        }
                    }

                    break;

                case SoundtrackType.Map:
                    if (_changeTrack)
                    {
                        _changeTrack = false;

                        if (_uiContentStorage is not null)
                        {
                            var songs = _uiContentStorage.GetMapSong().ToArray();
                            var soundIndex = _random.Next(0, songs.Length);
                            var song = songs[soundIndex];

                            _currentSong = song;

                            MediaPlayer.IsRepeating = true;
                            MediaPlayer.Play(song, TimeSpan.Zero);
                        }
                    }

                    break;

                case SoundtrackType.Combat:
                    if (_changeTrack)
                    {
                        _changeTrack = false;

                        if (_uiContentStorage is not null)
                        {
                            var songDataList = _uiContentStorage.GetCombatSongs(_currentBiome).ToArray();
                            var soundIndex = _random.Next(0, songDataList.Length);
                            var songData = songDataList[soundIndex];

                            _currentSong = songData.Soundtrack;

                            MediaPlayer.IsRepeating = true;
                            MediaPlayer.Play(_currentSong, TimeSpan.Zero);
                        }
                    }

                    break;

                case SoundtrackType.Victory:
                    if (_changeTrack)
                    {
                        _changeTrack = false;

                        if (_uiContentStorage is not null)
                        {
                            var song = _uiContentStorage.GetVictorySong();

                            _currentSong = song;

                            MediaPlayer.IsRepeating = false;
                            MediaPlayer.Play(song, TimeSpan.Zero);
                        }
                    }

                    break;

                case SoundtrackType.Defeat:
                    if (_changeTrack)
                    {
                        _changeTrack = false;

                        if (_uiContentStorage is not null)
                        {
                            var song = _uiContentStorage.GetDefeatSong();

                            _currentSong = song;

                            MediaPlayer.IsRepeating = false;
                            MediaPlayer.Play(song, TimeSpan.Zero);
                        }
                    }

                    break;

                case SoundtrackType.Custom:
                    if (_changeTrack)
                    {
                        _changeTrack = false;

                        if (_uiContentStorage is not null && _customSong is not null)
                        {
                            _currentSong = _customSong;

                            MediaPlayer.IsRepeating = false;
                            MediaPlayer.Play(_customSong, TimeSpan.Zero);
                        }
                    }

                    break;
            }
        }

        internal void PlayIntroTrack()
        {
            ChangeState(SoundtrackType.Intro);
        }

        internal void PlayCustomTrack(Song song)
        {
            MediaPlayer.Stop();
            _customSong = song;
            ChangeState(SoundtrackType.Custom);
        }

        private Song? _customSong;

        private void ChangeState(SoundtrackType targetState)
        {
            if (_state != targetState)
            {
                _changeTrack = true;
            }

            _state = targetState;
        }
    }
}