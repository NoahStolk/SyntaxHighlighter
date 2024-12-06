namespace NoahStolk.SyntaxHighlighter;

public sealed class Language(Dictionary<string, string[]> reservedKeywords, char[] separators)
{
	public Dictionary<string, string[]> ReservedKeywords { get; } = reservedKeywords;
	public char[] Separators { get; } = separators;
}
