using MsgPack.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class MasterDataUtility
{
	public static byte[] Seialize<T>(T obj)
	{
		MemoryStream stream = new MemoryStream();
		var serializer = MessagePackSerializer.Get<T>();
		serializer.Pack(stream, obj);

		byte[] data = new byte[(int)stream.Length];
		stream.Position = 0;
		stream.Read(data, 0, (int)stream.Length);

		return data;
	}

	public static T Deserialize<T>(ref byte[] bytes)
	{
		var serializer = MessagePackSerializer.Get<T>();
		return serializer.UnpackSingleObject(bytes);
	}

	public static object Deserialize(ref byte[] bytes,System.Type type)
	{
		var serializer = MessagePackSerializer.Get(type);
		return serializer.UnpackSingleObject(bytes);
	}
}
