namespace NoahStolk.SyntaxHighlighter;

public class Style
{
	public Style(Dictionary<string, Color> highlightColors, Color backgroundColor, Color borderColor)
	{
		HighlightColors = highlightColors;
		BackgroundColor = backgroundColor;
		BorderColor = borderColor;
	}

	public Dictionary<string, Color> HighlightColors { get; }
	public Color BackgroundColor { get; }
	public Color BorderColor { get; }
}
