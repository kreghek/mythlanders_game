namespace Core.Props;

/// <summary>
/// Item storage interface.
/// </summary>
public interface IPropStore
{
    /// <summary>
    /// Adding an item to storage.
    /// </summary>
    /// <param name="prop"> Target item. </param>
    void Add(IProp prop);

    /// <summary>
    /// Items in inventory.
    /// </summary>
    IProp[] CalcActualItems();

    /// <summary>
    /// Checks if the item is in the store.
    /// </summary>
    /// <param name="prop">Specified item.</param>
    /// <returns>true - if the item exists.</returns>
    bool Has(IProp prop);

    /// <summary>
    /// Deleting an item from storage.
    /// </summary>
    /// <param name="prop"> Target item. </param>
    void Remove(IProp prop);

    /// <summary>
    /// The event is fired when a new item is added to the store.
    /// </summary>
    /// <remarks>
    /// This event does not fire if the number of resources has changed.
    /// </remarks>
    event EventHandler<PropStoreEventArgs>? Added;

    /// <summary>
    /// The event is fired if any item is removed from storage.
    /// </summary>
    event EventHandler<PropStoreEventArgs>? Removed;

    /// <summary>
    /// The event is fired when one of the items in the store changes.
    /// </summary>
    /// <remarks>
    /// Used when the amount of resources in the stack changes.
    /// </remarks>
    event EventHandler<PropStoreEventArgs>? Changed;
}