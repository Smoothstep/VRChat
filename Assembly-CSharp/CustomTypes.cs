using System;
using System.Runtime.CompilerServices;
using ExitGames.Client.Photon;
using UnityEngine;

// Token: 0x02000729 RID: 1833
internal static class CustomTypes
{
	// Token: 0x06003B8E RID: 15246 RVA: 0x0012B9A0 File Offset: 0x00129DA0
	internal static void Register()
	{
		Type typeFromHandle = typeof(Vector2);
		byte code = 87;
		if (CustomTypes.f__mg == null)
		{
			CustomTypes.f__mg = new SerializeStreamMethod(CustomTypes.SerializeVector2);
		}
		SerializeStreamMethod serializeMethod = CustomTypes.f__mg;
		if (CustomTypes.f__mg1 == null)
		{
			CustomTypes.f__mg1 = new DeserializeStreamMethod(CustomTypes.DeserializeVector2);
		}
		PhotonPeer.RegisterType(typeFromHandle, code, serializeMethod, CustomTypes.f__mg1);
		Type typeFromHandle2 = typeof(Vector3);
		byte code2 = 86;
		if (CustomTypes.f__mg2 == null)
		{
			CustomTypes.f__mg2 = new SerializeStreamMethod(CustomTypes.SerializeVector3);
		}
		SerializeStreamMethod serializeMethod2 = CustomTypes.f__mg2;
		if (CustomTypes.f__mg3 == null)
		{
			CustomTypes.f__mg3 = new DeserializeStreamMethod(CustomTypes.DeserializeVector3);
		}
		PhotonPeer.RegisterType(typeFromHandle2, code2, serializeMethod2, CustomTypes.f__mg3);
		Type typeFromHandle3 = typeof(Quaternion);
		byte code3 = 81;
		if (CustomTypes.f__mg4 == null)
		{
			CustomTypes.f__mg4 = new SerializeStreamMethod(CustomTypes.SerializeQuaternion);
		}
		SerializeStreamMethod serializeMethod3 = CustomTypes.f__mg4;
		if (CustomTypes.f__mg5 == null)
		{
			CustomTypes.f__mg5 = new DeserializeStreamMethod(CustomTypes.DeserializeQuaternion);
		}
		PhotonPeer.RegisterType(typeFromHandle3, code3, serializeMethod3, CustomTypes.f__mg5);
		Type typeFromHandle4 = typeof(PhotonPlayer);
		byte code4 = 80;
		if (CustomTypes.f__mg6 == null)
		{
			CustomTypes.f__mg6 = new SerializeStreamMethod(CustomTypes.SerializePhotonPlayer);
		}
		SerializeStreamMethod serializeMethod4 = CustomTypes.f__mg6;
		if (CustomTypes.f__mg7 == null)
		{
			CustomTypes.f__mg7 = new DeserializeStreamMethod(CustomTypes.DeserializePhotonPlayer);
		}
		PhotonPeer.RegisterType(typeFromHandle4, code4, serializeMethod4, CustomTypes.f__mg7);
	}

	// Token: 0x06003B8F RID: 15247 RVA: 0x0012BAE0 File Offset: 0x00129EE0
	private static short SerializeVector3(StreamBuffer outStream, object customobject)
	{
		Vector3 vector = (Vector3)customobject;
		int num = 0;
		object obj = CustomTypes.memVector3;
		lock (obj)
		{
			byte[] array = CustomTypes.memVector3;
			Protocol.Serialize(vector.x, array, ref num);
			Protocol.Serialize(vector.y, array, ref num);
			Protocol.Serialize(vector.z, array, ref num);
			outStream.Write(array, 0, 12);
		}
		return 12;
	}

	// Token: 0x06003B90 RID: 15248 RVA: 0x0012BB60 File Offset: 0x00129F60
	private static object DeserializeVector3(StreamBuffer inStream, short length)
	{
		Vector3 vector = default(Vector3);
		object obj = CustomTypes.memVector3;
		lock (obj)
		{
			inStream.Read(CustomTypes.memVector3, 0, 12);
			int num = 0;
			Protocol.Deserialize(out vector.x, CustomTypes.memVector3, ref num);
			Protocol.Deserialize(out vector.y, CustomTypes.memVector3, ref num);
			Protocol.Deserialize(out vector.z, CustomTypes.memVector3, ref num);
		}
		return vector;
	}

	// Token: 0x06003B91 RID: 15249 RVA: 0x0012BBF0 File Offset: 0x00129FF0
	private static short SerializeVector2(StreamBuffer outStream, object customobject)
	{
		Vector2 vector = (Vector2)customobject;
		object obj = CustomTypes.memVector2;
		lock (obj)
		{
			byte[] array = CustomTypes.memVector2;
			int num = 0;
			Protocol.Serialize(vector.x, array, ref num);
			Protocol.Serialize(vector.y, array, ref num);
			outStream.Write(array, 0, 8);
		}
		return 8;
	}

