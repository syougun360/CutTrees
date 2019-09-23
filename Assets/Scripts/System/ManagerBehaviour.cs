using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 全てのマネージャーに継承してください。
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class ManagerBehaviour<T> : MonoBehaviour where T : class
{
	static public T Instance { get; private set; }
	static public bool IsInstance { get; private set; }

	protected virtual void Awake()
	{
		if (IsInstance)
		{
			Destroy(this);
			return;
		}

		Instance = this as T;
		IsInstance = Instance != null;
	}

	public abstract MANAGER_TYPE GetManagerType();

	public virtual void StartBattle() { }
}
