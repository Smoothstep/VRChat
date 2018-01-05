using System;
using UnityEngine;

// Token: 0x02000A3C RID: 2620
public class IKHeadAlignment : MonoBehaviour
{
	// Token: 0x06004ED9 RID: 20185 RVA: 0x001A89F8 File Offset: 0x001A6DF8
	public void Initialize(Animator anim)
	{
		if (anim != null && anim.isHuman)
		{
			Transform boneTransform = anim.GetBoneTransform(HumanBodyBones.Head);
			Transform x = boneTransform.Find("HmdPivot");
			if (x != null)
			{
				base.transform.position = boneTransform.position;
				base.transform.rotation = boneTransform.rotation;
			}
		}
		else
		{
			base.transform.localPosition = Vector3.zero;
			base.transform.localRotation = Quaternion.identity;
		}
	}
}
