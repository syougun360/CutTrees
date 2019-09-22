using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryScene : SceneBase
{
	protected override void Loaded()
	{
		DontDestroyOnLoad(gameObject);
		SceneManager.LoadScene(SceneDefine.SCENE_ID.TITLE);
	}

	public override void StartUnload()
	{
	}
}
