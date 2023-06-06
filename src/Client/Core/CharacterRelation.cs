namespace Client.Core;

public sealed class CharacterRelation
{
    public CharacterRelation(UnitName name)
    {
        Name = name;
    }

    public CharacterKnowledgeLevel Level { get; set; }
    public UnitName Name { get; }
}