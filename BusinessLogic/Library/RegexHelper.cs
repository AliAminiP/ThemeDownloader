using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BusinessLogic.Library
{
    public interface IRegexHelper
    {
        Match Extract(string content, string pattern);
        IEnumerable<string> ExtractValues(string content, string pattern);
    }
    public class RegexHelper : IRegexHelper
    {
        public Match Extract(string content, string pattern)
        {
            if (!IsValidParams(content, pattern))
            {
                return null;
            }
            var matches = Regex.Matches(content, pattern);
            return matches.FirstOrDefault();
        }

        public IEnumerable<string> ExtractValues(string content, string pattern)
        {
            if (!IsValidParams(content, pattern))
            {
                return null;
            }
            var matches = Regex.Matches(content, pattern);
            var tags = new List<string>();
            foreach (Match match in matches)
            {
                if (match.Success && match.Groups.Count > 1)
                {
                    tags.Add(match.Groups[1].Value);
                }
            }
            return tags;
        }
        private bool IsValidParams(string content, string pattern) => !string.IsNullOrEmpty(content) && !string.IsNullOrEmpty(pattern);
    }
}
