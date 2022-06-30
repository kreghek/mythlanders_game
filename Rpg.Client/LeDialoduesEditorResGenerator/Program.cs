using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text.Json;
using System.Threading.Tasks;

namespace LeDialoduesEditorResGenerator
{
    public static class Program
    {
        private const string DEFAULT_LOCALE = "en";

        public static async Task Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Expecting exactly 2 args. JSON-file and output dir");
                Console.ResetColor();
                return;
            }

            var (inputDirInfo, outputDirInfo) = ParseArgs(args);
            if (!inputDirInfo.Exists)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Source JSON directory does not exist.");
                Console.ResetColor();
                return;
            }

            if (!outputDirInfo.Exists)
            {
                outputDirInfo.Create();
            }

            var totalResourceDataList = new List<(string rsourceKey, Dictionary<string, string> resourceData)>();

            foreach (var file in inputDirInfo.EnumerateFiles("*.json"))
            {
                var deserializedDialogues =
                    await JsonSerializer.DeserializeAsync<Dictionary<string, JsonElement>>(file.OpenRead());
                var texts = GetTexts(file, deserializedDialogues).ToArray();

                totalResourceDataList.AddRange(texts);
            }

            var groupedTranslations = totalResourceDataList.SelectMany(t =>
                {
                    return t.Item2.Select(x => new ResourceData(t.Item1, x.Key, x.Value));
                })
                .GroupBy(x => x.LangKey)
                .OrderByDescending(x => x.Count());

            foreach (var group in groupedTranslations)
            {
                var fileName = GetFileName(group.Key);
                var outputPath = Path.Combine(outputDirInfo.FullName, fileName);
                using var rw = new ResourceWriter(outputPath);
                foreach (var text in group)
                {
                    rw.AddResource(text.Id, text.Value);
                }
            }
        }

        private static string GetFileName(string lang)
        {
            const string FILE_NAME = "DialogueResources";
            if (lang.ToLower() == DEFAULT_LOCALE)
            {
                return $"{FILE_NAME}.resources";
            }
            return $"{FILE_NAME}.{lang}.resources";
        }

        private static IEnumerable<(string, Dictionary<string, string>)> GetTexts(FileInfo fileInfo,
            Dictionary<string, JsonElement> json)
        {
            var dialogueSid = Path.GetFileNameWithoutExtension(fileInfo.Name);
            foreach (var (key, obj) in json)
            {
                if (obj.TryGetProperty("text", out var texts))
                {
                    var localizations = JsonSerializer.Deserialize<Dictionary<string, string>>(texts.GetRawText());
                    var textNodeKey = $"{dialogueSid}_TextNode_{key}";
                    yield return (textNodeKey, localizations);
                }
                if (obj.TryGetProperty("choices", out var choices))
                {
                    var optionIndex = 0;
                    foreach (var choice in choices.EnumerateArray())
                    {
                        if (choice.TryGetProperty("text", out var choiceTexts))
                        {
                            var choicesLocalizations =
                                JsonSerializer.Deserialize<Dictionary<string, string>>(choiceTexts.GetRawText());
                            var optionKey = $"{dialogueSid}_TextNode_{key}_Option_{optionIndex}";
                            yield return (optionKey, choicesLocalizations);
                        }
                        optionIndex += 1;
                    }
                }
            }
        }
        private static (DirectoryInfo, DirectoryInfo) ParseArgs(IReadOnlyList<string> args) =>
            (new DirectoryInfo(args[0]), new DirectoryInfo(args[1]));
    }
}