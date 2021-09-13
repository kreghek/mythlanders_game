using Microsoft.Xna.Framework;

using Rpg.Client.Engine;

namespace Rpg.Client.Screens
{
    internal interface IScreen : IEwarDrawableComponent
    {
        IScreen? TargetScreen { get; set; }
    }
}