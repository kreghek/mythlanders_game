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
        private Song _defeatTrack;
        private SpriteFont _font;
        private Song[] _mapTracks;
        private Texture2D[] _modalBottomTextures;
        private Texture2D _modalShadowTexture;
        private Texture2D[] _modalTopTextures;
        private Song _titleTrack;
        private Texture2D _unitPanelTexture;
        private Song _victoryTrack;
        private Texture2D _speechTexture;

        public Texture2D GetButtonTexture()
        {
            return _buttonTexture;
        }
        
        public Texture2D GetSpeechTexture()
        {
            return _speechTexture;
        }

        public Texture2D GetEnvSpeechTexture()
        {
            return _modalShadowTexture ?? throw new InvalidOperationException();
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
            _speechTexture = contentManager.Load<Texture2D>("Sprites/Ui/Speech");
            _font = contentManager.Load<SpriteFont>("Fonts/Main");

            _modalShadowTexture = contentManager.Load<Texture2D>("Sprites/Ui/ModalDialogShadow");
            _modalTopTextures = new[] { contentManager.Load<Texture2D>("Sprites/Ui/ModalDialogBackgroundTop1") };
            _modalBottomTextures = new[] { contentManager.Load<Texture2D>("Sprites/Ui/ModalDialogBackgroundBottom1") };
            _combatPowerIconTextres = contentManager.Load<Texture2D>("Sprites/Ui/CombatPowerIcons");

            _biomeBackgroundDict = new Dictionary<BiomeType, Texture2D>
            {
                { BiomeType.Slavic, contentManager.Load<Texture2D>("Sprites/Ui/Biome") },
                { BiomeType.Chinese, contentManager.Load<Texture2D>("Sprites/Ui/Biome") },
                { BiomeType.Egyptian, contentManager.Load<Texture2D>("Sprites/Ui/Biome") },
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
                contentManager.Load<Song>("Audio/Background/Battle")
            };

            _victoryTrack = contentManager.Load<Song>("Audio/Background/Victory");
            _defeatTrack = contentManager.Load<Song>("Audio/Background/Defeat");

            _unitPanelTexture = contentManager.Load<Texture2D>("Sprites/Ui/UnitPanel");
        }

        public Texture2D GetBiomeBackground(BiomeType type)
        {
            return _biomeBackgroundDict[type];
        }

        public Song GetTitleSong()
        {
            return _titleTrack;
        }

        public Texture2D GetUnitPanelTexture()
        {
            return _unitPanelTexture;
        }

        public IEnumerable<Song> GetMapSong()
        {
            return _mapTracks;
        }

        public IEnumerable<Song> GetBattleSongs()
        {
            return _battleTracks;
        }

        public Song GetVictorySong()
        {
            return _victoryTrack;
        }

        public Song GetDefeatSong()
        {
            return _defeatTrack;
        }

        public Texture2D GetCombatPowerIconsTexture()
        {
            return _combatPowerIconTextres;
        }
    }
}