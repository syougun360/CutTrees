using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;
using UnityScene = UnityEngine.SceneManagement.Scene;

/// <summary>
/// シーンをロードする部分
/// </summary>
public class SceneLoader
{
	public SceneDefine.SCENE_ID SceneID { get; private set; }
	public string Name { get; private set; }

	bool addtive = false;
	bool async = false;

	AsyncOperation asyncOperation = null;

	/// <summary>
	/// 読込開始
	/// </summary>
	/// <param name="id"></param>
	/// <param name="addtive"></param>
	/// <param name="async"></param>
	public SceneLoader(SceneDefine.SCENE_ID id, bool addtive,bool async)
	{
		SceneID = id;
		Name = SceneDefine.SceneNames[(int)id];
		this.addtive = addtive;
		this.async = async;

		if (async)
		{
			var param = new UnityEngine.SceneManagement.LoadSceneParameters();
			if (addtive)
			{
				param.loadSceneMode = UnityEngine.SceneManagement.LoadSceneMode.Additive;
			}
			else
			{
				param.loadSceneMode = UnityEngine.SceneManagement.LoadSceneMode.Single;
			}

			asyncOperation = UnitySceneManager.LoadSceneAsync(Name, param);
		}
		else
		{
			if (addtive)
			{
				UnitySceneManager.LoadScene(Name, UnityEngine.SceneManagement.LoadSceneMode.Additive);
			}
			else
			{
				UnitySceneManager.LoadScene(Name, UnityEngine.SceneManagement.LoadSceneMode.Single);
			}
		}
	}

	/// <summary>
	/// 更新
	/// </summary>
	public bool Update()
	{
		if (async)
		{
			if (!asyncOperation.isDone)
			{
				return false;
			}
		}

		UnityScene scene = UnitySceneManager.GetSceneByName(Name);
		if (scene.isLoaded)
		{
			return true;
		}

		return false;
	}
}
