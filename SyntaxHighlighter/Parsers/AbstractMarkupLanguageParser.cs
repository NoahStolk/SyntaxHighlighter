using System.Collections.Generic;

namespace SyntaxHighlighter.Parsers
{
	public abstract class AbstractMarkupLanguageParser : AbstractParser
	{
		public override Language CodeLanguage { get; } = new Language(
			reservedKeywords: new Dictionary<string, string[]>(),
			separators: new char[] { ' ', '\t', '\r', '\n', '<', '>' });

		protected override Piece HandleLanguageSpecificCodeTypes(string[] pieces, int index)
		{
			string type = index >= 1 && pieces[index - 1][0] == '<' ? "Element" : "Other";

			return new Piece(pieces[index], type);
		}
	}
}