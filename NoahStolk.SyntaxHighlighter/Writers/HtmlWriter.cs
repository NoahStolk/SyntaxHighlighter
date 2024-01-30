namespace NoahStolk.SyntaxHighlighter.Writers;

public static class HtmlWriter
{
	public static string Write(string baseElement, List<Piece> code, Style style)
	{
		StringBuilder sb = new($"<{baseElement} class=\"code\" style=\"background-color: {style.BackgroundColor}; border-color: {style.BorderColor};\">");
		foreach (Piece codePiece in code.Where(p => p.Code != "\n"))
			sb.Append("<span style=\"color: ").Append(style.HighlightColors[codePiece.Type]).Append(";\">").Append(codePiece).Append("</span>");
		sb.Append("</").Append(baseElement).Append('>');
		return sb.ToString();
	}
}
