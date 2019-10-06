// -------------------------------------------------
// 武器のデータベース
// 作成時間：Sun Oct 06 2019 20:37:03 GMT+0900 (JST)
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
		public string icon_parh;
		public int level;
		public int equip_level;
		public int attack_power;
		public int charge_attack_power;
		public int hp;
	}
}
