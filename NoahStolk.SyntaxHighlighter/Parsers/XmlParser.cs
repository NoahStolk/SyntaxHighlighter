namespace NoahStolk.SyntaxHighlighter.Parsers;

public sealed class XmlParser : AbstractMarkupLanguageParser
{
	private static readonly Lazy<XmlParser> _lazy = new(() => new());

	private XmlParser()
	{
	}

	public static XmlParser Instance => _lazy.Value;

	public override string Name => "XML";

	public override Style CodeStyle { get; } = new(
		highlightColors: new()
		{
			{ "Number", new(127, 255, 127) },
			{ "Other", new(111, 223, 223) },
			{ "String", new(255, 127, 0) },
			{ "Char", new(255, 191, 0) },
			{ "Element", new(127, 127, 255) },
		},
		backgroundColor: new(5, 5, 11),
		borderColor: new(63, 63, 127));
}
