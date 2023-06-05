using Microsoft.Xna.Framework;

using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Combat.GameObjects
{
    internal sealed class UnitPositionProvider : IUnitPositionProvider
    {
        private readonly ResolutionIndependentRenderer _resolutionIndependentRenderer;
        private readonly Vector2[] _unitPredefinedPositions;

        public UnitPositionProvider(ResolutionIndependentRenderer resolutionIndependentRenderer)
        {
            _unitPredefinedPositions = new[]
            {
                new Vector2(335, 300),
                new Vector2(305, 250),
                new Vector2(305, 350),
                new Vector2(215, 250),
                new Vector2(215, 350),
                new Vector2(245, 300)
            };
            _resolutionIndependentRenderer = resolutionIndependentRenderer;
        }

        public Vector2 GetPosition(int slotIndex, bool isPlayerSide)
        {
            var predefinedPosition = _unitPredefinedPositions[slotIndex];

            Vector2 calculatedPosition;

            if (isPlayerSide)
            {
                calculatedPosition = predefinedPosition;
            }
            else
            {
                var width = _resolutionIndependentRenderer.VirtualWidth;
                // Move from right edge.
                var xMirror = width - predefinedPosition.X;
                calculatedPosition = new Vector2(xMirror, predefinedPosition.Y);
            }

            return calculatedPosition;
        }
    }
}