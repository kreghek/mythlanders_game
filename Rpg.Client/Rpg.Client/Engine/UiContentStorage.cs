﻿using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

using Rpg.Client.Core;

namespace Rpg.Client.Engine
{
    internal sealed class UiContentStorage : IUiContentStorage
    {
        private Song _battleTrack;
        private Dictionary<BiomeType, Texture2D> _biomeBackgroundDict;
        private Texture2D _buttonTexture;
        private SpriteFont _font;
        private Song _mapTrack;
        private Texture2D[] _modalBottomTextures;
        private Texture2D _modalShadowTexture;
        private Texture2D[] _modalTopTextures;
        private Song _titleTrack;

        public Texture2D GetButtonTexture()
        {
            return _buttonTexture;
        }

        public SpriteFont GetMainFont()
        {
            return _font;
        }

        public Texture2D[] GetModalBottomTextures()
        {
            return _modalBottomTextures ?? throw new InvalidOperationException();
        }

        public Texture2D[] GetModalTopTextures()
        {
            return _modalTopTextures ?? throw new InvalidOperationException();
        }

        public Texture2D GetModalShadowTexture()
        {
            return _modalShadowTexture ?? throw new InvalidOperationException();
        }

        public void LoadContent(ContentManager contentManager)
        {
            _buttonTexture = contentManager.Load<Texture2D>("Sprites/Ui/Button");
            _font = contentManager.Load<SpriteFont>("Fonts/Main");

            _modalShadowTexture = contentManager.Load<Texture2D>("Sprites/Ui/ModalDialogShadow");
            _modalTopTextures = new[] { contentManager.Load<Texture2D>("Sprites/Ui/ModalDialogBackgroundTop1") };
            _modalBottomTextures = new[] { contentManager.Load<Texture2D>("Sprites/Ui/ModalDialogBackgroundBottom1") };

            _biomeBackgroundDict = new Dictionary<BiomeType, Texture2D>
            {
                { BiomeType.Slavic, contentManager.Load<Texture2D>("Sprites/Ui/Biome") },
                { BiomeType.China, contentManager.Load<Texture2D>("Sprites/Ui/Biome") },
                { BiomeType.Egypt, contentManager.Load<Texture2D>("Sprites/Ui/Biome") },
                { BiomeType.Greek, contentManager.Load<Texture2D>("Sprites/Ui/Biome") }
            };

            _titleTrack = contentManager.Load<Song>("Audio/Background/Title");
            _mapTrack = contentManager.Load<Song>("Audio/Background/Map");
            _battleTrack = contentManager.Load<Song>("Audio/Background/Battle");
        }

        public Texture2D GetBiomeBackground(BiomeType type)
        {
            return _biomeBackgroundDict[type];
        }

        public Song GetTitleSong()
        {
            return _titleTrack;
        }

        public Song GetMapSong()
        {
            return _mapTrack;
        }

        public Song GetBattleSong()
        {
            return _battleTrack;
        }
    }
}