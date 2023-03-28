using Core.Props;

namespace Core.PropDrop
{
    /// <summary>
    /// Сервис для работы со схемами.
    /// </summary>
    public interface ISchemeService
    {
        /// <summary>
        /// Извлечь схему по идентификатору.
        /// </summary>
        /// <typeparam name="TScheme"> Тип схемы. </typeparam>
        /// <param name="sid"> Идентификатор схемы. </param>
        /// <returns> Возвращает экземпляр схемы. </returns>
        /// <exception cref="KeyNotFoundException">
        /// Выбрасывает исключение,
        /// если схема указанного типа не найдена по указанному идентификатору.
        /// </exception>
        TScheme GetScheme<TScheme>(string sid) where TScheme : class, IScheme;

        /// <summary>
        /// Извлечь все схемы укаканного типа.
        /// </summary>
        /// <typeparam name="TScheme"> Тип схемы. </typeparam>
        /// <returns> Возвращает набор схем указанного типа. </returns>
        TScheme[] GetSchemes<TScheme>() where TScheme : class, IScheme;
    }

    public sealed class SchemeService : ISchemeService
    {
        private readonly IDictionary<string, IPropScheme> _schemes = new Dictionary<string, IPropScheme>();

        public SchemeService()
        {
            _schemes["combat-xp"] = new PropScheme() { Sid = "combat-xp" };
        }

        TScheme ISchemeService.GetScheme<TScheme>(string sid)
        {
            return (TScheme)_schemes[sid];
        }

        TScheme[] ISchemeService.GetSchemes<TScheme>()
        {
            return _schemes.Values.OfType<TScheme>().ToArray();
        }
    }
}