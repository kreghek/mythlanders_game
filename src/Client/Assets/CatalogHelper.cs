using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Client.Assets;

internal static class CatalogHelper
{
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