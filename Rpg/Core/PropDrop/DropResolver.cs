using Core.Props;
using System.Diagnostics;

namespace Core.PropDrop
{
    public class DropResolver : IDropResolver
    {
        private readonly IPropFactory _propFactory;
        private readonly IDropResolverRandomSource _randomSource;
        private readonly ISchemeService _schemeService;

        public DropResolver(
            IDropResolverRandomSource randomSource,
            ISchemeService schemeService,
            IPropFactory propFactory)
        {
            _randomSource = randomSource;
            _schemeService = schemeService;
            _propFactory = propFactory;
        }

        private IProp GenerateProp(IDropTableRecordSubScheme record)
        {
            try
            {
                if (record.SchemeSid is null)
                {
                    throw new InvalidOperationException();
                }

                var scheme = _schemeService.GetScheme<IPropScheme>(record.SchemeSid);

                var rolledCount = _randomSource.RollResourceCount(record.MinCount, record.MaxCount);
                var resource = _propFactory.CreateResource(scheme, rolledCount);
                return resource;

            }
            catch (Exception exception)
            {
                //TODO Оборачивать в доменное исключение. Создать собственный тип.
                throw new Exception($"Ошибка при обработке записи дропа {record.SchemeSid}", exception);
            }
        }

        private IProp[] ResolveInner(IDropTableScheme[] dropTables)
        {
            var rolledRecords = new List<IDropTableRecordSubScheme>();

            var openDropTables = new List<IDropTableScheme>(dropTables);
            while (openDropTables.Any())
            {
                try
                {
                    var table = openDropTables[0];

                    var records = table.Records;
                    if (records is null || !records.Any())
                    {
                        Debug.Fail("The drop tables must have not null or empty records.");
                        // Do not try to roll if drop table has no records.

                        // Don't forget to remove empty drop table from open to avoid endless loop.
                        openDropTables.RemoveAt(0);
                        continue;
                    }

                    var recMods = records;

                    var totalWeight = recMods.Sum(x => x.Weight);

                    Debug.Assert(table.Rolls > 0,
                        "There is no reason to have zero rolls in table. This is most likely a mistake.");
                    var rolls = table.Rolls;
                    if (rolls == 0)
                    {
                        rolls = 1;
                    }

                    for (var rollIndex = 0; rollIndex < rolls; rollIndex++)
                    {
                        var rolledWeight = _randomSource.RollWeight(totalWeight);
                        var recMod = DropRoller.GetRecord(recMods, rolledWeight);

                        if (recMod?.SchemeSid == null)
                        {
                            continue;
                        }

                        rolledRecords.Add(recMod);

                        if (recMod.Extra != null)
                        {
                            //TODO Доделать учёт Rolls для экстра.
                            // Сейчас все экстра гарантированно выпадают по разу.
                            openDropTables.AddRange(recMod.Extra);
                        }
                    }

                    openDropTables.RemoveAt(0);
                }
                catch
                {
                    openDropTables.RemoveAt(0);
                    //TODO FIX
                }
            }

            var props = rolledRecords.Select(GenerateProp).ToArray();

            return props;
        }

        public IProp[] Resolve(IEnumerable<IDropTableScheme> dropTables)
        {
            var materializedDropTables = dropTables.ToArray();
            var props = ResolveInner(materializedDropTables);
            return props;
        }
    }
}
