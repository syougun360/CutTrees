using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeScene : SceneBase
{
	protected override void Loaded()
	{
		SceneManager.LoadScene(SceneDefine.SCENE_ID.BATTLE);
	}

	public override void StartUnload()
	{
	}
}
