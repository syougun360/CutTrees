
public class SceneDefine
{
	public const int MAX_SCENE_COUNT = (int)SCENE_ID.MAX;

	/// <summary>
	/// シーンID
	/// </summary>
	public enum SCENE_ID
	{
		NONE = -1,

		MAIN,
		ENTRY,
		TITLE,
		GAME_START,
		HOME,
		BATTLE,

		MAX,
	}

	/// <summary>
	/// シーン名
	/// </summary>
	public static readonly string[] SceneNames = new string[MAX_SCENE_COUNT] {
		"MainScene",
		"EntryScene",
		"TitleScene",
		"GameStartScene",
		"HomeScene",
		"BattleScene",
	};
}