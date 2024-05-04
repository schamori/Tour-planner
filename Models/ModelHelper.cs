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
            for (int i = 0; i < text.Length; i += maxLineLength)
            {
                if (i + maxLineLength >= text.Length)
                    wrapped.AppendLine(text.Substring(i));
                else
                    wrapped.AppendLine(text.Substring(i, maxLineLength));
            }
            return wrapped.ToString();
        }
    }
}
