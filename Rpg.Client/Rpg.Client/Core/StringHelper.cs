using System.Text;

namespace Rpg.Client.Core
{
    public static class StringHelper
    {
        public static string TempLineBreaking(string? text)
        {
            if (text is null)
            {
                return null;
            }

            var items = text.Split('\n');
            var sb = new StringBuilder();
            foreach (var item in items)
            {
                if (item.Length > 80)
                {
                    var textRemains = item;
                    do
                    {
                        sb.AppendLine(textRemains.Substring(0, 80));
                        textRemains = textRemains.Remove(0, 80);
                    } while (textRemains.Length > 80);

                    sb.AppendLine(textRemains);
                }
                else
                {
                    sb.AppendLine(item);
                }
            }

            return sb.ToString();
        }
    }
}