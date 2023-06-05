using Core.Props;

namespace Core.PropDrop
{
    /// <summary>
    /// Service for generating trophies according to drop tables.
    /// </summary>
    public interface IDropResolver
    {
        /// <summary>
        /// Receiving items according to the specified drop tables.
        /// </summary>
        /// <param name="dropTables"> Drop tables. </param>
        /// <returns> Returns the generated finished items. </returns>
        IProp[] Resolve(IEnumerable<IDropTableScheme> dropTables);
    }
}