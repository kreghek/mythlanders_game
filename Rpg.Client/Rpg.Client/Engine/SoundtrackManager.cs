using System;
using System.Linq;

using Microsoft.Xna.Framework.Media;

namespace Rpg.Client.Engine
{
    internal sealed class SoundtrackManager
    {
        private readonly Random _random;

        private bool _changeTrack;
        private Song? _currentSong;

        private string? _state;
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

        public void PlayBattleTrack()
        {
            ChangeState("battle");
        }

        public void PlayDefeatTrack()
        {
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

                case "title":
                    if (_changeTrack)
                    {
                        _changeTrack = false;

                        if (_uiContentStorage is not null)
                        {
                            MediaPlayer.IsRepeating = true;
                            MediaPlayer.Volume = 0.75f;
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
                            MediaPlayer.Volume = 0.75f;
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
                            var songs = _uiContentStorage.GetBattleSongs().ToArray();
                            var soundIndex = _random.Next(0, songs.Length);
                            var song = songs[soundIndex];

                            _currentSong = song;

                            MediaPlayer.IsRepeating = true;
                            MediaPlayer.Volume = 0.75f;
                            MediaPlayer.Play(song, TimeSpan.Zero);
                        }
                    }

                    //_counter += gameTime.ElapsedGameTime.TotalMilliseconds;
                    //if (_counter >= DURATION_MS * 4/* * (1 + 2 + 2 + 2 + 2)*/)
                    //{
                    //    _counter = 0;

                    //    PlayVictoryTrack();
                    //}

                    break;

                case "victory":
                    if (_changeTrack)
                    {
                        _changeTrack = false;

                        if (_uiContentStorage is not null)
                        {
                            var song = _uiContentStorage.GetVictorySong();

                            _currentSong = song;

                            MediaPlayer.IsRepeating = false;
                            MediaPlayer.Volume = 0.75f;
                            MediaPlayer.Play(song, TimeSpan.Zero);
                        }
                    }

                    break;

                case "defeat":
                    if (_changeTrack)
                    {
                        _changeTrack = false;

                        if (_uiContentStorage is not null)
                        {
                            var song = _uiContentStorage.GetDefeatSong();

                            _currentSong = song;

                            MediaPlayer.IsRepeating = false;
                            MediaPlayer.Volume = 0.75f;
                            MediaPlayer.Play(song, TimeSpan.Zero);
                        }
                    }

                    break;
            }
        }

        //private double _counter;
        //private const double DURATION_MS = 2051;  // 117 bmp https://vk.com/away.php?to=https%3A%2F%2Ftoolstud.io%2Fmusic%2Fbpm.php%3Fbpm%3D117%26bpm_unit%3D4%252F4&cc_key=
        // this is 2051 * 4 * (1 + 2 + 2 + 2) full length

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