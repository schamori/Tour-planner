using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    internal static class ModelHelper
    {
        public static string WrapText(string text, int maxLineLength = 20)
        {
            if (string.IsNullOrEmpty(text)) return text;

            var wrapped = new System.Text.StringBuilder();
            var lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            foreach (var line in lines)
            {
                if (line.Length <= maxLineLength)
                {
                    wrapped.Append(line);
                }
                else
                {
                    for (int i = 0; i < line.Length; i += maxLineLength)
                    {
                        if (i + maxLineLength >= line.Length)
                            wrapped.Append(line.Substring(i));
                        else
                            wrapped.AppendLine(line.Substring(i, maxLineLength));
                    }
                }
            }

            return wrapped.ToString();
        }
    }
}
