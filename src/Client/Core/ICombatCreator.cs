namespace Client.Core;

internal interface ICombatCreator
{
    string Sid { get; }

    CombatSequence Create();
}

internal interface ICombatAssetCatalog
{
    ICombatCreator GetAsset(int sid);
}