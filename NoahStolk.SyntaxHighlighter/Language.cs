using System.Collections.Generic;

namespace NoahStolk.SyntaxHighlighter;

public class Language
{
	public Language(Dictionary<string, string[]> reservedKeywords, char[] separators)
	{
		ReservedKeywords = reservedKeywords;
		Separators = separators;
	}

	public Dictionary<string, string[]> ReservedKeywords { get; }
	public char[] Separators { get; }
}
