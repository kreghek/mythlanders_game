namespace Core.Crises;

public record CrisisAftermathSid(string Value)
{
    public string ResourceName => $"{Value}_CrisisAftermath";
};