using System.Reflection;

namespace Core;

public static class SidHelper
{
    public static IEnumerable<TValue> GetValues<TValue>(Type t)
    {
        var properties = t.GetProperties(BindingFlags.Static | BindingFlags.Public);

        foreach (var prop in properties)
        {
            var value = prop.GetValue(null);

            if (value is not null)
            {
                yield return (TValue)value;
            }
        }
    }
}