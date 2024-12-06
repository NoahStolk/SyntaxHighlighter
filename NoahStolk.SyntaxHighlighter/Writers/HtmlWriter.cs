using System.Web;

namespace NoahStolk.SyntaxHighlighter.Writers;

public static class HtmlWriter
{
	public static string Write(string baseElement, List<Piece> code, Style style)
	{
		StringBuilder sb = new($"<{baseElement} class=\"code\" style=\"background-color: {GetColorString(style.BackgroundColor)}; border-color: {GetColorString(style.BorderColor)};\">");
		foreach (Piece codePiece in code.Where(p => p.Code != "\n"))
			sb.Append("<span style=\"color: ").Append(GetColorString(style.HighlightColors[codePiece.Type])).Append(";\">").Append(HttpUtility.HtmlEncode(codePiece)).Append("</span>");
		sb.Append("</").Append(baseElement).Append('>');
		return sb.ToString();
	}

	private static string GetColorString(Color color)
	{
		return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
	}
}
