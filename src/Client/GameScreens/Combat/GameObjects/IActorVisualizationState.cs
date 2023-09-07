using Microsoft.Xna.Framework;

namespace Client.GameScreens.Combat.GameObjects;

public interface IActorVisualizationState
{
    bool CanBeReplaced { get; }
    bool IsComplete { get; }
    void Cancel();
    void Update(GameTime gameTime);
}