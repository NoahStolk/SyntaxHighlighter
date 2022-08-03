using NoahStolk.SyntaxHighlighter.Extensions;

namespace NoahStolk.SyntaxHighlighter.Parsers;

public abstract class AbstractParser
{
	public abstract string Name { get; }
	public abstract Language CodeLanguage { get; }
	public abstract Style CodeStyle { get; }

	private static List<Piece> CombinePieces(List<Piece> codePieces)
	{
		List<Piece> combinedCodePieces = new();
		for (int i = 0; i < codePieces.Count;)
		{
			Piece piece = codePieces[i];
			StringBuilder combinedCode = new(piece.Code);
			string type = piece.Type;

			int check = 1;
			while (i + check < codePieces.Count && codePieces[i + check].Type == type)
				combinedCode.Append(codePieces[i + check++].Code);

			combinedCodePieces.Add(new Piece(combinedCode.ToString(), type));

			i += check;
		}

		return combinedCodePieces;
	}

	private static List<string> SplitCodeByPreProcessorDirectives(string code)
	{
		List<string> splitByPpd = new();
		bool isInsidePpd = false;
		int changeIndex = 0;
		for (int i = 0; i < code.Length - 1; i++)
		{
			char c = code[i];

			if (isInsidePpd && c == '\r' && code[i + 1] == '\n'
			|| !isInsidePpd && c == '#')
			{
				isInsidePpd = !isInsidePpd;
				int next = isInsidePpd ? i : i + 1;
				splitByPpd.Add(code[changeIndex..next]);
				changeIndex = next;
			}
		}

		splitByPpd.Add(code[changeIndex..]);
		return splitByPpd;
	}

	// TODO: Detect multi-line comments.
	// TODO: Detect if quote comes before comment on the same line, which makes it a string or char instead.
	// TODO: Support varying types of comments based on the derived parser class, such as <!-- -->.
	// TODO: Add support for different kind of line breaks.
	private static List<string> SplitCodeByComments(string code)
	{
		List<string> splitByComments = new();
		bool isInsideComment = false;
		int changeIndex = 0;
		for (int i = 0; i < code.Length - 1; i++)
		{
			char c = code[i];

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

	private static List<string> SplitCodeByQuotes(string code, char quote)
	{
		char opposingQuote = quote switch
		{
			'"' => '\'',
			'\'' => '"',
			_ => throw new ArgumentException($"Parameter '{nameof(quote)}' was not a quote."),
		};

		List<string> splitByString = new();
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

	private static IEnumerable<(string Substring, bool IsBetween)> GetSubstringsBetween(string s, char start, char end)
	{
		StringBuilder sb = new();
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

	public List<Piece> Parse(string code, List<Piece>? externalDeclarations = null)
	{
		List<Piece> codePieces = new();

		List<Piece> detectedDeclarations = DetectDeclarations(code.SplitIncludeDelimiters(CodeLanguage.Separators));

		List<string> splitByPpd = SplitCodeByPreProcessorDirectives(code);
		for (int i = 0; i < splitByPpd.Count; i++)
		{
			bool isPpd = i % 2 == 1;
			if (isPpd)
			{
				codePieces.Add(new Piece(splitByPpd[i].TrimEnd(), "PreProcessorDirective"));
				continue;
			}

			List<string> splitByComments = SplitCodeByComments(splitByPpd[i]);
			for (int j = 0; j < splitByComments.Count; j++)
			{
				bool isComment = j % 2 == 1;
				if (isComment)
				{
					codePieces.Add(new Piece(splitByComments[j].TrimEnd(), "Comment"));
					continue;
				}

				List<string> splitByDoubleQuote = SplitCodeByQuotes(splitByComments[j], '"');
				for (int k = 0; k < splitByDoubleQuote.Count; k++)
				{
					bool isString = k % 2 == 1;
					if (isString)
					{
						if (k > 0 && splitByDoubleQuote[k - 1].Length > 0 && splitByDoubleQuote[k - 1][^1] == '$')
						{
							(string Substring, bool IsBetween)[] splitByCurlyBraces = GetSubstringsBetween(splitByDoubleQuote[k], '{', '}').ToArray();
							for (int l = 0; l < splitByCurlyBraces.Length; l++)
							{
								if (!splitByCurlyBraces[l].IsBetween)
									codePieces.Add(new Piece(splitByCurlyBraces[l].Substring, "String"));
								else
									codePieces.AddRange(Parse(splitByCurlyBraces[l].Substring, externalDeclarations));
							}
						}
						else
						{
							codePieces.Add(new Piece(splitByDoubleQuote[k], "String"));
						}

						continue;
					}

					List<string> splitBySingleQuote = SplitCodeByQuotes(splitByDoubleQuote[k], '\'');
					for (int l = 0; l < splitBySingleQuote.Count; l++)
					{
						bool isChar = l % 2 == 1;
						if (isChar)
						{
							codePieces.Add(new Piece(splitBySingleQuote[l], "Char"));
							continue;
						}

						string[] splitBySeparator = splitBySingleQuote[l].SplitIncludeDelimiters(CodeLanguage.Separators);
						for (int m = 0; m < splitBySeparator.Length; m++)
							HandleSplitBySeparator(splitBySeparator, ref m);
					}
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
			 && IsDigitsOnly(split[index + 2].Replace("f", string.Empty)))
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

			Piece? detectedDeclaration = detectedDeclarations?.FirstOrDefault(d => d.Code == s0);
			Piece? externalDeclaration = externalDeclarations?.FirstOrDefault(d => d.Code == s0);
			if (detectedDeclaration != null)
				codePieces.Add(detectedDeclaration);
			else if (externalDeclaration != null)
				codePieces.Add(externalDeclaration);
			else
				codePieces.Add(HandleLanguageSpecificCodeTypes(split, index));
		}
	}

	private static bool IsDigitsOnly(string s)
		=> !string.IsNullOrEmpty(s) && s.All(char.IsDigit);

	private bool IsReservedKeyword(string s, out string type)
	{
		type = string.Empty;

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

	protected virtual List<Piece> DetectDeclarations(string[] pieces) => new();

	protected abstract Piece HandleLanguageSpecificCodeTypes(string[] pieces, int index);
}
