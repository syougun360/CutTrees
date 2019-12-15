using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIBattleDamage : MonoBehaviour
{
	class DamageData
	{
		public bool active;

		public Transform transform;
		public Animation animation;
		public AnimationState animationState;
		public TextMeshProUGUI text;
	}

	[SerializeField]
	GameObject baseDamageObject = null;

	[SerializeField]
	int createDamageObjectCount = 10;

    [SerializeField]
    Canvas canvas = null;

	DamageData[] damageDatas = null;

	private void Awake()
	{
		damageDatas = new DamageData[createDamageObjectCount];
		for (int i = 0; i < createDamageObjectCount; i++)
		{
			var createObject = GameObject.Instantiate(baseDamageObject, transform);
			createObject.transform.position = GlobalDefine.DisablePos;

			DamageData data = new DamageData();
			data.transform = createObject.transform;
			data.animation = createObject.GetComponent<Animation>();
			data.text = createObject.GetComponentInChildren<TextMeshProUGUI>();
			damageDatas[i] = data;
		}

		baseDamageObject.SetActive(false);
	}

	private void Update()
	{
		for (int i = 0; i < damageDatas.Length; i++)
		{
			if (!damageDatas[i].active)
			{
				continue;
			}

			if (damageDatas[i].animationState.normalizedTime >= 1.0f)
			{
				damageDatas[i].transform.position = GlobalDefine.DisablePos;
				damageDatas[i].active = false;
			}
		}
	}

	public void DrawDamageNumber(int damage, ref Vector3 worldPos)
	{
		for (int i = 0; i < damageDatas.Length; i++)
		{
			if (damageDatas[i].active)
			{
				continue;
			}

            Vector2 pos = GlobalDefine.Vec2Zero;
            var screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPos);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                screenPos, canvas.worldCamera, out pos);

            damageDatas[i].transform.localPosition = pos;
            damageDatas[i].text.text = damage.ToString();
			var state = damageDatas[i].animation.PlayQueued("uianim_battle_damage", QueueMode.PlayNow);
			damageDatas[i].animationState = state;
			damageDatas[i].active = true;
			return;
		}
	}
}
