using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(GraphicRaycaster))]
public abstract class UIBase : MonoBehaviour
{
	class PlayAnimationData
	{
		public AnimationState animationState = null;
		public int animationIndex = 0;
	}

	public enum SEQ_STATE
	{
		NONE,
		OPEN,
		UPDATE,
		CLOSE
	}

	SeqStateUtility<SEQ_STATE> seqState = new SeqStateUtility<SEQ_STATE>(SEQ_STATE.NONE);

	RectTransform cacheRectTrans = null;
	GameObject cacheGameObject = null;

	Animation[] cacheAnimationList = null;
	List<PlayAnimationData> cacheAnimationStateList = new List<PlayAnimationData>();

	Canvas cacheCanvas = null;
	GraphicRaycaster cacheGraphicRaycaster = null;

	private void Awake()
	{
		cacheCanvas = GetComponent<Canvas>();
		cacheGraphicRaycaster = GetComponent<GraphicRaycaster>();
		cacheRectTrans = transform as RectTransform;
		cacheGameObject = gameObject;
		cacheAnimationList = GetComponentsInChildren<Animation>();
		ResetButton();
	}

	private void Update()
	{
		switch (seqState.GetState())
		{
			case SEQ_STATE.OPEN:
				for (int i = cacheAnimationStateList.Count - 1; i >= 0; i--)
				{
					if (cacheAnimationStateList[i].animationState.normalizedTime >= 1.0f)
					{
						cacheAnimationList[cacheAnimationStateList[i].animationIndex].Stop();
						cacheAnimationStateList.RemoveAt(i);
					}
				}

				bool opend = cacheAnimationStateList.Count == 0;
				UpdateOpenUI(opend);
				if (opend)
				{
					seqState.ChangeState(SEQ_STATE.UPDATE);
				}
				break;
			case SEQ_STATE.UPDATE:
				UpdateUI();
				break;
			case SEQ_STATE.CLOSE:
				for (int i = cacheAnimationStateList.Count - 1; i >= 0; i--)
				{
					if (cacheAnimationStateList[i].animationState.normalizedTime >= 1.0f)
					{
						cacheAnimationList[cacheAnimationStateList[i].animationIndex].Stop();
						cacheAnimationStateList.RemoveAt(i);
					}
				}

				bool closed = cacheAnimationStateList.Count == 0;
				UpdateCloseUI(closed);
				if (closed)
				{
					cacheCanvas.enabled = false;
					cacheGraphicRaycaster.enabled = false;


					seqState.ChangeState(SEQ_STATE.NONE);
				}
				break;
		}
	}

	protected abstract void UpdateOpenUI(bool change);
	protected abstract void UpdateUI();
	protected abstract void UpdateCloseUI(bool change);


	public void StartOpen()
	{
		if (!IsClosed())
		{
			return;
		}

		for (int i = 0; i < cacheAnimationList.Length; i++)
		{
			foreach (AnimationState state in cacheAnimationList[i])
			{
				string clipName = state.name;
				int index = clipName.LastIndexOf("_in");
				if (index == clipName.Length - 3)
				{
					cacheAnimationList[i].Stop();

					AnimationState playState = cacheAnimationList[i].PlayQueued(clipName);
					cacheAnimationStateList.Add(new PlayAnimationData() {
						animationState = playState,
						animationIndex = i
					});
				}
			}
		}

		cacheGraphicRaycaster.enabled = true;
		cacheCanvas.enabled = true;

		seqState.ChangeState(SEQ_STATE.OPEN);
		Open();
	}

	protected virtual void Open()
	{

	}

	public void StartClose()
	{
		if (!IsOpend())
		{
			return;
		}

		for (int i = 0; i < cacheAnimationList.Length; i++)
		{
			foreach (AnimationState state in cacheAnimationList[i])
			{
				string clipName = state.name;
				int index = clipName.LastIndexOf("_out");
				if (index == clipName.Length - 4)
				{
					cacheAnimationList[i].Stop();

					AnimationState playState = cacheAnimationList[i].PlayQueued(clipName);
					cacheAnimationStateList.Add(new PlayAnimationData()
					{
						animationState = playState,
						animationIndex = i
					});
				}
			}
		}

		seqState.ChangeState(SEQ_STATE.CLOSE);
		Close();
	}

	protected virtual void Close()
	{

	}

	public void ResetButton()
	{
		Button[] buttons = GetComponentsInChildren<Button>();
		for (int i = 0; i < buttons.Length; i++)
		{
			Button button = buttons[i];
			button.onClick.RemoveAllListeners();

			string buttonName = button.name;
			int nameLength = buttonName.Length;
			string numberStr = buttonName.Substring(nameLength - 2, 2);
			numberStr = numberStr.Replace("_", "");
			int number = -1;
			if (!int.TryParse(numberStr, out number))
			{
				number = -1;
			}
			else
			{
				buttonName = buttonName.Substring(0, nameLength - 2);
			}

			int nameHash = buttonName.GetHashCode();
			button.onClick.AddListener(() => {
				OnButtonCallbackListener(button, nameHash , number);
			});
		}
	}

	void OnButtonCallbackListener(Button button, int buttonNameHash, int number)
	{
		if (IsOpend())
		{
			if (number >= 0)
			{
				OnButtonSelectCallback(button, buttonNameHash, number);
			}
			else
			{
				OnButtonCallback(button, buttonNameHash);
			}
		}
	}

	protected virtual void OnButtonCallback(Button button, int buttonNameHash)
	{

	}

	protected virtual void OnButtonSelectCallback(Button button, int buttonNameHash, int number)
	{

	}

	public RectTransform GetRectTrans()
	{
		return cacheRectTrans;
	}

	public GameObject GetGameObject()
	{
		return cacheGameObject;
	}

	public bool IsOpend()
	{
		return seqState.GetState() == SEQ_STATE.UPDATE;
	}

	public bool IsClosed()
	{
		return seqState.GetState() == SEQ_STATE.NONE;
	}
}
