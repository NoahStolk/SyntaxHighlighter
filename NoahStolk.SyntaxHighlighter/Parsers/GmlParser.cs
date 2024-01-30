namespace NoahStolk.SyntaxHighlighter.Parsers;

public sealed class GmlParser : AbstractParser
{
	private static readonly Lazy<GmlParser> _lazy = new(() => new());

	private GmlParser()
	{
	}

	public static GmlParser Instance => _lazy.Value;

	public override string Name { get; } = "GML";

	public override Language CodeLanguage { get; } = new(
		reservedKeywords: new Dictionary<string, string[]>
		{
			{
				"KeywordDefault",
				new string[] { "self", "other", "all", "noone", "var", "enum", "global", "globalvar" }
			},
			{
				"KeywordConditional",
				new string[] { "break", "case", "continue", "do", "else", "exit", "for", "if", "repeat", "return", "switch", "while", "with" }
			},
			{
				"KeywordLocal",
				new string[] { "alarm", "bbox_bottom", "bbox_left", "bbox_right", "bbox_top", "depth", "direction", "friction", "gravity", "gravity_direction", "hspeed", "id", "image_alpha", "image_angle", "image_blend", "image_index", "image_number", "image_single", "image_speed", "image_xscale", "image_yscale", "mask_index", "object_index", "path_endaction", "path_index", "path_orientation", "path_position", "path_positionprevious", "path_scale", "path_speed", "persistent", "solid", "speed", "sprite_height", "sprite_index", "sprite_width", "sprite_xoffset", "sprite_yoffset", "timeline_index", "timeline_position", "timeline_speed", "visible", "vspeed", "x", "xprevious", "xstart", "y", "yprevious", "ystart" }
			},
			{
				"KeywordGlobal",
				new string[] { "argument", "argument0", "argument1", "argument10", "argument11", "argument12", "argument13", "argument14", "argument15", "argument2", "argument3", "argument4", "argument5", "argument6", "argument7", "argument8", "argument9", "argument_relative", "background_alpha", "background_blend", "background_color", "background_foreground", "background_height", "background_hspeed", "background_htiled", "background_index", "background_showcolor", "background_visible", "background_vspeed", "background_vtiled", "background_width", "background_x", "background_xscale", "background_y", "background_yscale", "caption_health", "caption_lives", "caption_score", "current_day", "current_hour", "current_minute", "current_month", "current_second", "current_time", "current_weekday", "current_year", "cursor_sprite", "delta_time", "error_last", "error_occurred", "event_action", "event_number", "event_object", "event_type", "fps", "game_id", "health", "instance_count", "instance_id", "keyboard_key", "keyboard_lastchar", "keyboard_lastkey", "keyboard_string", "lives", "mouse_button", "mouse_lastbutton", "mouse_x", "mouse_y", "room", "room_caption", "room_first", "room_height", "room_last", "room_persistent", "room_speed", "room_width", "score", "secure_mode", "show_health", "show_lives", "show_score", "temp_directory", "transition_kind", "transition_steps", "transition_time", "view_angle", "view_current", "view_enabled", "view_hborder", "view_hport", "view_hspeed", "view_hview", "view_object", "view_vborder", "view_visible", "view_vspeed", "view_wport", "view_wview", "view_xport", "view_xview", "view_yport", "view_yview", "working_directory" }
			},
		},
		separators: new char[] { ' ', '\t', '\r', '\n', ',', '[', ']', '(', ')', ';', '.' });

	public override Style CodeStyle { get; } = new(
		highlightColors: new Dictionary<string, Color>
		{
			{ "KeywordDefault", new(255, 31, 0) },
			{ "KeywordConditional", new(0, 127, 255) },
			{ "KeywordLocal", new(0, 255, 0) },
			{ "KeywordGlobal", new(0, 255, 91) },
			{ "Number", new(255, 0, 127) },
			{ "Other", new(255, 255, 255) },
			{ "String", new(255, 255, 0) },
			{ "Char", new(255, 191, 0) },
			{ "Function", new(255, 127, 0) },
			{ "Comment", new(0, 159, 0) },
		},
		backgroundColor: new(11, 5, 5),
		borderColor: new(127, 63, 63));

	protected override Piece HandleLanguageSpecificCodeTypes(string[] pieces, int index)
	{
		if (index < pieces.Length - 1 && pieces[index + 1][0] == '(' && pieces[index][0] != '(')
			return new(pieces[index], "Function");

		return new(pieces[index], "Other");
	}
}
