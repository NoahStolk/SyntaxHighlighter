using System;
using System.Collections.Generic;

namespace SyntaxHighlighter.Parsers
{
	public sealed class JsonParser : AbstractParser
	{
		private static readonly Lazy<JsonParser> _lazy = new Lazy<JsonParser>(() => new JsonParser());

		private JsonParser()
		{
		}

		public static JsonParser Instance => _lazy.Value;

		public override string Name { get; } = "JSON";

		public override Language CodeLanguage { get; } = new Language(
			reservedKeywords: new Dictionary<string, string[]>
			{
				{
					"Keyword",
					new string[] { "false", "null", "true" }
				},
			},
			separators: new char[] { ' ', '\t', '\r', '\n', ',', '[', ']', '(', ')', '<', '>', ';', '.' });

		public override Style CodeStyle { get; } = new Style(
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
			=> new Piece(pieces[index], "Other");
	}
}