using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Engine
{
    internal abstract class EntityButtonBase<T> : ButtonBase
    {
        protected EntityButtonBase(Texture2D texture, T entity) : base(texture, Rectangle.Empty)
        {
            Entity = entity;
        }

        public T Entity { get; }
    }
}