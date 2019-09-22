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
		END,
	}

	STATE state = STATE.INIT;

	protected override void Loaded()
	{
		state = STATE.LOAD_WAIT;
		EffectManager.StartLoad();
	}

	public override void StartUnload()
	{
	}

	private void Update()
	{
		switch (state)
		{
			case STATE.LOAD_WAIT:
				if (EffectManager.IsLoaded())
				{
					state = STATE.UPDATE;
				}
				break;
			case STATE.UPDATE:
				break;
			case STATE.END:
				break;
		}
	}
}
