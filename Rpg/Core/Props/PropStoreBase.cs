namespace Core.Props
{
    /// <summary>
    /// Базовый класс для всех хранилищ предметов.
    /// </summary>
    public abstract class PropStoreBase : IPropStore
    {
        protected PropStoreBase()
        {
            Items = new HashSet<IProp>();
        }

        protected HashSet<IProp> Items { get; }

        private void DoAddProp(IProp prop)
        {
            DoEventInner(Added, prop);
        }

        private void DoChangedProp(IProp prop)
        {
            DoEventInner(Changed, prop);
        }

        private void DoEventInner(EventHandler<PropStoreEventArgs>? @event,
            IProp? prop)
        {
            if (prop == null)
            {
                throw new ArgumentNullException(nameof(prop));
            }

            @event?.Invoke(this, new PropStoreEventArgs(prop));
        }

        private void DoRemovedProp(IProp prop)
        {
            DoEventInner(Removed, prop);
        }

        private bool HasResource(Resource resource)
        {
            return Items.OfType<Resource>().Any(x => x.Scheme.Sid == resource.Scheme.Sid && x.Count >= resource.Count);
        }

        private void RemoveResource(Resource resource)
        {
            var currentResource = Items.OfType<Resource>()
                .SingleOrDefault(x => x.Scheme == resource.Scheme);

            if (currentResource == null)
            {
                throw new InvalidOperationException($"В инвентаре не найден ресурс со схемой {resource.Scheme}.");
            }

            if (currentResource.Count < resource.Count)
            {
                throw new InvalidOperationException(
                    $"Попытка удалить {resource.Count} ресурсов {resource.Scheme} больше чем есть в инвентаре.");
            }

            if (currentResource.Count == resource.Count)
            {
                Items.Remove(currentResource);
                DoRemovedProp(currentResource);
            }
            else
            {
                currentResource.Count -= resource.Count;
                DoChangedProp(currentResource);
            }
        }

        private void StackResource(Resource resource)
        {
            var currentResource = Items.OfType<Resource>()
                .SingleOrDefault(x => x.Scheme == resource.Scheme);

            if (currentResource == null)
            {
                Items.Add(resource);
                DoAddProp(resource);
            }
            else
            {
                currentResource.Count += resource.Count;
                DoChangedProp(currentResource);
            }
        }

        public event EventHandler<PropStoreEventArgs>? Added;
        public event EventHandler<PropStoreEventArgs>? Removed;
        public event EventHandler<PropStoreEventArgs>? Changed;

        public virtual IProp[] CalcActualItems()
        {
            return Items.ToArray();
        }

        public void Add(IProp prop)
        {
            switch (prop)
            {
                case Resource resource:
                    StackResource(resource);
                    break;
            }
        }

        public void Remove(IProp prop)
        {
            switch (prop)
            {
                case Resource resource:
                    RemoveResource(resource);
                    break;
            }
        }

        public bool Has(IProp prop)
        {
            return prop switch
            {
                Resource resource => HasResource(resource),
                _ => throw new InvalidOperationException()
            };
        }
    }
}