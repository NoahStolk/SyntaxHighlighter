namespace NoahStolk.SyntaxHighlighter.Parsers;

public sealed class HtmlParser : AbstractMarkupLanguageParser
{
	private static readonly Lazy<HtmlParser> _lazy = new(() => new HtmlParser());

	private HtmlParser()
	{
	}

	public static HtmlParser Instance => _lazy.Value;

	public override string Name => "HTML";

	public override Style CodeStyle { get; } = new(
		highlightColors: new Dictionary<string, Color>
		{
			{ "Number", new Color(191, 191, 191) },
			{ "Other", new Color(127, 191, 255) },
			{ "String", new Color(191, 191, 191) },
			{ "Char", new Color(191, 191, 191) },
			{ "Element", new Color(63, 127, 255) },
		},
		backgroundColor: new Color(11, 11, 5),
		borderColor: new Color(127, 127, 63));
}
