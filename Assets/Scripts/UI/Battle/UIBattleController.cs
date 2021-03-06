﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBattleController : MonoBehaviour
{
	static UIBattleDamage damageUI = null;
	static UIBattleTreeStatus treeStatusUI = null;
	static UIBattleChangeWeapon changeWeaponUI = null;

	private void Awake()
	{
		damageUI = GetComponentInChildren<UIBattleDamage>();
		treeStatusUI = GetComponentInChildren<UIBattleTreeStatus>();
		changeWeaponUI = GetComponentInChildren<UIBattleChangeWeapon>();
	}

	public static UIBattleDamage GetDamageUI()
	{
		return damageUI;
	}

	public static UIBattleTreeStatus GetTreeStatusUI()
	{
		return treeStatusUI;
	}

	public static UIBattleChangeWeapon GetChangeWeaponUI()
	{
		return changeWeaponUI;
	}
}
