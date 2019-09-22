using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeqStateUtility<T>
{
	int step = 0;
	bool init = false;

	T seqState;

	public SeqStateUtility(T state)
	{
		this.seqState = state;
	}

	public void ChangeState(T state)
	{
		this.seqState = state;
		this.init = false;
		this.step = 0;
	}

	public T GetState()
	{
		return this.seqState;
	}

	public void SetInit(bool init)
	{
		this.init = init;
	}

	public bool IsInit()
	{
		return this.init;
	}

	public void NextStep()
	{
		this.step++;
	}

	public int GetStep()
	{
		return this.step;
	}
}