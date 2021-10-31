using Rpg.Client.Core;

namespace Rpg.Client.Models
{
    internal class GameObjectHelper
    {
        public static string GetLocalizedUnitName(UnitName unitName)
        {
            var rm = GameObjectResources.ResourceManager;
            var name = rm.GetString(unitName.ToString()) ?? unitName.ToString();
            return name;
        }
    }
}
