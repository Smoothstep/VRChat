using System;
using UnityEngine;

// Token: 0x02000A82 RID: 2690
public class FaceCamera : MonoBehaviour
{
	// Token: 0x0600510A RID: 20746 RVA: 0x001BAA17 File Offset: 0x001B8E17
	private void Awake()
	{
		this.lookAt = VRCTrackingManager.GetTrackedTransform(VRCTracking.ID.Hmd);
	}

	// Token: 0x0600510B RID: 20747 RVA: 0x001BAA25 File Offset: 0x001B8E25
	private void Update()
	{
		this.ForceUpdate();
	}

	// Token: 0x0600510C RID: 20748 RVA: 0x001BAA2D File Offset: 0x001B8E2D
	public void ForceUpdate()
	{
		if (!this.RotateZOnly)
		{
			base.transform.LookAt(this.lookAt, Vector3.up);
		}
	}

	// Token: 0x0400396A RID: 14698
	public bool RotateZOnly;

	// Token: 0x0400396B RID: 14699
	private Transform lookAt;
}
