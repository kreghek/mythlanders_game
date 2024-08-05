using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.PreHistory;

internal sealed class StartHeroesScene : IPreHistoryScene
{
    private readonly Texture2D _hopliteTexture;
    private readonly Texture2D _liberatorTexture;
    private readonly Texture2D _monkTexture;
    private readonly Texture2D[] _portraitTextures;
    private readonly Texture2D _swordsmanTexture;

    private int? _hoverOption;
    private int? _selectedOption;

    public StartHeroesScene(Texture2D swordsmanTexture, Texture2D monkTexture, Texture2D hopliteTexture,
        Texture2D liberatorTexture)
    {
        _swordsmanTexture = swordsmanTexture;
        _monkTexture = monkTexture;
        _hopliteTexture = hopliteTexture;
        _liberatorTexture = liberatorTexture;

        _portraitTextures = new[]
        {
            _monkTexture,
            _swordsmanTexture,
            _hopliteTexture,
            _liberatorTexture
        };
    }

    private static void DrawPortrait(Texture2D portraitTexture, bool isActive, SpriteBatch spriteBatch,
        Vector2 position, double transition)
    {
        spriteBatch.Draw(portraitTexture,
            position,
            isActive
                ? Color.Lerp(Color.Transparent, Color.White, (float)transition)
                : Color.Lerp(Color.Transparent, new Color(75, 75, 75, 255), (float)transition));
    }

    public void Draw(SpriteBatch spriteBatch, Rectangle contentRect, double transition)
    {
        for (var i = _portraitTextures.Length - 1; i >= 0; i--)
        {
            var isActive = _hoverOption == i || _selectedOption == i;
            var portraitOffset = new Vector2((256 - 128) * i, 0);
            var portraitPosition = contentRect.Location.ToVector2() + portraitOffset;

            DrawPortrait(_portraitTextures[i], isActive, spriteBatch, portraitPosition, transition);
        }
    }

    public void HoverOption(int? index)
    {
        _hoverOption = index;
    }

    public void SelectOption(int index)
    {
        _selectedOption = index;
    }

    public void Update(GameTime gameTime, bool isInteractive)
    {
    }
}