using Rpg.Client.Assets.Catalogs;

namespace Content.SideQuests.Tests;

public class LocalDialogueResourceProvider: IDialogueResourceProvider
{
    public string GetResource(string resourceSid)
    {
        return File.ReadAllText(Path.Combine("../Client/Content/Dialogues", resourceSid + ".yaml"));
    }
}