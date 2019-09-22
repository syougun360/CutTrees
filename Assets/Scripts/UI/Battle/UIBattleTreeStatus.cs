using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleTreeStatus : MonoBehaviour
{
	[SerializeField]
	Image hpBar;

	[SerializeField]
	TextMeshProUGUI hpValue;

	[SerializeField]
	TextMeshProUGUI waveValue;

	[SerializeField]
	AnimationCurve hpAnimation;

	float prevHpRatio = 1.0f;
	float newHpRatio = 1.0f;
	float hpAnimationTime = 0.0f;
	float startChangeHpTime = 0.0f;

	private void Update()
	{
		float time = (GlobalDefine.GameTime - startChangeHpTime) / 0.3f;
		float animRatio = hpAnimation.Evaluate(time); 
		hpBar.fillAmount = Mathf.Lerp(prevHpRatio, newHpRatio, animRatio);
		if (time >= 1.0f)
		{
			prevHpRatio = newHpRatio;
		}
	}

	public void SetTreeHpValue(int hp, float ratio, bool force = false)
	{
		if (force)
		{
			hpBar.fillAmount = ratio;
			prevHpRatio = ratio;
		}

		startChangeHpTime = GlobalDefine.GameTime;
		hpAnimationTime = 0.0f;
		newHpRatio = ratio;
		hpValue.text = hp.ToString();
	}

	public void SetTreeWave(int wave)
	{
		waveValue.text = wave.ToString();
	}
}
