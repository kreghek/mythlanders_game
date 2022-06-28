namespace LeDialoduesEditorResGenerator
{
    public class ResourceData
    {
        public ResourceData(string id, string langKey, string value)
        {
            Id = id;
            LangKey = langKey;
            Value = value;
        }
        public string Id { get; }
        public string LangKey { get; }
        public string Value { get; }
    }
}