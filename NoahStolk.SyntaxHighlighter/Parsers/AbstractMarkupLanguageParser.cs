namespace NoahStolk.SyntaxHighlighter.Parsers;

public abstract class AbstractMarkupLanguageParser : AbstractParser
{
	public override Language CodeLanguage { get; } = new(
		reservedKeywords: new Dictionary<string, string[]>(),
		separators: new char[] { ' ', '\t', '\r', '\n', '<', '>' });

	protected override Piece HandleLanguageSpecificCodeTypes(string[] pieces, int index)
	{
		string type = index >= 1 && pieces[index - 1][0] == '<' ? "Element" : "Other";

		return new Piece(pieces[index], type);
	}
}
