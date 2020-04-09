using NetBase.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SyntaxHighlighter.Parsers
{
	public abstract class AbstractParser
	{
		public abstract string Name { get; }
		public abstract Language CodeLanguage { get; }
		public abstract Style CodeStyle { get; }

		private List<Piece> CombinePieces(List<Piece> codePieces)
		{
			List<Piece> combinedCodePieces = new List<Piece>();
			for (int i = 0; i < codePieces.Count;)
			{
				Piece piece = codePieces[i];
				StringBuilder combinedCode = new StringBuilder(piece.Code);
				string type = piece.Type;

				int check = 1;
				while (i + check < codePieces.Count && codePieces[i + check].Type == type)
					combinedCode.Append(codePieces[i + check++].Code);

				combinedCodePieces.Add(new Piece(combinedCode.ToString(), type));

				i += check;
			}

			return combinedCodePieces;
		}

		// TODO: Detect multi-line comments.
		// TODO: Detect if quote comes before comment on the same line, which makes it a string or char instead.
		// TODO: Support varying types of comments based on the derived parser class, such as <!-- -->.
		private List<string> SplitCodeByComments(string code)
		{
			List<string> splitByComments = new List<string>();
			bool isInsideComment = false;
			int changeIndex = 0;
			for (int i = 0; i < code.Length - 1; i++)
			{
				char c = code[i];

				// TODO: Add support for different kind of line breaks.
				if (isInsideComment && c == '\r' && code[i + 1] == '\n'
				|| !isInsideComment && c == '/' && code[i + 1] == '/')
				{
					isInsideComment = !isInsideComment;
					int next = isInsideComment ? i : i + 1;
					splitByComments.Add(code[changeIndex..next]);
					changeIndex = next;
				}
			}
			splitByComments.Add(code[changeIndex..]);
			return splitByComments;
		}

		private List<string> SplitCodeByQuotes(string code, char quote)
		{
			char opposingQuote = quote switch
			{
				'"' => '\'',
				'\'' => '"',
				_ => throw new ArgumentException($"Parameter '{nameof(quote)}' was not a quote.")
			};

			List<string> splitByString = new List<string>();
			bool isInsideQuotes = false;
			int changeIndex = 0;
			for (int i = 0; i < code.Length; i++)
			{
				char c = code[i];

				// TODO: Fix code for \\\, \\\\, etc...
				bool isEscapeCharEscaped = i > 1 && code[i - 1] == '\\' && code[i - 2] == '\\';
				bool isEscaped = i > 0 && code[i - 1] == '\\' && !isEscapeCharEscaped;
				bool isSurroundedByOpposingQuotes = i > 0 && i < code.Length - 1 && code[i - 1] == opposingQuote && code[i + 1] == opposingQuote;

				if (c == quote && !isEscaped && !isSurroundedByOpposingQuotes)
				{
					isInsideQuotes = !isInsideQuotes;
					int next = isInsideQuotes ? i : i + 1;
					splitByString.Add(code[changeIndex..next]);
					changeIndex = next;
				}
			}
			splitByString.Add(code[changeIndex..]);
			return splitByString;
		}

		private IEnumerable<(string substring, bool isBetween)> GetSubstringsBetween(string s, char start, char end)
		{
			StringBuilder sb = new StringBuilder();
			bool active = false;
			for (int i = 0; i < s.Length; i++)
			{
				char c = s[i];
				if (c == start)
				{
					active = true;
					yield return (sb.ToString(), false);
					sb.Clear();
					sb.Append(c);
				}
				else if (c == end)
				{
					if (active)
					{
						active = false;
						sb.Append(c);
						yield return (sb.ToString(), true);
						sb.Clear();
					}
					else
					{
						sb.Append(c);
						yield return (sb.ToString(), false);
						sb.Clear();
					}
				}
				else
				{
					sb.Append(c);
				}
			}
			yield return (sb.ToString(), false);
		}

		public List<Piece> Parse(string code, List<Piece> externalDeclarations = null)
		{
			List<Piece> codePieces = new List<Piece>();

			List<Piece> detectedDeclarations = DetectDeclarations(code.SplitIncludeDelimiters(CodeLanguage.Separators));

			List<string> splitByComments = SplitCodeByComments(code);
			for (int i = 0; i < splitByComments.Count; i++)
			{
				bool isComment = i % 2 == 1;
				if (isComment)
				{
					codePieces.Add(new Piece(splitByComments[i].TrimEnd(), "Comment"));
					continue;
				}

				List<string> splitByDoubleQuote = SplitCodeByQuotes(splitByComments[i], '"');
				for (int j = 0; j < splitByDoubleQuote.Count; j++)
				{
					bool isString = j % 2 == 1;
					if (isString)
					{
						if (j > 0 && splitByDoubleQuote[j - 1].Length > 0 && splitByDoubleQuote[j - 1][^1] == '$')
						{
							(string substring, bool isBetween)[] splitByCurlyBraces = GetSubstringsBetween(splitByDoubleQuote[j], '{', '}').ToArray();
							for (int k = 0; k < splitByCurlyBraces.Length; k++)
							{
								if (!splitByCurlyBraces[k].isBetween)
									codePieces.Add(new Piece(splitByCurlyBraces[k].substring, "String"));
								else
									codePieces.AddRange(Parse(splitByCurlyBraces[k].substring, externalDeclarations));
							}
						}
						else
						{
							codePieces.Add(new Piece(splitByDoubleQuote[j], "String"));
						}
						continue;
					}

					List<string> splitBySingleQuote = SplitCodeByQuotes(splitByDoubleQuote[j], '\'');
					for (int k = 0; k < splitBySingleQuote.Count; k++)
					{
						bool isChar = k % 2 == 1;
						if (isChar)
						{
							codePieces.Add(new Piece(splitBySingleQuote[k], "Char"));
							continue;
						}

						string[] splitBySeparator = splitBySingleQuote[k].SplitIncludeDelimiters(CodeLanguage.Separators);
						for (int l = 0; l < splitBySeparator.Length; l++)
							HandleSplitBySeparator(splitBySeparator, ref l);
					}
				}
			}

			return CombinePieces(codePieces);

			void HandleSplitBySeparator(string[] split, ref int index)
			{
				string s0 = split[index];

				if (index < split.Length - 2
				 && split[index + 1] == "."
				 && IsDigitsOnly(s0[0] == '-' ? s0[1..] : s0)
				 && IsDigitsOnly(split[index + 2].Replace("f", "")))
				{
					string newPiece = string.Concat(s0, split[index + 1], split[index + 2]);
					codePieces.Add(new Piece(newPiece, "Number"));
					index += 2;
					return;
				}

				if (IsDigitsOnly(s0[0] == '-' ? s0[1..] : s0))
				{
					codePieces.Add(new Piece(s0, "Number"));
					return;
				}

				if (IsReservedKeyword(s0, out string type))
				{
					codePieces.Add(new Piece(s0, type));
					return;
				}

				Piece detectedDeclaration = detectedDeclarations?.FirstOrDefault(d => d.Code == s0);
				Piece externalDeclaration = externalDeclarations?.FirstOrDefault(d => d.Code == s0);
				if (detectedDeclaration != null)
					codePieces.Add(detectedDeclaration);
				else if (externalDeclaration != null)
					codePieces.Add(externalDeclaration);
				else
					codePieces.Add(HandleLanguageSpecificCodeTypes(split, index));
			}
		}

		private static bool IsDigitsOnly(string s) => !string.IsNullOrEmpty(s) && s.All(char.IsDigit);

		private bool IsReservedKeyword(string s, out string type)
		{
			type = null;

			foreach (KeyValuePair<string, string[]> kvp in CodeLanguage.ReservedKeywords)
			{
				if (Array.IndexOf(kvp.Value, s) != -1)
				{
					type = kvp.Key;
					return true;
				}
			}

			return false;
		}

		protected virtual List<Piece> DetectDeclarations(string[] pieces) => new List<Piece>();

		protected abstract Piece HandleLanguageSpecificCodeTypes(string[] pieces, int index);
	}
}