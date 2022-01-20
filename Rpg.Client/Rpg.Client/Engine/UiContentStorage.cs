using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

using Rpg.Client.Core;

namespace Rpg.Client.Engine
{
    internal sealed class UiContentStorage : IUiContentStorage
    {
        private (BiomeType, Song)[] _battleTracks;
        private Dictionary<BiomeType, Texture2D> _biomeBackgroundDict;
        private Texture2D _buttonIndicatorsTexture;
        private Texture2D? _buttonTexture;
        private SpriteFont _combatIndicatorFont;
        private Texture2D _combatPowerIconTextres;
        private Song _defeatTrack;
        private Song _introTrack;
        private Texture2D[] _introVideoTextures;
        private SpriteFont _mainFont;
        private Song[] _mapTracks;
        private Texture2D[] _modalBottomTextures;
        private Texture2D _modalShadowTexture;
        private Texture2D[] _modalTopTextures;
        private Texture2D? _panelTexture;
        private Texture2D _speechTexture;
        private SpriteFont _titlesFont;
        private Song _titleTrack;
        private Texture2D _unitPanelTexture;
        private Song _victoryTrack;
        private Texture2D _modalsTopSymbolTexture;

        public Texture2D GetButtonTexture()
        {
            return _buttonTexture;
        }

        public Texture2D GetPanelTexture()
        {
            return _panelTexture;
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
            return _mainFont;
        }

        public SpriteFont GetTitlesFont()
        {
            return _titlesFont;
        }

        public SpriteFont GetCombatIndicatorFont()
        {
            return _combatIndicatorFont;
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
            _panelTexture = contentManager.Load<Texture2D>("Sprites/Ui/Panel");
            _speechTexture = contentManager.Load<Texture2D>("Sprites/Ui/Speech");
            _mainFont = contentManager.Load<SpriteFont>("Fonts/Main");
            _titlesFont = contentManager.Load<SpriteFont>("Fonts/Titles");
            _combatIndicatorFont = contentManager.Load<SpriteFont>("Fonts/CombatIndicator");

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
                (BiomeType.Slavic, contentManager.Load<Song>("Audio/Background/Battle")),
                (BiomeType.Slavic, contentManager.Load<Song>("Audio/Background/Battle2")),
                (BiomeType.Chinese, contentManager.Load<Song>("Audio/Background/BattleChinese"))
            };

            _victoryTrack = contentManager.Load<Song>("Audio/Background/Victory");
            _defeatTrack = contentManager.Load<Song>("Audio/Background/Defeat");

            _introTrack = contentManager.Load<Song>("Audio/Intro/Intro");

            _unitPanelTexture = contentManager.Load<Texture2D>("Sprites/Ui/UnitPanel");

            _buttonIndicatorsTexture = contentManager.Load<Texture2D>("Sprites/Ui/ButtonIndicators");

            var introVideoTextures = new List<Texture2D>(150);
            for (var i = 1; i <= 150; i++)
            {
                var texture = contentManager.Load<Texture2D>($"Video/ezgif-frame-{i:000}");
                introVideoTextures.Add(texture);
            }

            _introVideoTextures = introVideoTextures.ToArray();

            _modalsTopSymbolTexture = contentManager.Load<Texture2D>("Sprites/Ui/ModalHeaders");
        }

        public Texture2D GetButtonIndicatorsTexture()
        {
            return _buttonIndicatorsTexture;
        }

        public Texture2D GetModalTopSymbolTextures()
        {
            return _modalsTopSymbolTexture;
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

        public Song GetIntroSong()
        {
            return _introTrack;
        }

        public IReadOnlyCollection<Song> GetBattleSongs(BiomeType currentBiome)
        {
            return _battleTracks.Where(x => x.Item1 == currentBiome).Select(x => x.Item2).ToList();
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

        public Texture2D[] GetIntroVideo()
        {
            return _introVideoTextures;
        }
    }
}