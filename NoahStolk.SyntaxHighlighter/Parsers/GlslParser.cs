namespace NoahStolk.SyntaxHighlighter.Parsers;

public sealed class GlslParser : AbstractParser
{
	private static readonly Lazy<GlslParser> _lazy = new(() => new());

	private GlslParser()
	{
		string[] vectorTypes = ["bvec", "dvec", "ivec", "uvec", "vec"];
		List<string> vectorTypeKeywords = [];
		foreach (string vectorType in vectorTypes)
		{
			vectorTypeKeywords.Add(vectorType);
			for (int i = 2; i <= 4; i++)
				vectorTypeKeywords.Add($"{vectorType}{i}");
		}

		string[] matrixTypes = ["dmat", "mat"];
		List<string> matrixTypeKeywords = [];
		foreach (string matrixType in matrixTypes)
		{
			matrixTypeKeywords.Add(matrixType);
			for (int i = 2; i <= 4; i++)
			{
				matrixTypeKeywords.Add($"{matrixType}{i}");
				for (int j = i; j <= 4; j++)
					matrixTypeKeywords.Add($"{matrixType}{i}x{j}");
			}
		}

		CodeLanguage.ReservedKeywords["KeywordType"] = vectorTypeKeywords.Concat(matrixTypeKeywords).Concat(new[] { "double", "float", "int", "long", "short" }).ToArray();
	}

	public static GlslParser Instance => _lazy.Value;

	public override string Name => "GLSL";

	public override Language CodeLanguage { get; } = new(
		reservedKeywords: new()
		{
			["KeywordDefault"] = ["break", "case", "const", "continue", "default", "discard", "do", "else", "false", "flat", "for", "goto", "highp", "if", "in", "inout", "invariant", "layout", "lowp", "mediump", "noperspective", "out", "patch", "precision", "register", "return", "smooth", "struct", "subroutine", "switch", "uniform", "union", "varying", "void", "while"],
			["KeywordTypeSampler"] = ["isampler1D", "isampler1DArray", "isampler2D", "isampler2DArray", "isampler2DMS", "isampler2DMSArray", "isampler2DRect", "isampler3D", "isamplerBuffer", "isamplerCube", "isamplerCubeArray", "sample", "sampler1D", "sampler1DArray", "sampler1DArrayShadow", "sampler1DShadow", "sampler2D", "sampler2DArray", "sampler2DArrayShadow", "sampler2DMS", "sampler2DMSArray", "sampler2DRect", "sampler2DRectShadow", "sampler2DShadow", "sampler3D", "samplerBuffer", "samplerCube", "samplerCubeArray", "samplerCubeArrayShadow", "samplerCubeShadow", "usampler1D", "usampler1DArray", "usampler2D", "usampler2DArray", "usampler2DMS", "usampler2DMSArray", "usampler2DRect", "usampler3D", "usamplerBuffer", "usamplerCube", "usamplerCubeArray"],
		},
		separators: [' ', '\t', '\r', '\n', ',', '[', ']', '(', ')', '<', '>', ';', '.', ':']);

	public override Style CodeStyle { get; } = new(
		highlightColors: new()
		{
			{ "KeywordDefault", new(0, 127, 255) },
			{ "KeywordType", new(127, 63, 255) },
			{ "KeywordTypeSampler", new(127, 127, 255) },
			{ "KeywordFunctions", new(63, 255, 255) },
			{ "Number", new(127, 255, 127) },
			{ "Other", new(255, 255, 255) },
			{ "Function", new(255, 127, 255) },
			{ "Field", new(0, 255, 0) },
			{ "Struct", new(159, 255, 0) },
			{ "Comment", new(0, 159, 0) },
			{ "PreProcessorDirective", new(127, 127, 127) },
		},
		backgroundColor: new(11, 0, 0),
		borderColor: new(127, 0, 0));

	protected override Piece HandleLanguageSpecificCodeTypes(string[] pieces, int index)
	{
		string type;

		if (index < pieces.Length - 1 && pieces[index + 1][0] == '(' && pieces[index][0] != '(')
			type = "Function";
		else if (index > 1 && pieces[index - 1][0] == '.')
			type = "Field";
		else
			type = "Other";

		return new(pieces[index], type);
	}
}
