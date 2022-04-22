using System;
using System.Linq;

using Microsoft.Xna.Framework.Media;

using Rpg.Client.Core;

namespace Rpg.Client.Engine
{
    internal sealed class SoundtrackManager
    {
        private readonly Random _random;
        private readonly float MUSIC_VOLUME = 1.0f;

        private bool _changeTrack;

        private BiomeType _currentBiome;
        private Song? _currentSong;

        private SoundtrackType _state;

        private IUiContentStorage? _uiContentStorage;

        public SoundtrackManager()
        {
            _random = new Random();
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
                            MediaPlayer.Volume = MUSIC_VOLUME;
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
                            MediaPlayer.Volume = MUSIC_VOLUME;
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
                            MediaPlayer.Volume = MUSIC_VOLUME;
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
                            MediaPlayer.Volume = MUSIC_VOLUME;
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
                            MediaPlayer.Volume = MUSIC_VOLUME;
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
                            MediaPlayer.Volume = MUSIC_VOLUME;
                            MediaPlayer.Play(song, TimeSpan.Zero);
                        }
                    }

                    break;
            }
        }

        internal void PlayIntroTrack()
        {
            ChangeState(SoundtrackType.Intro);
        }

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