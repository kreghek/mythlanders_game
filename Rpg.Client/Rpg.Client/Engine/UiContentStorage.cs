using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

using Rpg.Client.Core;

namespace Rpg.Client.Engine
{
    internal sealed class UiContentStorage : IUiContentStorage
    {
        private Song[] _battleTracks;
        private Dictionary<BiomeType, Texture2D> _biomeBackgroundDict;
        private Texture2D? _buttonTexture;
        private Texture2D _combatPowerIconTextres;
        private SpriteFont _font;
        private Song[] _mapTracks;
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
            _combatPowerIconTextres = contentManager.Load<Texture2D>("Sprites/Ui/CombatPowerIcons");

            _biomeBackgroundDict = new Dictionary<BiomeType, Texture2D>
            {
                { BiomeType.Slavic, contentManager.Load<Texture2D>("Sprites/Ui/Biome") },
                { BiomeType.China, contentManager.Load<Texture2D>("Sprites/Ui/Biome") },
                { BiomeType.Egypt, contentManager.Load<Texture2D>("Sprites/Ui/Biome") },
                { BiomeType.Greek, contentManager.Load<Texture2D>("Sprites/Ui/Biome") }
            };

            _titleTrack = contentManager.Load<Song>("Audio/Background/Title");
            _mapTracks = new[]
            {
                contentManager.Load<Song>("Audio/Background/Map"),
                contentManager.Load<Song>("Audio/Background/Map2"),
                contentManager.Load<Song>("Audio/Background/Map3")
            };
            _battleTracks = new[]
            {
                contentManager.Load<Song>("Audio/Background/Battle"),
                contentManager.Load<Song>("Audio/Background/Battle2"),
                contentManager.Load<Song>("Audio/Background/Battle3"),
                contentManager.Load<Song>("Audio/Background/Battle4"),
                contentManager.Load<Song>("Audio/Background/Battle5"),
                contentManager.Load<Song>("Audio/Background/Battle6"),
                contentManager.Load<Song>("Audio/Background/Battle7")
            };
        }

        public Texture2D GetBiomeBackground(BiomeType type)
        {
            return _biomeBackgroundDict[type];
        }

        public Song GetTitleSong()
        {
            return _titleTrack;
        }

        public IEnumerable<Song> GetMapSong()
        {
            return _mapTracks;
        }

        public IEnumerable<Song> GetBattleSongs()
        {
            return _battleTracks;
        }

        public Texture2D GetCombatPowerIconsTexture()
        {
            return _combatPowerIconTextres;
        }
    }
}