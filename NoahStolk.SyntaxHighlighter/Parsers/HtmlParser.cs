using System;
using System.Collections.Generic;

namespace NoahStolk.SyntaxHighlighter.Parsers
{
	public sealed class HtmlParser : AbstractMarkupLanguageParser
	{
		private static readonly Lazy<HtmlParser> _lazy = new(() => new());

		private HtmlParser()
		{
		}

		public static HtmlParser Instance => _lazy.Value;

		public override string Name { get; } = "HTML";

		public override Style CodeStyle { get; } = new(
			highlightColors: new Dictionary<string, Color>
			{
				{ "Number", new(191, 191, 191) },
				{ "Other", new(127, 191, 255) },
				{ "String", new(191, 191, 191) },
				{ "Char", new(191, 191, 191) },
				{ "Element", new(63, 127, 255) },
			},
			backgroundColor: new(11, 11, 5),
			borderColor: new(127, 127, 63));
	}
}
