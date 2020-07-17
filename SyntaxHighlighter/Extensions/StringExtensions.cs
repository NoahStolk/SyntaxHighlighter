using System.Linq;
using System.Text.RegularExpressions;

namespace SyntaxHighlighter.Extensions
{
	public static class StringExtensions
	{
		public static string[] SplitIncludeDelimiters(this string s, params char[] delimiters)
			=> SplitIncludeDelimiters(s, delimiters.Select(d => d.ToString()).ToArray());

		public static string[] SplitIncludeDelimiters(this string s, params string[] delimiters)
		{
			string pattern = $"({string.Join("|", delimiters.Select(d => Regex.Escape(d)).ToArray())})";
			return Regex.Split(s, pattern).Where(split => !string.IsNullOrEmpty(split)).ToArray();
		}
	}
}