using Microsoft.Xna.Framework;

namespace Rpg.Client.GameScreens.Combat.GameObjects
{
    public interface IActorVisualizationState
    {
        bool CanBeReplaced { get; }
        bool IsComplete { get; }
        void Cancel();
        void Update(GameTime gameTime);
    }
}