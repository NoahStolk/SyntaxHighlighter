using System;
using System.Collections.Generic;

namespace SyntaxHighlighter.Parsers
{
	public sealed class XmlParser : AbstractMarkupLanguageParser
	{
		private static readonly Lazy<XmlParser> _lazy = new Lazy<XmlParser>(() => new XmlParser());

		private XmlParser()
		{
		}

		public static XmlParser Instance => _lazy.Value;

		public override string Name { get; } = "XML";

		public override Style CodeStyle { get; } = new Style(
			highlightColors: new Dictionary<string, Color>
			{
				{ "Number", new Color(127, 255, 127) },
				{ "Other", new Color(111, 223, 223) },
				{ "String", new Color(255, 127, 0) },
				{ "Char", new Color(255, 191, 0) },
				{ "Element", new Color(127, 127, 255) },
			},
			backgroundColor: new Color(5, 5, 11),
			borderColor: new Color(63, 63, 127));
	}
}