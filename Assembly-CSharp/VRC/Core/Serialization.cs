using System;
using UnityEngine;

namespace VRC.Core
{
	// Token: 0x02000A66 RID: 2662
	public static class Serialization
	{
		// Token: 0x06005091 RID: 20625 RVA: 0x001B8864 File Offset: 0x001B6C64
		public static void SerializeQuaternionAsShorts(PhotonStream stream, Quaternion quat)
		{
			float num = 100f;
			stream.SendNext((short)Mathf.FloatToHalf(quat.x * num));
			stream.SendNext((short)Mathf.FloatToHalf(quat.y * num));
			stream.SendNext((short)Mathf.FloatToHalf(quat.z * num));
			stream.SendNext((short)Mathf.FloatToHalf(quat.w * num));
		}

		// Token: 0x06005092 RID: 20626 RVA: 0x001B88E0 File Offset: 0x001B6CE0
		public static void SerializeVectorAsShorts(PhotonStream stream, Vector3 vect, bool small)
		{
			float num = (!small) ? 1f : 100f;
			stream.SendNext((short)Mathf.FloatToHalf(vect.x * num));
			stream.SendNext((short)Mathf.FloatToHalf(vect.y * num));
			stream.SendNext((short)Mathf.FloatToHalf(vect.z * num));
		}

		// Token: 0x06005093 RID: 20627 RVA: 0x001B8954 File Offset: 0x001B6D54
		public static void SerializeQuaternion(PhotonStream stream, Quaternion quat)
		{
			stream.SendNext(quat.x);
			stream.SendNext(quat.y);
			stream.SendNext(quat.z);
			stream.SendNext(quat.w);
		}

		// Token: 0x06005094 RID: 20628 RVA: 0x001B89A9 File Offset: 0x001B6DA9
		public static void SerializeVector(PhotonStream stream, Vector3 vect)
		{
			stream.SendNext(vect.x);
			stream.SendNext(vect.y);
			stream.SendNext(vect.z);
		}

		// Token: 0x06005095 RID: 20629 RVA: 0x001B89E4 File Offset: 0x001B6DE4
		public static Quaternion DeserializeQuaternionFromShorts(PhotonStream stream)
		{
			float num = 0.01f;
			short num2 = (short)stream.ReceiveNext();
			short num3 = (short)stream.ReceiveNext();
			short num4 = (short)stream.ReceiveNext();
			short num5 = (short)stream.ReceiveNext();
			return new Quaternion(Mathf.HalfToFloat((ushort)num2) * num, Mathf.HalfToFloat((ushort)num3) * num, Mathf.HalfToFloat((ushort)num4) * num, Mathf.HalfToFloat((ushort)num5) * num);
		}

		// Token: 0x06005096 RID: 20630 RVA: 0x001B8A54 File Offset: 0x001B6E54
		public static Vector3 DeserializeVectorFromShorts(PhotonStream stream, bool small)
		{
			float num = (!small) ? 1f : 0.01f;
			short num2 = (short)stream.ReceiveNext();
			short num3 = (short)stream.ReceiveNext();
			short num4 = (short)stream.ReceiveNext();
			return new Vector3(Mathf.HalfToFloat((ushort)num2) * num, Mathf.HalfToFloat((ushort)num3) * num, Mathf.HalfToFloat((ushort)num4) * num);
		}

		// Token: 0x06005097 RID: 20631 RVA: 0x001B8ABC File Offset: 0x001B6EBC
		public static Quaternion DeserializeQuaternion(PhotonStream stream)
		{
			Quaternion result;
			result.x = (float)stream.ReceiveNext();
			result.y = (float)stream.ReceiveNext();
			result.z = (float)stream.ReceiveNext();
			result.w = (float)stream.ReceiveNext();
			return result;
		}

		// Token: 0x06005098 RID: 20632 RVA: 0x001B8B14 File Offset: 0x001B6F14
		public static Vector3 DeserializeVector(PhotonStream stream)
		{
			float x = (float)stream.ReceiveNext();
			float y = (float)stream.ReceiveNext();
			float z = (float)stream.ReceiveNext();
			return new Vector3(x, y, z);
		}
	}
}
