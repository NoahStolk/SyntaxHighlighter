using System;
using System.Collections.Generic;

namespace NoahStolk.SyntaxHighlighter.Parsers;

public sealed class JsonParser : AbstractParser
{
	private static readonly Lazy<JsonParser> _lazy = new(() => new());

	private JsonParser()
	{
	}

	public static JsonParser Instance => _lazy.Value;

	public override string Name { get; } = "JSON";

	public override Language CodeLanguage { get; } = new(
		reservedKeywords: new Dictionary<string, string[]>
		{
			{
				"Keyword",
				new string[] { "false", "null", "true" }
			},
		},
		separators: new char[] { ' ', '\t', '\r', '\n', ',', '[', ']', '(', ')', '<', '>', ';', '.' });

	public override Style CodeStyle { get; } = new(
		highlightColors: new Dictionary<string, Color>
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
		=> new(pieces[index], "Other");
}
