using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace LEDialoguePlotConverter
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var files = Directory.GetFiles(Path.Combine(args[0], "*.json"));
            var infoFiles = files.Where(x => x.EndsWith("_info.json")).ToArray();
            var dialogueFiles = files.Where(x => !x.EndsWith("_info.json")).ToArray();

            var eventDtoList = new List<EventDto>();

            foreach (var infoFile in infoFiles)
            {
                var infoFileDeserialized = JsonSerializer.Deserialize<Info[]>(File.ReadAllText(infoFile));

                //var eventDto = new EventDto
                //{
                //    Sid = infoFileDeserialized.Sid,
                //    Name = infoFileDeserialized.Name,
                //    Location = infoFileDeserialized.Location,
                //    BeforeCombatAftermath = excelEvent.BeforeCombatAftermath,
                //    AfterCombatAftermath = excelEvent.AfterCombatAftermath,
                //    GoalDescription = excelEvent.GoalDescription,
                //    ParentSids = ParseParentSids(excelEvent)
                //};

                //eventDtoList.Add(eventDto);
            }

            var serialized = JsonSerializer.Serialize(new { }, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true,
                IgnoreNullValues = true,
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            });

            var outputPath = args[1];

            // Run with argument which contains full path to Rpg.Client/Resources directory

            var outputRuFileName = Path.Combine(outputPath, "Balance.json");
            File.WriteAllLines(outputRuFileName, new[] { serialized });
        }
    }
}