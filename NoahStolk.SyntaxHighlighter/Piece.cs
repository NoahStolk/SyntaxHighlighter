using System.Web;

namespace NoahStolk.SyntaxHighlighter;

public class Piece
{
	public Piece(string code, string type)
	{
		Code = code;
		Type = type;
	}

	public string Code { get; }
	public string Type { get; }

	// TODO: Move to HtmlWriter.
	public override string ToString()
	{
		return HttpUtility.HtmlEncode(Code);
	}
}
