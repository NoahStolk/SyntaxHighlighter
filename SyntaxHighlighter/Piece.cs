using System.Web;

namespace SyntaxHighlighter
{
	public class Piece
	{
		public string Code { get; }
		public string Type { get; }

		public Piece(string code, string type)
		{
			Code = code;
			Type = type;
		}

		public override string ToString() => HttpUtility.HtmlEncode(Code);
	}
}