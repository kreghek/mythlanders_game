using Core.Props;

namespace Core.PropDrop;

public sealed class SchemeService : ISchemeService
{
    private readonly IDictionary<string, IPropScheme> _schemes = new Dictionary<string, IPropScheme>();

    public SchemeService()
    {
        void AddScheme(string sid)
        {
            _schemes[sid] = new PropScheme()
            {
                Sid = sid
            };
        }

        AddScheme("combat-xp");
        AddScheme("digital-claws");
        AddScheme("bandages");
        AddScheme("snow");
    }

    TScheme ISchemeService.GetScheme<TScheme>(string sid)
    {
        return (TScheme)_schemes[sid];
    }

    TScheme[] ISchemeService.GetSchemes<TScheme>()
    {
        return _schemes.Values.OfType<TScheme>().ToArray();
    }
}