	// Token: 0x06003B92 RID: 15250 RVA: 0x0012BC5C File Offset: 0x0012A05C
	private static object DeserializeVector2(StreamBuffer inStream, short length)
	{
		Vector2 vector = default(Vector2);
		object obj = CustomTypes.memVector2;
		lock (obj)
		{
			inStream.Read(CustomTypes.memVector2, 0, 8);
			int num = 0;
			Protocol.Deserialize(out vector.x, CustomTypes.memVector2, ref num);
			Protocol.Deserialize(out vector.y, CustomTypes.memVector2, ref num);
		}
		return vector;
	}

	// Token: 0x06003B93 RID: 15251 RVA: 0x0012BCD8 File Offset: 0x0012A0D8
	private static short SerializeQuaternion(StreamBuffer outStream, object customobject)
	{
		Quaternion quaternion = (Quaternion)customobject;
		object obj = CustomTypes.memQuarternion;
		lock (obj)
		{
			byte[] array = CustomTypes.memQuarternion;
			int num = 0;
			Protocol.Serialize(quaternion.w, array, ref num);
			Protocol.Serialize(quaternion.x, array, ref num);
			Protocol.Serialize(quaternion.y, array, ref num);
			Protocol.Serialize(quaternion.z, array, ref num);
			outStream.Write(array, 0, 16);
		}
		return 16;
	}

	// Token: 0x06003B94 RID: 15252 RVA: 0x0012BD64 File Offset: 0x0012A164
	private static object DeserializeQuaternion(StreamBuffer inStream, short length)
	{
		Quaternion quaternion = default(Quaternion);
		object obj = CustomTypes.memQuarternion;
		lock (obj)
		{
			inStream.Read(CustomTypes.memQuarternion, 0, 16);
			int num = 0;
			Protocol.Deserialize(out quaternion.w, CustomTypes.memQuarternion, ref num);
			Protocol.Deserialize(out quaternion.x, CustomTypes.memQuarternion, ref num);
			Protocol.Deserialize(out quaternion.y, CustomTypes.memQuarternion, ref num);
			Protocol.Deserialize(out quaternion.z, CustomTypes.memQuarternion, ref num);
		}
		return quaternion;
	}

	// Token: 0x06003B95 RID: 15253 RVA: 0x0012BE04 File Offset: 0x0012A204
	private static short SerializePhotonPlayer(StreamBuffer outStream, object customobject)
	{
		int id = ((PhotonPlayer)customobject).ID;
		object obj = CustomTypes.memPlayer;
		short result;
		lock (obj)
		{
			byte[] array = CustomTypes.memPlayer;
			int num = 0;
			Protocol.Serialize(id, array, ref num);
			outStream.Write(array, 0, 4);
			result = 4;
		}
		return result;
	}

	// Token: 0x06003B96 RID: 15254 RVA: 0x0012BE64 File Offset: 0x0012A264
	private static object DeserializePhotonPlayer(StreamBuffer inStream, short length)
	{
		object obj = CustomTypes.memPlayer;
		int key;
		lock (obj)
		{
			inStream.Read(CustomTypes.memPlayer, 0, (int)length);
			int num = 0;
			Protocol.Deserialize(out key, CustomTypes.memPlayer, ref num);
		}
		if (PhotonNetwork.networkingPeer.mActors.ContainsKey(key))
		{
			return PhotonNetwork.networkingPeer.mActors[key];
		}
		return null;
	}

	// Token: 0x04002456 RID: 9302
	public static readonly byte[] memVector3 = new byte[12];

	// Token: 0x04002457 RID: 9303
	public static readonly byte[] memVector2 = new byte[8];

	// Token: 0x04002458 RID: 9304
	public static readonly byte[] memQuarternion = new byte[16];

	// Token: 0x04002459 RID: 9305
	public static readonly byte[] memPlayer = new byte[4];

	// Token: 0x0400245A RID: 9306
	[CompilerGenerated]
	private static SerializeStreamMethod f__mg;

	// Token: 0x0400245B RID: 9307
	[CompilerGenerated]
	private static DeserializeStreamMethod f__mg1;

	// Token: 0x0400245C RID: 9308
	[CompilerGenerated]
	private static SerializeStreamMethod f__mg2;

	// Token: 0x0400245D RID: 9309
	[CompilerGenerated]
	private static DeserializeStreamMethod f__mg3;

	// Token: 0x0400245E RID: 9310
	[CompilerGenerated]
	private static SerializeStreamMethod f__mg4;

	// Token: 0x0400245F RID: 9311
	[CompilerGenerated]
	private static DeserializeStreamMethod f__mg5;

	// Token: 0x04002460 RID: 9312
	[CompilerGenerated]
	private static SerializeStreamMethod f__mg6;

	// Token: 0x04002461 RID: 9313
	[CompilerGenerated]
	private static DeserializeStreamMethod f__mg7;
}
