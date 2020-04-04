using System.Collections.Generic;

namespace SyntaxHighlighter
{
	public class Language
	{
		public Dictionary<string, string[]> ReservedKeywords { get; }
		public char[] Separators { get; }

		public Language(Dictionary<string, string[]> reservedKeywords, char[] separators)
		{
			ReservedKeywords = reservedKeywords;
			Separators = separators;
		}
	}
}