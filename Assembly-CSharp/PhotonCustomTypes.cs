using System;
using System.Runtime.CompilerServices;
using System.Text;
using ExitGames.Client.Photon;
using UnityEngine;
using VRC.Core;

// Token: 0x02000ABD RID: 2749
internal static class PhotonCustomTypes
{
	// Token: 0x06005379 RID: 21369 RVA: 0x001CCB54 File Offset: 0x001CAF54
	internal static void Register()
	{
		Debug.Log("Registering Custom Photon Objets");
		Type typeFromHandle = typeof(ApiAvatar);
		byte code = 66;
		if (PhotonCustomTypes.f__mg0 == null)
		{
			PhotonCustomTypes.f__mg0 = new SerializeMethod(PhotonCustomTypes.SerializeApiAvatar);
		}
		SerializeMethod serializeMethod = PhotonCustomTypes.f__mg0;
		if (PhotonCustomTypes.f__mg1 == null)
		{
			PhotonCustomTypes.f__mg1 = new DeserializeMethod(PhotonCustomTypes.DeserializeApiAvatar);
		}
		PhotonPeer.RegisterType(typeFromHandle, code, serializeMethod, PhotonCustomTypes.f__mg1);
		Type typeFromHandle2 = typeof(ApiWorld);
		byte code2 = 87;
		if (PhotonCustomTypes.f__mg2 == null)
		{
			PhotonCustomTypes.f__mg2 = new SerializeMethod(PhotonCustomTypes.SerializeApiWorld);
		}
		SerializeMethod serializeMethod2 = PhotonCustomTypes.f__mg2;
		if (PhotonCustomTypes.f__mg3 == null)
		{
			PhotonCustomTypes.f__mg3 = new DeserializeMethod(PhotonCustomTypes.DeserializeApiWorld);
		}
		PhotonPeer.RegisterType(typeFromHandle2, code2, serializeMethod2, PhotonCustomTypes.f__mg3);
	}

	// Token: 0x0600537A RID: 21370 RVA: 0x001CCC04 File Offset: 0x001CB004
	private static byte[] SerializeApiAvatar(object customobject)
	{
		int num = 0;
		ApiAvatar apiAvatar = (ApiAvatar)customobject;
		byte[] bytes = Encoding.UTF8.GetBytes(apiAvatar.id);
		byte[] bytes2 = Encoding.UTF8.GetBytes(apiAvatar.assetUrl);
		byte[] array = new byte[bytes.Length + bytes2.Length + 2 + 256];
		Protocol.Serialize((short)bytes.Length, array, ref num);
		bytes.CopyTo(array, num);
		num += bytes.Length;
		Protocol.Serialize((short)bytes2.Length, array, ref num);
		bytes2.CopyTo(array, num);
		num += bytes2.Length;
		Protocol.Serialize((short)apiAvatar.version, array, ref num);
		return array;
	}

	// Token: 0x0600537B RID: 21371 RVA: 0x001CCC9C File Offset: 0x001CB09C
	private static object DeserializeApiAvatar(byte[] bytes)
	{
		ApiAvatar apiAvatar = new ApiAvatar();
		apiAvatar.Init();
		int num = 0;
		short num2 = 0;
		Protocol.Deserialize(out num2, bytes, ref num);
		apiAvatar.id = Encoding.UTF8.GetString(bytes, num, (int)num2);
		num += (int)num2;
		Protocol.Deserialize(out num2, bytes, ref num);
		apiAvatar.assetUrl = Encoding.UTF8.GetString(bytes, num, (int)num2);
		num += (int)num2;
		int version = 0;
		Protocol.Deserialize(out version, bytes, ref num);
		apiAvatar.version = version;
		return apiAvatar;
	}

	// Token: 0x0600537C RID: 21372 RVA: 0x001CCD10 File Offset: 0x001CB110
	private static byte[] SerializeApiWorld(object customobject)
	{
		int num = 0;
		ApiWorld apiWorld = (ApiWorld)customobject;
		byte[] bytes = Encoding.UTF8.GetBytes(apiWorld.id);
		byte[] bytes2 = Encoding.UTF8.GetBytes(apiWorld.assetUrl);
		string s = (!string.IsNullOrEmpty(apiWorld.pluginUrl)) ? apiWorld.pluginUrl : "-";
		byte[] bytes3 = Encoding.UTF8.GetBytes(s);
		byte[] array = new byte[bytes.Length + bytes2.Length + bytes3.Length + 2 + 256];
		Protocol.Serialize((short)bytes.Length, array, ref num);
		bytes.CopyTo(array, num);
		num += bytes.Length;
		Protocol.Serialize((short)bytes2.Length, array, ref num);
		bytes2.CopyTo(array, num);
		num += bytes2.Length;
		Protocol.Serialize((short)bytes3.Length, array, ref num);
		bytes3.CopyTo(array, num);
		num += bytes3.Length;
		Protocol.Serialize((short)apiWorld.version, array, ref num);
		return array;
	}

	// Token: 0x0600537D RID: 21373 RVA: 0x001CCDFC File Offset: 0x001CB1FC
	private static object DeserializeApiWorld(byte[] bytes)
	{
		ApiWorld apiWorld = new ApiWorld();
		apiWorld.Init();
		int num = 0;
		short num2 = 0;
		Protocol.Deserialize(out num2, bytes, ref num);
		apiWorld.id = Encoding.UTF8.GetString(bytes, num, (int)num2);
		num += (int)num2;
		Protocol.Deserialize(out num2, bytes, ref num);
		apiWorld.assetUrl = Encoding.UTF8.GetString(bytes, num, (int)num2);
		num += (int)num2;
		Protocol.Deserialize(out num2, bytes, ref num);
		apiWorld.pluginUrl = Encoding.UTF8.GetString(bytes, num, (int)num2);
		num += (int)num2;
		if (apiWorld.pluginUrl == "-")
		{
			apiWorld.pluginUrl = null;
		}
		int version = 0;
		Protocol.Deserialize(out version, bytes, ref num);
		apiWorld.version = version;
		return apiWorld;
	}

	// Token: 0x04003AEE RID: 15086
	[CompilerGenerated]
	private static SerializeMethod f__mg0;

	// Token: 0x04003AEF RID: 15087
	[CompilerGenerated]
	private static DeserializeMethod f__mg1;

	// Token: 0x04003AF0 RID: 15088
	[CompilerGenerated]
	private static SerializeMethod f__mg2;

	// Token: 0x04003AF1 RID: 15089
	[CompilerGenerated]
	private static DeserializeMethod f__mg3;
}
