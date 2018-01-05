using System;
using UnityEngine;
using VRCSDK2;

// Token: 0x02000AA3 RID: 2723
public class InternalSDKNpc : MonoBehaviour
{
	// Token: 0x060051D0 RID: 20944 RVA: 0x001C078C File Offset: 0x001BEB8C
	private void Awake()
	{
	}

	// Token: 0x060051D1 RID: 20945 RVA: 0x001C078E File Offset: 0x001BEB8E
	private void OnDestroy()
	{
	}

	// Token: 0x060051D2 RID: 20946 RVA: 0x001C0790 File Offset: 0x001BEB90
	public static VRC_NpcApi GetApiByGameObject(GameObject npcGameObject)
	{
		return npcGameObject.GetComponent<VRC_NpcApi>();
	}

	// Token: 0x060051D3 RID: 20947 RVA: 0x001C0798 File Offset: 0x001BEB98
	public static void Initialize(VRC_NpcApi api)
	{
		VRC_NpcInternal component = api.GetComponent<VRC_NpcInternal>();
		component.InitializeApi(api);
	}

	// Token: 0x060051D4 RID: 20948 RVA: 0x001C07B4 File Offset: 0x001BEBB4
	public static void ActThis(VRC_NpcApi api, int number, bool loop)
	{
		VRC_NpcInternal component = api.GetComponent<VRC_NpcInternal>();
		component.ActThis(number, loop);
	}

	// Token: 0x060051D5 RID: 20949 RVA: 0x001C07D0 File Offset: 0x001BEBD0
	public static void SayThis(VRC_NpcApi api, AudioClip clip, float volume)
	{
		VRC_NpcInternal component = api.GetComponent<VRC_NpcInternal>();
		component.SayThis(clip, volume);
	}

	// Token: 0x060051D6 RID: 20950 RVA: 0x001C07EC File Offset: 0x001BEBEC
	public static void SetNamePlate(VRC_NpcApi api, bool visible, string nameTag, string vipTag)
	{
		VRC_NpcInternal component = api.GetComponent<VRC_NpcInternal>();
		component.SetNamePlate(visible, nameTag, vipTag);
	}

	// Token: 0x060051D7 RID: 20951 RVA: 0x001C080C File Offset: 0x001BEC0C
	public static void SetSocialStatus(VRC_NpcApi api, bool friend, bool vip, bool blocked)
	{
		VRC_NpcInternal component = api.GetComponent<VRC_NpcInternal>();
		component.SetSocialStatus(friend, vip, blocked);
	}

	// Token: 0x060051D8 RID: 20952 RVA: 0x001C082C File Offset: 0x001BEC2C
	public static void SetMuteStatus(VRC_NpcApi api, bool canSpeak, bool canHear)
	{
		VRC_NpcInternal component = api.GetComponent<VRC_NpcInternal>();
		component.SetMuteStatus(canSpeak, canHear);
	}
}
