using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Client.Assets;

internal static class CatalogHelper
{
    public static IReadOnlyCollection<TFactory> GetAllFactories<TFactory>(Assembly catalogAssembly)
    {
        var factoryTypes = catalogAssembly.GetTypes()
            .Where(x => typeof(TFactory).IsAssignableFrom(x) && x != typeof(TFactory) && !x.IsAbstract);
        var factories = factoryTypes.Select(Activator.CreateInstance);
        return factories.OfType<TFactory>().ToArray();
    }

    public static IReadOnlyCollection<TFactory> GetAllFactories<TFactory>()
    {
        return GetAllFactories<TFactory>(typeof(TFactory).Assembly);
    }

    public static IReadOnlyCollection<TObj> GetAllFromStaticCatalog<TObj>(Type catalog)
    {
        return catalog
            .GetProperties(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.PropertyType == typeof(TObj))
            .Select(f => f.GetValue(null))
            .Where(v => v is not null)
            .Select(v => (TObj)v!)
            .ToArray();
    }
}