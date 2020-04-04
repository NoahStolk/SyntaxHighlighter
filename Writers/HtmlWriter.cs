using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SyntaxHighlighter.Writers
{
	public static class HtmlWriter
	{
		public static string Write(string baseElement, List<Piece> code, Style style)
		{
			StringBuilder sb = new StringBuilder($"<{baseElement} class=\"code\" style=\"background-color: {style.BackgroundColor}; border-color: {style.BorderColor};\">");
			foreach (Piece codePiece in code.Where(p => p.Code != "\n"))
				sb.Append($"<span style=\"color: {style.HighlightColors[codePiece.Type]};\">{codePiece}</span>");
			sb.Append($"</{baseElement}>");
			return sb.ToString();
		}
	}
}