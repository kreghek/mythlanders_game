namespace Core.Crises;

public interface ICrisisAftermath
{
    CrisisAftermathSid Sid { get; }
    void Apply(ICrisisAftermathContext context);
}