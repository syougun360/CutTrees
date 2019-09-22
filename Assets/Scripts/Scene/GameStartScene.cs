using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartScene : SceneBase
{
	protected override void Loaded()
	{
		SceneManager.LoadScene(SceneDefine.SCENE_ID.HOME);
	}

	public override void StartUnload()
	{
	}
}
