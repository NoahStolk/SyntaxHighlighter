namespace NoahStolk.SyntaxHighlighter;

public readonly record struct Color(byte R, byte G, byte B)
{
	// TODO: Move to HtmlWriter.
	public override string ToString()
	{
		return $"#{BitConverter.ToString([R, G, B]).Replace("-", string.Empty)}";
	}
}
