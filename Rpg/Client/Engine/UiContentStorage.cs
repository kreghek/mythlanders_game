using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

using Rpg.Client.Core;

namespace Rpg.Client.Engine
{
    internal sealed class UiContentStorage : IUiContentStorage
    {
        private IDictionary<BiomeType, Texture2D> _biomeBackgroundDict;
        private Texture2D _buttonIndicatorsTexture;
        private IDictionary<string, SpriteFont> _combatIndicatorFonts;
        private Texture2D _combatPowerIconTextres;
        private Texture2D _combatSkillPanelTextre;
        private CombatSoundtrack[] _combatTracks;
        private Texture2D? _controlBackgroundTexture;
        private Texture2D _cursonTextures;
        private Song _defeatTrack;
        private Texture2D _effectIconsTexture;
        private Texture2D _equipmentIconsTexture;
        private Song _introTrack;
        private Texture2D[] _introVideoTextures;
        private Texture2D _logoTexture;
        private IDictionary<string, SpriteFont> _mainFonts;
        private Song[] _mapTracks;
        private Texture2D[] _modalBottomTextures;
        private Texture2D _modalShadowTexture;
        private Texture2D _modalsTopSymbolTexture;
        private Texture2D[] _modalTopTextures;
        private Texture2D _socialTexture;
        private Texture2D _titleBackgroundTexture;
        private IDictionary<string, SpriteFont> _titlesFonts;
        private Song _titleTrack;
        private Texture2D _unitPanelTexture;
        private Song _victoryTrack;

        public Texture2D GetEnvSpeechTexture()
        {
            return _modalShadowTexture ?? throw new InvalidOperationException();
        }

        private static string GetLanguageKey()
        {
            var currentLanguage = Thread.CurrentThread.CurrentUICulture;
            if (string.Equals(
                    currentLanguage.TwoLetterISOLanguageName,
                    "ru",
                    StringComparison.InvariantCultureIgnoreCase))
            {
                return "ru";
            }

            if (string.Equals(
                    currentLanguage.TwoLetterISOLanguageName,
                    "en",
                    StringComparison.InvariantCultureIgnoreCase))
            {
                return "en";
            }

            if (string.Equals(
                    currentLanguage.TwoLetterISOLanguageName,
                    "zh",
                    StringComparison.InvariantCultureIgnoreCase))
            {
                return "zh";
            }

            throw new Exception();
        }

        private static SpriteFont GetSpriteFont(IDictionary<string, SpriteFont> fontDict, string languageKey)
        {
            return fontDict[languageKey];
        }

        public bool ContentWasLoaded { get; private set; }

        public Texture2D GetCursorsTexture()
        {
            return _cursonTextures;
        }

        public Texture2D GetControlBackgroundTexture()
        {
            return _controlBackgroundTexture;
        }

        public Texture2D GetEffectIconsTexture()
        {
            return _effectIconsTexture;
        }

        public SpriteFont GetMainFont()
        {
            var key = GetLanguageKey();
            var spriteFont = GetSpriteFont(_mainFonts, key);
            return spriteFont;
        }

        public SpriteFont GetTitlesFont()
        {
            var key = GetLanguageKey();
            var spriteFont = GetSpriteFont(_titlesFonts, key);
            return spriteFont;
        }

