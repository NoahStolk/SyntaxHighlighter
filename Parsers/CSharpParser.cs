using System;
using System.Collections.Generic;

namespace SyntaxHighlighter.Parsers
{
	public sealed class CSharpParser : AbstractParser
	{
		private static readonly Lazy<CSharpParser> lazy = new Lazy<CSharpParser>(() => new CSharpParser());
		public static CSharpParser Instance => lazy.Value;

		private CSharpParser()
		{
		}

		public override string Name { get; } = "C#";

		public override Language CodeLanguage { get; } = new Language(
			reservedKeywords: new Dictionary<string, string[]>
			{
				{
					"KeywordDefault",
					new string[] { "abstract", "as", "base", "bool", "byte", "char", "checked", "class", "const", "decimal", "default", "delegate", "double", "enum", "event", "explicit", "extern", "false", "fixed", "float", "implicit", "int", "interface", "internal", "is", "lock", "long", "namespace", "new", "null", "object", "operator", "out", "override", "params", "private", "protected", "public", "readonly", "ref", "sbyte", "sealed", "short", "sizeof", "stackalloc", "static", "string", "struct", "this", "throw", "true", "typeof", "uint", "ulong", "unchecked", "unsafe", "ushort", "using", "virtual", "void", "volatile" }
				},
				{
					"KeywordConditional",
					// "default", Only inside a switch statement...
					new string[] { "break", "case", "catch", "continue", "do", "else", "finally", "for", "foreach", "goto", "if", "in", "return", "switch", "try", "while" }
				},
				{
					"KeywordContextual",
					new string[] { "add", "alias", "ascending", "async", "await", "by", "descending", "dynamic", "equals", "from", "get", "global", "group", "into", "join", "let", "nameof", "on", "orderby", "partial", "remove", "select", "set", "unmanaged", "value", "var", "when", "where", "yield" }
				}
			},
			separators: new char[] { ' ', '\t', '\r', '\n', ',', '[', ']', '(', ')', '<', '>', '{', '}', ';', '.' });

		public override Style CodeStyle { get; } = new Style(
			highlightColors: new Dictionary<string, Color>
			{
				{ "KeywordDefault", new Color(0, 127, 255) },
				{ "KeywordConditional", new Color(191, 63, 255) },
				{ "KeywordContextual", new Color(95, 127, 255) },
				{ "Number", new Color(127, 255, 127) },
				{ "Other", new Color(255, 255, 255) },
				{ "String", new Color(255, 127, 0) },
				{ "Char", new Color(255, 191, 0) },
				{ "Function", new Color(255, 255, 127) },
				{ "Class", new Color(0, 255, 0) },
				{ "Struct", new Color(159, 255, 0) },
				{ "Constructor", new Color(63, 255, 159) },
				{ "Interface", new Color(191, 255, 0) },
				{ "Enum", new Color(95, 255, 0) },
				{ "TypeConstraint", new Color(159, 255, 127) },
				{ "Comment", new Color(0, 159, 0) }
			},
			backgroundColor: new Color(5, 11, 5),
			borderColor: new Color(63, 127, 63));

		protected override List<Piece> DetectDeclarations(string[] pieces)
		{
			List<Piece> declarations = new List<Piece>();
			for (int i = 0; i < pieces.Length; i++)
			{
				string twoBehind = i > 1 ? pieces[i - 2] : null;
				string type = twoBehind switch
				{
					"class" => "Class",
					"struct" => "Struct",
					"enum" => "Enum",
					"interface" => "Interface",
					"new" => "Class", // Guess
					"abstract" => "Class", // Guess
					"override" => "Class", // Guess
					"virtual" => "Class", // Guess
					_ => null
				};

				if (type != null)
					declarations.Add(new Piece(pieces[i], type));
			}
			return declarations;
		}

		protected override Piece HandleLanguageSpecificCodeTypes(string[] pieces, int index)
		{
			string type;

			if (index < pieces.Length - 1 && pieces[index + 1][0] == '(')
			{
				string twoBehind = index > 1 ? pieces[index - 2] : null;
				type = twoBehind switch
				{
					"new" => "Constructor",
					_ => "Function"
				};
			}
			else if (index < pieces.Length - 1 && pieces[index + 1][0] == '<') // Extra check for generics
			{
				type = "Class"; // Guess
			}
			else
			{
				type = "Other";
			}

			return new Piece(pieces[index], type);
		}
	}
}