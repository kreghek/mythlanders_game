using System;
using System.Text;

namespace Rpg.Client.Core
{
    public static class StringHelper
    {
        public static string LineBreaking(string text, int maxInLine)
        {
            var items = text.Split('\n');
            var sb = new StringBuilder();
            var singleSb = new StringBuilder();
            foreach (var item in items)
            {
                var lineItems = item.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                singleSb.Clear();
                var firstInLine = true;
                foreach (var lineItem in lineItems)
                {
                    if (firstInLine)
                    {
                        
                    }
                    else
                    {
                        singleSb.Append(" ");
                    }
                    
                    firstInLine = false;

                    singleSb.Append(lineItem);

                    if (singleSb.Length <= maxInLine)
                    {
                        sb.Append(lineItem);
                    }
                    else
                    {
                        singleSb.Clear();
                        sb.AppendLine();
                        
                        singleSb.Append(lineItem);
                        sb.Append(lineItem);
                    }
                }
            }

            return sb.ToString().Trim();
        }
    }
}