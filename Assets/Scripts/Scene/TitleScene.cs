using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : SceneBase
{
	protected override void Loaded()
	{
		SceneManager.LoadScene(SceneDefine.SCENE_ID.GAME_START);
	}

	public override void StartUnload()
	{
	}
}
