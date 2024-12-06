namespace NoahStolk.SyntaxHighlighter.Parsers;

public sealed class JsonParser : AbstractParser
{
	private static readonly Lazy<JsonParser> _lazy = new(() => new JsonParser());

	private JsonParser()
	{
	}

	public static JsonParser Instance => _lazy.Value;

	public override string Name => "JSON";

	public override Language CodeLanguage { get; } = new(
		reservedKeywords: new Dictionary<string, string[]>
		{
			["Keyword"] = ["false", "null", "true"],
		},
		separators: [' ', '\t', '\r', '\n', ',', '[', ']', '(', ')', '<', '>', ';', '.']);

	public override Style CodeStyle { get; } = new(
		highlightColors: new Dictionary<string, Color>
		{
			{ "Number", new Color(127, 255, 127) },
			{ "Other", new Color(255, 255, 255) },
			{ "String", new Color(255, 127, 0) },
			{ "Char", new Color(255, 191, 0) },
			{ "Element", new Color(127, 127, 255) },
			{ "Keyword", new Color(0, 0, 255) },
		},
		backgroundColor: new Color(11, 8, 5),
		borderColor: new Color(127, 95, 63));

	protected override Piece HandleLanguageSpecificCodeTypes(string[] pieces, int index)
	{
		return new Piece(pieces[index], "Other");
	}
}
