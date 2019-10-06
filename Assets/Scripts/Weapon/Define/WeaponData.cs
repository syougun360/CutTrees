// -------------------------------------------------
// 武器のデータベース
// 作成時間：Wed Sep 25 2019 00:35:32 GMT+0900 (JST)
// -------------------------------------------------
namespace Weapon
{
	public enum WEAPON_ID
	{
		NULL,
		SWORD_01,
		SWORD_02,
		SWORD_03,
		HUMMER_01,
		STAFF_01,
		AXE_01,
		AXE_02,
		CLUB_01,
		MAX,
	}
	
	[System.Serializable]
	public class WeaponInfoMasterData
	{
		public int version;
		public WeaponInfo[] datas;
	}
	
	[System.Serializable]
	public class WeaponInfo
	{
		public int id;
		public string label;
		public long labelHash;
		public string name;
		public string res_name;
		public int attack_power;
		public int charge_attack_power;
		public int hp;
	}
}
