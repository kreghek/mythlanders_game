using System.Collections.Generic;
using System.Linq;

using Client.Core;
using Client.Core.CampaignEffects;
using Client.GameScreens.CampaignReward.Ui;
using Client.GameScreens.Combat;

using Core.Props;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Common.Result;

internal sealed class PropCampaignRewardImageDrawer : CampaignRewardImageDrawerBase<ResourceCampaignEffect>
{
    private readonly Texture2D _propTexture;
    private readonly SpriteFont _font;
    private readonly Inventory _currentInventory;

    private readonly IDictionary<IProp, AnimatedCountableResource> _countableItems;

    public PropCampaignRewardImageDrawer(Texture2D propTexture, SpriteFont font, Inventory currentInventory)
    {
        _propTexture = propTexture;
        _font = font;
        _currentInventory = currentInventory;

        _countableItems = new Dictionary<IProp, AnimatedCountableResource>();
    }

    public override Point ImageSize => new Point(32);

    protected override void Draw(ResourceCampaignEffect reward, SpriteBatch spriteBatch, Vector2 position)
    {
        for (var index = 0; index < reward.Resources.OfType<Resource>().ToArray().Length; index++)
        {
            var resource = reward.Resources.OfType<Resource>().ToArray()[index];

            spriteBatch.Draw(_propTexture, position, new Rectangle(0, index * 32, 32, 32), Color.White);

            if (!_countableItems.TryGetValue(resource, out var countable))
            {
                countable = new AnimatedCountableResource(new ResourceReward()
                {
                    StartValue = _currentInventory.CalcActualItems().OfType<Resource>()
                        .SingleOrDefault(x => x.Scheme.Sid == resource.Scheme.Sid)?.Count ?? 0,
                    Amount = resource.Count,
                });

                _countableItems[resource] = countable;
            }

            var labelText = $"{resource.Scheme.Sid} x {countable.CurrentValue} (+{countable.Amount})";
            spriteBatch.DrawString(_font, labelText, position + new Vector2(index * 32, 32), Color.Wheat);
        }
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var animatedCountableResource in _countableItems)
        {
            animatedCountableResource.Value.Update();
        }
    }
}
