namespace NoahStolk.SyntaxHighlighter;

public sealed class Piece(string code, string type)
{
	public string Code { get; } = code;
	public string Type { get; } = type;
}
