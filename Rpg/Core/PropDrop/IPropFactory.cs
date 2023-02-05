using Core.Props;

namespace Core.PropDrop
{
    /// <summary>
    /// Фабрика предметов.
    /// </summary>
    public interface IPropFactory
    {
        /// <summary>
        /// Создаёт ресурс на основе схемы.
        /// </summary>
        /// <param name="scheme"> Схема ресурса. </param>
        /// <param name="count"> Количество ресурса. </param>
        /// <returns> Возвращает экземпляр созданного ресурса. </returns>
        Resource CreateResource(IPropScheme scheme, int count);
    }
}