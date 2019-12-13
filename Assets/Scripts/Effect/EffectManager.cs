using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : ManagerBehaviour<EffectManager>
{
	enum SEQ_STATE
	{
		NONE,
		LOAD,
		UPDATE,
	}

	public enum EFFECT_ID
	{
		NONE,
		CHARGE,
		CHARGE_START,
		HIT_ATTACK,
		HIT_CHARGE,
		TREE_DOWN_IMPACT,
		TREE_APPEAR_IMPACT,
		MAX,
	}

	class EffectLoadData
	{
		public EFFECT_ID effectId;
		public AssetLoadHandle assetLoadHandle;
		public GameObject gameObject;
		public bool isLoaded;
	}

	class EffectStopData
	{
		public EFFECT_ID effectId;
		public ParticleSystem effect;
		public float time;
	}

	class EffectPlayData
	{
		public EFFECT_ID effectId;
		public ParticleSystem effect;
		public float time;
		public int index = 0;
	}

	readonly string[] EFFECT_PATH_TABLE = new string[(int)EFFECT_ID.MAX] {
		"",
		"effect/charge",
		"effect/charge_start",
		"effect/hit_attack",
		"effect/hit_charge",
		"effect/tree_down_impact",
		"effect/tree_appear_impact"
	};

	readonly int[] EFFECT_COUNT_TABLE = new int[(int)EFFECT_ID.MAX] {
		0,
		5,
		5,
		5,
		5,
		2,
		2,
	};

	const int ERROR_HANDLE = -1;

	Transform rootTrans = null;

	SeqStateUtility<SEQ_STATE> seqState = new SeqStateUtility<SEQ_STATE>(SEQ_STATE.NONE);
	Dictionary<int, ParticleSystem[]> effectTable = new Dictionary<int, ParticleSystem[]>();
	List<EffectPlayData> playEffects = new List<EffectPlayData>();
	List<EffectStopData> stopEffects = new List<EffectStopData>();

	EffectLoadData[] effectLoadDatas = new EffectLoadData[(int)EFFECT_ID.MAX];

	public override MANAGER_TYPE GetManagerType()
	{
		return MANAGER_TYPE.EFFECT;
	}

	private void Start()
	{
		var rootObj = new GameObject("EffectRoot");
		rootTrans = rootObj.transform;
	}

	private void Update()
	{
		switch (seqState.GetState())
		{
			case SEQ_STATE.LOAD:
				UpdateLoad();
				break;
			case SEQ_STATE.UPDATE:
				UpdateExecute();
				break;
		}
	}

	void UpdateLoad()
	{
		if (!seqState.IsInit())
		{
			for (int i = 0; i < EFFECT_PATH_TABLE.Length; i++)
			{
				var path = EFFECT_PATH_TABLE[i];
				var handle = AssetLoadManager.Load<GameObject>(ref path);

				effectLoadDatas[i] = new EffectLoadData()
				{
					assetLoadHandle = handle,
					effectId = (EFFECT_ID)i,
				};
			}

			seqState.SetInit(true);
		}

		bool end = true;
		for (int i = 0; i < effectLoadDatas.Length; i++)
		{
			var data = effectLoadDatas[i];
            if(data.isLoaded)
            {
                continue;
            }

			if (data.assetLoadHandle.Result == ASSET_LOAD_RESULT_TYPE.SUCCESS)
			{
                data.gameObject = data.assetLoadHandle.LoadObject as GameObject;
                data.isLoaded = true;

                int id = (int)data.effectId;
                int count = EFFECT_COUNT_TABLE[id];
				ParticleSystem[] particles = new ParticleSystem[count];
				for (int k = 0; k < particles.Length; k++)
				{
					GameObject instance = GameObject.Instantiate(data.gameObject, rootTrans);
					instance.name = data.gameObject.name;
					instance.SetActive(false);
                    instance.layer = LayerDefine.Effect;
                    particles[k] = instance.GetComponent<ParticleSystem>();
				}

				effectTable.Add(id, particles);
			}
			else if (data.assetLoadHandle.Result == ASSET_LOAD_RESULT_TYPE.FAILURE)
			{
				int id = (int)data.effectId;
                data.isLoaded = true;
                effectTable.Add(id, null);
			}
			else
			{
				end = false;
			}
		}

		if (end)
		{
			seqState.ChangeState(SEQ_STATE.UPDATE);
		}
	}

	void UpdateExecute()
	{
		if (!seqState.IsInit())
		{

			seqState.SetInit(true);
		}

		for (int i = playEffects.Count - 1; i >= 0; i--)
		{
			var effect = playEffects[i];
			if (effect.effect.main.loop)
			{
				continue;
			}

			effect.time += GlobalDefine.DeltaTime;
			if (effect.effect.main.duration <= effect.time)
			{
				StopEffect(effect.effectId, ref effect.index);
			}
		}

		for (int i = stopEffects.Count - 1; i >= 0; i--)
		{
			var effect = stopEffects[i];
			effect.time += GlobalDefine.DeltaTime;
			if (effect.effect.main.duration <= effect.time)
			{
				effect.effect.gameObject.SetActive(false);
				stopEffects.RemoveAt(i);
			}
		}
	}

	public static void StartLoad()
	{
		if (IsInstance)
		{
			Instance.seqState.ChangeState(SEQ_STATE.LOAD);
		}
	}

	public static bool IsLoaded()
	{
		if (IsInstance)
		{
			return Instance.seqState.GetState() == SEQ_STATE.UPDATE;
		}

		return false;
	}

	public static int PlayEffect(EFFECT_ID id, ref Vector3 pos)
	{
		if (!IsLoaded())
		{
			return ERROR_HANDLE;
		}

		if (IsInstance)
		{
			var effects = Instance.effectTable[(int)id];
			for (int i = 0; i < effects.Length; i++)
			{
				var effect = effects[i];
				if (effect.gameObject.activeInHierarchy)
				{
					continue;
				}

				Instance.playEffects.Add(new EffectPlayData() {
					effect = effect,
					index = i,
					effectId = id,
					time = 0.0f
				});

				effect.gameObject.SetActive(true);
				effect.transform.localPosition = pos;
				effect.Play();
				return i;
			}
		}

		return ERROR_HANDLE;
	}

	public static void StopEffect(EFFECT_ID id,ref int index, bool force = false)
	{
		if (!IsLoaded())
		{
			return;
		}

		if (IsInstance)
		{
			var effects = Instance.effectTable[(int)id];
			if (index >= 0)
			{
				var effect = effects[index];
				if (!effect.gameObject.activeInHierarchy)
				{
					index = ERROR_HANDLE;
					return;
				}

				effect.Stop();

				for (int i = 0; i < Instance.playEffects.Count; i++)
				{
					var playData = Instance.playEffects[i];
					if (playData.effectId == id && playData.index == index)
					{
						Instance.playEffects.RemoveAt(i);
						break;
					}
				}

				if (force)
				{
					effect.gameObject.SetActive(false);
				}
				else
				{
					Instance.stopEffects.Add(new EffectStopData()
					{
						effect = effect,
						effectId = id,
						time = 0.0f
					});
				}
			}

			index = ERROR_HANDLE;
		}
	}

	public static void StopEffect(EFFECT_ID id, bool force = false)
	{
		if (!IsLoaded())
		{
			return;
		}

		if (IsInstance)
		{
			var effects = Instance.effectTable[(int)id];
			for (int i = 0; i < effects.Length; i++)
			{
				var effect = effects[i];
				effect.Stop();

				for (int k = 0; k < Instance.playEffects.Count; k++)
				{
					var playData = Instance.playEffects[k];
					if (playData.effectId == id && playData.index == i)
					{
						Instance.playEffects.RemoveAt(k);
						break;
					}
				}

				if (force)
				{
					effect.gameObject.SetActive(false);
				}
				else
				{
					Instance.stopEffects.Add(new EffectStopData()
					{
						effect = effect,
						effectId = id,
						time = 0.0f
					});
				}
			}
		}
	}
}
