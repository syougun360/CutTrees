using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : SceneBase
{
	protected override void Loaded()
	{
		DontDestroyOnLoad(gameObject);
		SceneManager.LoadScene(SceneDefine.SCENE_ID.ENTRY);
	}

	public override void StartUnload()
	{
	}
}
