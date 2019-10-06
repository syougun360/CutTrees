using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameParam
{
    static TreeParam treeParam = new TreeParam();
    static PlayerParam playerParam = new PlayerParam();
    static Dictionary<int, WeaponParam> weaponParamDic = new Dictionary<int, WeaponParam>();

    public static void Setup()
    {
        var weaponMasterData = MasterDataManager.GetMasterData<Weapon.WeaponInfoMasterData>(MasterDataManager.MASTER_DATE_ID.WEAPON);
        for (int i = 1; i < weaponMasterData.datas.Length; i++)
        {
            var data = weaponMasterData.datas[i];
            var param = new WeaponParam();
            param.id = data.id;
            param.hp = data.hp;
            weaponParamDic.Add(data.id, param);
        }
    }

    public static void SetEquipWeaponID(Weapon.WEAPON_ID id)
    {
        playerParam.equipWeaponId = (int)id;
    }

    public static void SetTreeDestroyCount(int count)
    {
        treeParam.destroyCount = count;
    }

    public static void SetTreeHp(int hp)
    {
        treeParam.hp = hp;
    }

    public static PlayerParam GetPlayerParam()
    {
        return playerParam;
    }

    public static TreeParam GetTreeParam()
    {
        return treeParam;
    }

    public static WeaponParam GetWeaponParam(Weapon.WEAPON_ID id)
    {
        int intId = (int)id;
        if (weaponParamDic.ContainsKey(intId))
        {
            return weaponParamDic[intId];
        }

        return null;
    }

    public static List<WeaponParam> GetWeaponParamList()
    {
        return weaponParamDic.Values.ToList();
    }
}
