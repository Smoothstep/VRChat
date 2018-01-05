using System;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

// Token: 0x02000C13 RID: 3091
public class SteamVR_TrackedObject : MonoBehaviour
{
	// Token: 0x06005FB7 RID: 24503 RVA: 0x0021A7A7 File Offset: 0x00218BA7
	private SteamVR_TrackedObject()
	{
		this.newPosesAction = SteamVR_Events.NewPosesAction(new UnityAction<TrackedDevicePose_t[]>(this.OnNewPoses));
	}

	// Token: 0x17000D8F RID: 3471
	// (get) Token: 0x06005FB8 RID: 24504 RVA: 0x0021A7C6 File Offset: 0x00218BC6
	// (set) Token: 0x06005FB9 RID: 24505 RVA: 0x0021A7CE File Offset: 0x00218BCE
	public bool isValid { get; private set; }

	// Token: 0x06005FBA RID: 24506 RVA: 0x0021A7D8 File Offset: 0x00218BD8
	private void OnNewPoses(TrackedDevicePose_t[] poses)
	{
		if (this.index == SteamVR_TrackedObject.EIndex.None)
		{
			return;
		}
		int num = (int)this.index;
		this.isValid = false;
		if (poses.Length <= num)
		{
			return;
		}
		if (!poses[num].bDeviceIsConnected)
		{
			return;
		}
		if (!poses[num].bPoseIsValid)
		{
			return;
		}
		this.isValid = true;
		SteamVR_Utils.RigidTransform rigidTransform = new SteamVR_Utils.RigidTransform(poses[num].mDeviceToAbsoluteTracking);
		if (this.origin != null)
		{
			base.transform.position = this.origin.transform.TransformPoint(rigidTransform.pos);
			base.transform.rotation = this.origin.rotation * rigidTransform.rot;
		}
		else
		{
			base.transform.localPosition = rigidTransform.pos;
			base.transform.localRotation = rigidTransform.rot;
		}
	}

	// Token: 0x06005FBB RID: 24507 RVA: 0x0021A8C8 File Offset: 0x00218CC8
	private void OnEnable()
	{
		SteamVR_Render instance = SteamVR_Render.instance;
		if (instance == null)
		{
			base.enabled = false;
			return;
		}
		this.newPosesAction.enabled = true;
	}

	// Token: 0x06005FBC RID: 24508 RVA: 0x0021A8FB File Offset: 0x00218CFB
	private void OnDisable()
	{
		this.newPosesAction.enabled = false;
		this.isValid = false;
	}

	// Token: 0x06005FBD RID: 24509 RVA: 0x0021A910 File Offset: 0x00218D10
	public void SetDeviceIndex(int index)
	{
		if (Enum.IsDefined(typeof(SteamVR_TrackedObject.EIndex), index))
		{
			this.index = (SteamVR_TrackedObject.EIndex)index;
		}
	}

	// Token: 0x04004563 RID: 17763
	public SteamVR_TrackedObject.EIndex index;

	// Token: 0x04004564 RID: 17764
	[Tooltip("If not set, relative to parent")]
	public Transform origin;

	// Token: 0x04004566 RID: 17766
	private SteamVR_Events.Action newPosesAction;

	// Token: 0x02000C14 RID: 3092
	public enum EIndex
	{
		// Token: 0x04004568 RID: 17768
		None = -1,
		// Token: 0x04004569 RID: 17769
		Hmd,
		// Token: 0x0400456A RID: 17770
		Device1,
		// Token: 0x0400456B RID: 17771
		Device2,
		// Token: 0x0400456C RID: 17772
		Device3,
		// Token: 0x0400456D RID: 17773
		Device4,
		// Token: 0x0400456E RID: 17774
		Device5,
		// Token: 0x0400456F RID: 17775
		Device6,
		// Token: 0x04004570 RID: 17776
		Device7,
		// Token: 0x04004571 RID: 17777
		Device8,
		// Token: 0x04004572 RID: 17778
		Device9,
		// Token: 0x04004573 RID: 17779
		Device10,
		// Token: 0x04004574 RID: 17780
		Device11,
		// Token: 0x04004575 RID: 17781
		Device12,
		// Token: 0x04004576 RID: 17782
		Device13,
		// Token: 0x04004577 RID: 17783
		Device14,
		// Token: 0x04004578 RID: 17784
		Device15
	}
}
