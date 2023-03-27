namespace Core.Crises;

public record CrisisSid(string Value)
{
    public string ResourceName => $"{Value}_Crisis";
};