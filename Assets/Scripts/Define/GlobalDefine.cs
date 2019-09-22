using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// グローバル的な定義
/// </summary>
public class GlobalDefine : MonoBehaviour
{
	public static readonly Vector2 DisablePos = Vector2.one * 100000.0f;

	public static readonly Vector2 Vec2Zero = Vector2.zero; 
	public static readonly Vector2 Vec2up = Vector2.up; 
	public static readonly Vector2 Vec2Down = Vector2.down; 
	public static readonly Vector2 Vec2One = Vector2.one;
	public static readonly Vector2 Vec2Right = Vector2.right;
	public static readonly Vector2 Vec2Left = Vector2.left;

	public static readonly Vector3 Vec3Zero = Vector3.zero;
	public static readonly Vector3 Vec3up = Vector3.up;
	public static readonly Vector3 Vec3Down = Vector3.down;
	public static readonly Vector3 Vec3One = Vector3.one;
	public static readonly Vector3 Vec3Right = Vector3.right;
	public static readonly Vector3 Vec3Left = Vector3.left;
	public static readonly Vector3 Vec3Forward = Vector3.forward;
	public static readonly Vector3 Vec3Back = Vector3.back;

	public static float DeltaTime { get; private set; }
	public static float GameTime { get; private set; }
	public static float UnscaledDeltaTime { get; private set; }

	private void Update()
	{
		DeltaTime = Time.deltaTime;
		GameTime = Time.time;
		UnscaledDeltaTime = Time.unscaledDeltaTime;
	}
}
