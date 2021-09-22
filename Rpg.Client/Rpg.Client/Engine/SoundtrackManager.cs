﻿using System;
using System.Linq;

using Microsoft.Xna.Framework.Media;

namespace Rpg.Client.Engine
{
    internal sealed class SoundtrackManager
    {
        private readonly Random _random;

        private bool _changeTrack;

        private string? _state;
        private IUiContentStorage? _uiContentStorage;
        private Song? _currentSong;

        public SoundtrackManager()
        {
            _random = new Random();
        }

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

        public void Update()
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
                            var song = songs[_random.Next(0, songs.Length)];

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

                    break;
            }
        }

        public string? CurrentTrackName => _currentSong?.Name;

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