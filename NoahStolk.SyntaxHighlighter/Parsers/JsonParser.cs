namespace NoahStolk.SyntaxHighlighter.Parsers;

public sealed class JsonParser : AbstractParser
{
	private static readonly Lazy<JsonParser> _lazy = new(() => new());

	private JsonParser()
	{
	}

	public static JsonParser Instance => _lazy.Value;

	public override string Name => "JSON";

	public override Language CodeLanguage { get; } = new(
		reservedKeywords: new()
		{
			["Keyword"] = ["false", "null", "true"],
		},
		separators: [' ', '\t', '\r', '\n', ',', '[', ']', '(', ')', '<', '>', ';', '.']);

	public override Style CodeStyle { get; } = new(
		highlightColors: new()
		{
			{ "Number", new(127, 255, 127) },
			{ "Other", new(255, 255, 255) },
			{ "String", new(255, 127, 0) },
			{ "Char", new(255, 191, 0) },
			{ "Element", new(127, 127, 255) },
			{ "Keyword", new(0, 0, 255) },
		},
		backgroundColor: new(11, 8, 5),
		borderColor: new(127, 95, 63));

	protected override Piece HandleLanguageSpecificCodeTypes(string[] pieces, int index)
	{
		return new(pieces[index], "Other");
	}
}
