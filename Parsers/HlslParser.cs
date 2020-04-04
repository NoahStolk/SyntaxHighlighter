using System;
using System.Collections.Generic;

namespace SyntaxHighlighter.Parsers
{
	public sealed class HlslParser : AbstractParser
	{
		private static readonly Lazy<HlslParser> lazy = new Lazy<HlslParser>(() => new HlslParser());
		public static HlslParser Instance => lazy.Value;

		private HlslParser()
		{
			string[] types = new string[] { "bool", "double", "float", "half", "int", "min10float", "min12int", "min16float", "min16int", "min16uint", "uint" };
			List<string> typeKeywords = new List<string>();
			foreach (string type in types)
			{
				typeKeywords.Add(type);
				for (int i = 0; i < 4; i++)
				{
					typeKeywords.Add($"{type}{i + 1}");
					for (int j = i; j < 4; j++)
						typeKeywords.Add($"{type}{i + 1}x{j + 1}");
				}
			}
			CodeLanguage.ReservedKeywords["KeywordType"] = typeKeywords.ToArray();

			string[] functions = new string[] { "tex1D", "tex2D", "tex3D", "texCUBE" };
			string[] functionTypes = new string[] { "bias", "grad", "lod", "proj" };
			List<string> functionKeywords = new List<string> { "step", "sqrt", "abs", "pow", "saturate", "mul", "dot", "normalize", "clip" };
			foreach (string function in functions)
			{
				functionKeywords.Add(function);
				foreach (string functionType in functionTypes)
					functionKeywords.Add($"{function}{functionType}");
			}
			CodeLanguage.ReservedKeywords["KeywordFunctions"] = functionKeywords.ToArray();
		}

		public override string Name { get; } = "HLSL";

		// TODO: Add all keywords (there's a lot).
		public override Language CodeLanguage { get; } = new Language(
			reservedKeywords: new Dictionary<string, string[]>
			{
				{
					"KeywordDefault",
					new string[] { "break", "case", "const", "compile", "continue", "discard", "do", "else", "false", "for", "goto", "if", "int", "pass", "register", "return", "sampler", "sampler1D", "sampler2D", "sampler3D", "samplerCUBE", "sampler_state", "struct", "switch", "technique", "texture", "true", "while" }
				},
				{
					"KeywordControlStatement",
					new string[] { "loop", "branch" }
				},
				{
					"KeywordSemantic",
					new string[]{ "COLOR0" }
				}
			},
			separators: new char[] { ' ', '\t', '\r', '\n', ',', '[', ']', '(', ')', '<', '>', ';', '.' });

		public override Style CodeStyle { get; } = new Style(
			highlightColors: new Dictionary<string, Color>
			{
				{ "KeywordDefault", new Color(0, 127, 255) },
				{ "KeywordType", new Color(127, 63, 255) },
				{ "KeywordFunctions", new Color(63, 255, 255) },
				{ "KeywordSemantic", new Color(0, 191, 127) },
				{ "KeywordControlStatement", new Color(191, 63, 127) },
				{ "Number", new Color(127, 255, 127) },
				{ "Other", new Color(255, 255, 255) },
				{ "Function", new Color(255, 127, 255) },
				{ "Field", new Color(0, 255, 0) },
				{ "Struct", new Color(159, 255, 0) },
				{ "Comment", new Color(0, 159, 0) }
			},
			backgroundColor: new Color(11, 5, 11),
			borderColor: new Color(127, 63, 127));

		protected override Piece HandleLanguageSpecificCodeTypes(string[] pieces, int index)
		{
			string type;

			if (index < pieces.Length - 1 && pieces[index + 1][0] == '(')
				type = "Function";
			else if (index > 1 && pieces[index - 1][0] == '.')
				type = "Field";
			else
				type = "Other";

			return new Piece(pieces[index], type);
		}
	}
}