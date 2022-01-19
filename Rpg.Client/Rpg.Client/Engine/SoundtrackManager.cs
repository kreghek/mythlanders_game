using System;
using System.Linq;

using Microsoft.Xna.Framework.Media;

using Rpg.Client.Core;

namespace Rpg.Client.Engine
{
    internal sealed class SoundtrackManager
    {
        private const float MUSIC_VOLUME = 0.5f;
        private const double DURATION_MS = 2051; // 117 bmp https://toolstud.io/music/bpm.php?bpm=117&bpm_unit=4%2F4

        private readonly Random _random;

        private bool _changeTrack;

        private double _counter;

        private BiomeType _currentBiome;
        private Song? _currentSong;

        private string? _state;

        // this is 2051 * 4 * (1 + 2 + 2 + 2) full length
        private bool _transition;
        private IUiContentStorage? _uiContentStorage;

        public SoundtrackManager()
        {
            _random = new Random();
        }

        internal void PlayIntroTrack()
        {
            ChangeState("intro");
        }

        public string? CurrentTrackName => _currentSong?.Name;

        public bool IsInitialized { get; private set; }

        public void Initialize(IUiContentStorage uiContentStorage)
        {
            _uiContentStorage = uiContentStorage;
            IsInitialized = true;
        }

        public void PlayBattleTrack(BiomeType type)
        {
            _currentBiome = type;
            ChangeState("battle");
        }

        public void PlayDefeatTrack()
        {
            _transition = true;
            ChangeState("defeat");
        }

        public void PlayMapTrack()
        {
            ChangeState("map");
        }

        public void PlaySilence()
        {
            _state = null; // means silence.
        }

        public void PlayTitleTrack()
        {
            ChangeState("title");
        }

        public void PlayVictoryTrack()
        {
            _transition = true;
            ChangeState("victory");
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (!IsInitialized)
            {
                return;
            }

            switch (_state)
            {
                case null:
                    _currentSong = null;
                    MediaPlayer.Stop();
                    break;

                case "intro":
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

                case "title":
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

                case "map":
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

                case "battle":
                    if (_changeTrack)
                    {
                        _changeTrack = false;

                        if (_uiContentStorage is not null)
                        {
                            var songs = _uiContentStorage.GetBattleSongs(_currentBiome).ToArray();
                            var soundIndex = _random.Next(0, songs.Length);
                            var song = songs[soundIndex];

                            _currentSong = song;

                            MediaPlayer.IsRepeating = true;
                            MediaPlayer.Volume = MUSIC_VOLUME;
                            MediaPlayer.Play(song, TimeSpan.Zero);
                        }
                    }

                    break;

                case "victory":
                    if (_changeTrack)
                    {
                        if (_transition)
                        {
                            _counter += gameTime.ElapsedGameTime.TotalMilliseconds;
                            if (_counter < DURATION_MS * 4 /* * (1 + 2 + 2 + 2 + 2)*/)
                            {
                                return;
                            }

                            _counter = 0;
                        }

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

                case "defeat":
                    if (_changeTrack)
                    {
                        if (_transition)
                        {
                            _counter += gameTime.ElapsedGameTime.TotalMilliseconds;
                            if (_counter < DURATION_MS * 4 /* * (1 + 2 + 2 + 2 + 2)*/)
                            {
                                return;
                            }

                            _counter = 0;
                        }

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

        private void ChangeState(string targetState)
        {
            if (_state != targetState)
            {
                _changeTrack = true;
            }

            _state = targetState;
        }
    }
}