namespace Rpg.Client.Engine
{
    internal abstract class EntityButtonBase<T> : ButtonBase
    {
        protected EntityButtonBase(T entity)
        {
            Entity = entity;
        }

        public T Entity { get; }
    }
}