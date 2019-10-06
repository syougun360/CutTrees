using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene : SceneBase
{
	enum STATE
	{
		INIT,
		LOAD_WAIT,
		UPDATE,
		TREE_DOWN,
		END,
	}

	STATE state = STATE.INIT;

	protected override void Loaded()
	{
		state = STATE.INIT;
	}

	public override void StartUnload()
	{
	}

	private void Update()
	{
		switch (state)
		{
			case STATE.INIT:
				if (MasterDataManager.IsLoaded())
				{
					state = STATE.LOAD_WAIT;

                    GameParam.Setup();

                    EffectManager.StartLoad();
					WeaponManager.StartLoad();
				}
				break;
			case STATE.LOAD_WAIT:
				if (EffectManager.IsLoaded() &&
					WeaponManager.IsLoaded())
				{
					state = STATE.UPDATE;
					EventManager.Dispatcher(EVENT_ID.START_BATTLE);
				}
				break;
			case STATE.UPDATE:
				if (TreeObject.IsAnimationPlaying())
				{
					state = STATE.TREE_DOWN;
				}
				break;
			case STATE.TREE_DOWN:
				if (!TreeObject.IsAnimationPlaying())
				{
					state = STATE.UPDATE;
				}
				break;
			case STATE.END:
				break;
		}
	}

	public static bool IsPlayableState()
	{
		var battleScene = SceneManager.GetSceneObject(SceneDefine.SCENE_ID.BATTLE) as BattleScene;
		if (battleScene != null)
		{
			return battleScene.state == STATE.UPDATE;
		}

		return false;
	}
}
