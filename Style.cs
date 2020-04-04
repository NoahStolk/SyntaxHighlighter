using System.Collections.Generic;

namespace SyntaxHighlighter
{
	public class Style
	{
		public Dictionary<string, Color> HighlightColors { get; }
		public Color BackgroundColor { get; }
		public Color BorderColor { get; }

		public Style(Dictionary<string, Color> highlightColors, Color backgroundColor, Color borderColor)
		{
			HighlightColors = highlightColors;
			BackgroundColor = backgroundColor;
			BorderColor = borderColor;
		}
	}
}