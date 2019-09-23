using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// モーション制御
/// </summary>
public class CharaMotionController : MonoBehaviour
{
	readonly int IDLE_NAME_HASH = Animator.StringToHash("Idle");

	[SerializeField]
	int animatorLayerIndex = 0;

	public Animator CacheAnimator { get; private set; }

	PlayerCharacter character = null;

	public void Setup(PlayerCharacter charaBase)
	{
		this.character = charaBase;
		CacheAnimator = GetComponent<Animator>();
	}

	private void Update()
	{
		// TODO：モーションイベント実装する。
		//AnimatorStateInfo current = CacheAnimator.GetCurrentAnimatorStateInfo(animatorLayerIndex);
		//AnimatorStateInfo next = CacheAnimator.GetNextAnimatorStateInfo(animatorLayerIndex);

	}

	/// <summary>
	/// モーションを再生する。
	/// </summary>
	/// <param name="stateHashName"></param>
	/// <param name="crossFadeTime"></param>
	public void PlayMotion(int stateHashName,float crossFadeTime = 0.2f)
	{
		CacheAnimator.CrossFade(stateHashName, crossFadeTime);
	}

	/// <summary>
	/// Idle状態にする。
	/// </summary>
	public void PlayIdle()
	{
		int count = CacheAnimator.parameters.Length;
		for (int i = 0; i < count; i++)
		{
			AnimatorControllerParameter param = CacheAnimator.parameters[i];
			if (param.type == AnimatorControllerParameterType.Bool)
			{
				CacheAnimator.SetBool(param.name, param.defaultBool);
			}
			else if (param.type == AnimatorControllerParameterType.Int)
			{
				CacheAnimator.SetInteger(param.name, param.defaultInt);
			}
			else if (param.type == AnimatorControllerParameterType.Float)
			{
				CacheAnimator.SetFloat(param.name, param.defaultFloat);
			}
		}

		PlayMotion(IDLE_NAME_HASH);
	}

	/// <summary>
	/// モーションが終わったか？
	/// </summary>
	/// <returns></returns>
	public bool IsMotionEnd()
	{
		AnimatorStateInfo current = CacheAnimator.GetCurrentAnimatorStateInfo(animatorLayerIndex);
		if (!current.loop)
		{
			if (current.normalizedTime >= 1.0f)
			{
				return true;
			}
		}

		return false;
	}

	/// <summary>
	/// 再生時間を取得（ノーマライズ）
	/// </summary>
	/// <returns></returns>
	public float GetNormalizedTime()
	{
		AnimatorStateInfo current = CacheAnimator.GetCurrentAnimatorStateInfo(animatorLayerIndex);
		return current.normalizedTime;
	}

	/// <summary>
	/// 再生中のアニメーションの名前ハッシュ
	/// </summary>
	/// <returns></returns>
	public bool IsShortNameHash(int targetNameHash)
	{
		if (CacheAnimator.GetCurrentAnimatorClipInfoCount(animatorLayerIndex) > 0)
		{
			AnimatorStateInfo current = CacheAnimator.GetCurrentAnimatorStateInfo(animatorLayerIndex);
			if (current.shortNameHash == targetNameHash)
			{
				return true;
			}
		}

		return false;
	}

	/// <summary>
	/// 再生速度を設定
	/// </summary>
	/// <param name="speed"></param>
	public void SetSpeed(float speed)
	{
		CacheAnimator.speed = speed;
	}

	public void SetBoolParam(string paramName, bool value)
	{
		CacheAnimator.SetBool(paramName, value);
	}
	public void SetIntParam(string paramName, int value)
	{
		CacheAnimator.SetInteger(paramName, value);
	}
	public void SetFloatParam(string paramName, float value)
	{
		CacheAnimator.SetFloat(paramName, value);
	}
	public void SetTriggerParam(string paramName)
	{
		CacheAnimator.SetTrigger(paramName);
	}

	public bool GetBoolParam(string paramName)
	{
		return CacheAnimator.GetBool(paramName);
	}
}
