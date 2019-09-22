/// <summary>
/// タグ名を定数で管理するクラス
/// </summary>
public static class TagDefine
{
	public const int Default = 0;
	public const int TransparentFX = 1;
	public const int IgnoreRaycast = 2;
	public const int Water = 4;
	public const int UI = 5;
	public const int Player = 8;
	public const int Enemy = 9;
	public const int Map = 10;
	public const int Effect = 11;
	public const int Weapon = 12;
	public const int Camera = 13;

	public const int DefaultMask = 1;
	public const int TransparentFXMask = 2;
	public const int IgnoreRaycastMask = 4;
	public const int WaterMask = 16;
	public const int UIMask = 32;
	public const int PlayerMask = 256;
	public const int EnemyMask = 512;
	public const int MapMask = 1024;
	public const int EffectMask = 2048;
	public const int WeaponMask = 4096;
	public const int CameraMask = 8192;
}
