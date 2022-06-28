using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpg.Client.Core
{
    internal interface ICombatCreator
    {
        string Sid { get; }

        CombatSequence Create();
    }

    internal interface ICombatAssetCatalog
    {
        ICombatCreator GetAsset(int sid);
    }
}
