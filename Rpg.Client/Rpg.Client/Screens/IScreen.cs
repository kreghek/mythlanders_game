using Microsoft.Xna.Framework;

using Rpg.Client.GameComponents;

namespace Rpg.Client.Screens
{
    internal interface IScreen : IEwarDrawableComponent
    {
        IScreen? TargetScreen { get; set; }
    }
}