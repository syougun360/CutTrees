﻿/// <summary>
/// レイヤー名を定数で管理するクラス
/// </summary>
public static class LayerDefine
{
	public const int Default = 0;
	public const int TransparentFX = 1;
	public const int IgnoreRaycast = 2;
	public const int Water = 4;
	public const int UI = 5;
	public const int Player = 9;
	public const int Tree = 10;
	public const int Effect = 11;
	public const int Map = 12;
	public const int Object = 13;
	
	public const int DefaultMask = 1;
	public const int TransparentFXMask = 2;
	public const int IgnoreRaycastMask = 4;
	public const int WaterMask = 16;
	public const int UIMask = 32;
	public const int PlayerMask = 512;
	public const int TreeMask = 1024;
	public const int EffectMask = 2048;
	public const int MapMask = 4096;
	public const int ObjectMask = 8192;
}
