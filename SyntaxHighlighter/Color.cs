using System;

namespace SyntaxHighlighter
{
	public struct Color
	{
		public byte R { get; }
		public byte G { get; }
		public byte B { get; }

		public Color(byte r, byte g, byte b)
		{
			R = r;
			G = g;
			B = b;
		}

		public override string ToString() => $"#{BitConverter.ToString(new[] { R, G, B }).Replace("-", "")}";
	}
}