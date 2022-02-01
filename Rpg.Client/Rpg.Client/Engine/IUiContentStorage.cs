using System.Collections.Generic;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

using Rpg.Client.Core;

namespace Rpg.Client.Engine
{
    internal interface IUiContentStorage
    {
        IReadOnlyCollection<Song> GetBattleSongs(BiomeType currentBiome);
        Texture2D GetBiomeBackground(BiomeType type);
        Texture2D GetButtonIndicatorsTexture();
        Texture2D GetButtonTexture();
        SpriteFont GetCombatIndicatorFont();
        Texture2D GetCombatPowerIconsTexture();
        Texture2D GetCombatSkillPanelTexture();
        Song GetDefeatSong();
        Texture2D GetDisabledTexture();
        Texture2D GetEnvSpeechTexture();
        Texture2D GetEquipmentTextures();
        Song GetIntroSong();
        Texture2D[] GetIntroVideo();
        SpriteFont GetMainFont();
        IEnumerable<Song> GetMapSong();
        Texture2D[] GetModalBottomTextures();
        Texture2D GetModalShadowTexture();
        Texture2D GetModalTopSymbolTextures();
        Texture2D[] GetModalTopTextures();
        Texture2D GetPanelTexture();
        Texture2D GetSkillButtonTexture();
        Texture2D GetSpeechTexture();
        SpriteFont GetTitlesFont();
        Song GetTitleSong();
        Texture2D GetUnitStatePanelTexture();
        Song GetVictorySong();
        void LoadContent(ContentManager contentManager);
    }
}