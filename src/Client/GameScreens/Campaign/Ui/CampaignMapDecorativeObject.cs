using Client.Core;

using GameClient.Engine.Animations;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Campaign.Ui;

internal sealed record CampaignMapDecorativeObject(
    Texture2D SourceTexture,
    IAnimationFrameSet AnimationFrameSet,
    Vector2 RelativePosition,
    Point Size,
    Color? Color = null,
    bool? IsFlipped = null);