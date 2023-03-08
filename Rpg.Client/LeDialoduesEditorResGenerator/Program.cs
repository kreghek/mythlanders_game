using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text.Json;
using System.Threading.Tasks;

using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using LeDialoduesEditorResGenerator.Serialization;
using System.Reflection;
using static System.Collections.Specialized.BitVector32;

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
                Console.WriteLine("Expecting exactly 2 args. YAML-file and output dir");
                Console.ResetColor();
                return;
            }

            var (inputDirInfo, outputDirInfo) = ParseArgs(args);
            if (!inputDirInfo.Exists)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Source YAML directory does not exist.");
                Console.ResetColor();
                return;
            }

            if (!outputDirInfo.Exists)
            {
                outputDirInfo.Create();
            }

            var totalResourceDataList = new List<(string resourceKey, string resourceData)>();

            foreach (var file in inputDirInfo.EnumerateFiles("*.yaml"))
            {
                var dialogueYaml = File.ReadAllText(file.FullName);

                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();

                var dialogueDtoDict = deserializer.Deserialize<Dictionary<string, DialogueDtoScene>>(dialogueYaml);

                var dialogueSid = Path.GetFileNameWithoutExtension(file.Name);
                var texts = GetTexts(dialogueSid, dialogueDtoDict).ToArray();

                totalResourceDataList.AddRange(texts);
            }

            totalResourceDataList.Add(("Common_end_dialogue", "Завершить"));

            foreach (var lang in new[] { "ru", "en" })
            {
                var fileName = GetFileName(lang);
                var outputPath = Path.Combine(outputDirInfo.FullName, fileName);
                using var rw = new ResourceWriter(outputPath);
                foreach (var (resourceKey, resourceData) in totalResourceDataList)
                {
                    rw.AddResource(resourceKey, resourceData);
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

        private static IEnumerable<(string, string)> GetTexts(string dialogueSid,
            Dictionary<string, DialogueDtoScene> scenes)
        {
            foreach (var (sceneSid, scene) in scenes)
            {
                for (var paragraphIndex = 0; paragraphIndex < scene.Paragraphs.Length; paragraphIndex++)
                {
                    var paragraph = scene.Paragraphs[paragraphIndex];
                    if (paragraph.Text is not null)
                    {
                        yield return ($"{dialogueSid}_Scene_{sceneSid}_Paragraph_{paragraphIndex}", paragraph.Text);

                    }
                    else if (paragraph.Reactions is not null)
                    {
                        foreach (var reaction in paragraph.Reactions)
                        {
                            yield return ($"{dialogueSid}_Scene_{sceneSid}_Paragraph_{paragraphIndex}_reaction_{reaction.Hero}", reaction.Text);
                        }
                    }
                }

                if (scene.Options is not null)
                {
                    for (var optionIndex = 0; optionIndex < scene.Options.Length; optionIndex++)
                    {
                        var option = scene.Options[optionIndex];
                        yield return ($"{dialogueSid}_Scene_{sceneSid}_Option_{optionIndex}", option.Text);
                    }
                }
            }
        }

        private static (DirectoryInfo, DirectoryInfo) ParseArgs(IReadOnlyList<string> args)
        {
            return (new DirectoryInfo(args[0]), new DirectoryInfo(args[1]));
        }
    }
}