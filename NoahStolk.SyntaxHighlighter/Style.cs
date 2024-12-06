namespace NoahStolk.SyntaxHighlighter;

public sealed class Style(Dictionary<string, Color> highlightColors, Color backgroundColor, Color borderColor)
{
	public Dictionary<string, Color> HighlightColors { get; } = highlightColors;
	public Color BackgroundColor { get; } = backgroundColor;
	public Color BorderColor { get; } = borderColor;
}