        public SpriteFont GetCombatIndicatorFont()
        {
            var key = GetLanguageKey();
            var spriteFont = GetSpriteFont(_combatIndicatorFonts, key);
            return spriteFont;
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

        public Texture2D GetLogoTexture()
        {
            return _logoTexture;
        }

        public Texture2D GetTitleBackgroundTexture()
        {
            return _titleBackgroundTexture;
        }

        public void LoadContent(ContentManager contentManager)
        {
            _logoTexture = contentManager.Load<Texture2D>("Sprites/Ui/GameLogo");
            _socialTexture = contentManager.Load<Texture2D>("Sprites/Ui/Social");
            _controlBackgroundTexture = contentManager.Load<Texture2D>("Sprites/Ui/ControlBackgrounds");

            _mainFonts = new Dictionary<string, SpriteFont>
            {
                { "en", contentManager.Load<SpriteFont>("Fonts/Main") },
                { "ru", contentManager.Load<SpriteFont>("Fonts/Main") },
                { "zh", contentManager.Load<SpriteFont>("Fonts/MainChinese") }
            };
            _titlesFonts = new Dictionary<string, SpriteFont>
            {
                { "en", contentManager.Load<SpriteFont>("Fonts/Titles") },
                { "ru", contentManager.Load<SpriteFont>("Fonts/Titles") },
                { "zh", contentManager.Load<SpriteFont>("Fonts/TitlesChinese") }
            };
            _combatIndicatorFonts = new Dictionary<string, SpriteFont>
            {
                { "en", contentManager.Load<SpriteFont>("Fonts/CombatIndicator") },
                { "ru", contentManager.Load<SpriteFont>("Fonts/CombatIndicator") },
                { "zh", contentManager.Load<SpriteFont>("Fonts/CombatIndicatorChinese") }
            };

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
            _combatTracks = new[]
            {
                new CombatSoundtrack(BiomeType.Slavic, contentManager.Load<Song>("Audio/Background/Combat_Slavic01")),
                new CombatSoundtrack(BiomeType.Slavic, contentManager.Load<Song>("Audio/Background/Combat_Slavic02")),
                new CombatSoundtrack(BiomeType.Chinese, contentManager.Load<Song>("Audio/Background/Combat_Chinese01")),
                new CombatSoundtrack(BiomeType.Chinese, contentManager.Load<Song>("Audio/Background/Combat_Chinese02")),
                new CombatSoundtrack(BiomeType.Egyptian,
                    contentManager.Load<Song>("Audio/Background/Combat_Egyptian01")),
                new CombatSoundtrack(BiomeType.Egyptian,
                    contentManager.Load<Song>("Audio/Background/Combat_Egyptian02")),
                new CombatSoundtrack(BiomeType.Egyptian, CombatSoundtrackRole.Intro,
                    contentManager.Load<Song>("Audio/Background/Combat_Egyptian01_Intro")),

                new CombatSoundtrack(BiomeType.Greek,
                    contentManager.Load<Song>("Audio/Background/Combat_Greek01")),
                new CombatSoundtrack(BiomeType.Greek, CombatSoundtrackRole.Intro,
                    contentManager.Load<Song>("Audio/Background/Combat_Greek01_Intro")),
                new CombatSoundtrack(BiomeType.Greek,
                    contentManager.Load<Song>("Audio/Background/Combat_Greek02")),
                new CombatSoundtrack(BiomeType.Greek, CombatSoundtrackRole.Intro,
                    contentManager.Load<Song>("Audio/Background/Combat_Greek02_Intro"))
            };

            _victoryTrack = contentManager.Load<Song>("Audio/Background/Victory");
            _defeatTrack = contentManager.Load<Song>("Audio/Background/Defeat");

            _introTrack = contentManager.Load<Song>("Audio/Intro/Intro");

            _unitPanelTexture = contentManager.Load<Texture2D>("Sprites/Ui/UnitPanel");

            _buttonIndicatorsTexture = contentManager.Load<Texture2D>("Sprites/Ui/ButtonIndicators");

            _effectIconsTexture = contentManager.Load<Texture2D>("Sprites/Ui/SkillEffectIcons");

            var introVideoTextures = new List<Texture2D>(150);
            for (var i = 1; i <= 150; i++)
            {
                var texture = contentManager.Load<Texture2D>($"Video/ezgif-frame-{i:000}");
                introVideoTextures.Add(texture);
            }

            _introVideoTextures = introVideoTextures.ToArray();

            _modalsTopSymbolTexture = contentManager.Load<Texture2D>("Sprites/Ui/ModalHeaders");

            _combatSkillPanelTextre = contentManager.Load<Texture2D>("Sprites/Ui/CombatSkillsPanel");
            _equipmentIconsTexture = contentManager.Load<Texture2D>("Sprites/Ui/EquipmentIcons");

            _cursonTextures = contentManager.Load<Texture2D>("Sprites/Ui/Cursors");

            _titleBackgroundTexture = contentManager.Load<Texture2D>("Sprites/Ui/TitleBackground");

            ContentWasLoaded = true;
        }

        public Texture2D GetSocialTexture()
        {
            return _socialTexture;
        }

        public Texture2D GetCombatSkillPanelTexture()
        {
            return _combatSkillPanelTextre;
        }

        public Texture2D GetEquipmentTextures()
        {
            return _equipmentIconsTexture;
        }

        public Texture2D GetButtonIndicatorsTexture()
        {
            return _buttonIndicatorsTexture;
        }

        public Texture2D GetModalTopSymbolTextures()
        {
            return _modalsTopSymbolTexture;
        }

        public Texture2D GetDisabledTexture()
        {
            return _modalShadowTexture;
        }

        public Texture2D GetBiomeBackground(BiomeType type)
        {
            return _biomeBackgroundDict[type];
        }

        public Song GetTitleSong()
        {
            return _titleTrack;
        }

        public Texture2D GetUnitStatePanelTexture()
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

        public IReadOnlyCollection<CombatSoundtrack> GetCombatSongs(BiomeType currentBiome)
        {
            return _combatTracks.Where(x => x.ApplicableBiome == currentBiome && x.Role == CombatSoundtrackRole.Regular)
                .ToList();
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