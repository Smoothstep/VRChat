using System;
using UnityEngine;

// Token: 0x02000B1E RID: 2846
public class VRCTracking : MonoBehaviour
{
	// Token: 0x17000C96 RID: 3222
	// (get) Token: 0x0600567B RID: 22139 RVA: 0x001DC930 File Offset: 0x001DAD30
	// (set) Token: 0x0600567C RID: 22140 RVA: 0x001DC933 File Offset: 0x001DAD33
	public virtual bool SeatedPlay
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	// Token: 0x0600567D RID: 22141 RVA: 0x001DC935 File Offset: 0x001DAD35
	public virtual bool IsInVRMode()
	{
		return false;
	}

	// Token: 0x0600567E RID: 22142 RVA: 0x001DC938 File Offset: 0x001DAD38
	public virtual void ResetHMDOrientation()
	{
	}

	// Token: 0x0600567F RID: 22143 RVA: 0x001DC93A File Offset: 0x001DAD3A
	public virtual void ToggleSeatedPlay(bool recalibrate = true)
	{
	}

	// Token: 0x06005680 RID: 22144 RVA: 0x001DC93C File Offset: 0x001DAD3C
	public virtual float GetPlayerHeight()
	{
		return VRCTracking.DefaultPlayerHeight;
	}

	// Token: 0x06005681 RID: 22145 RVA: 0x001DC943 File Offset: 0x001DAD43
	public virtual float GetEyeHeight()
	{
		return VRCTracking.DefaultEyeHeight;
	}

	// Token: 0x06005682 RID: 22146 RVA: 0x001DC94A File Offset: 0x001DAD4A
	public virtual float GetArmLength()
	{
		return VRCTracking.DefaultArmLength;
	}

	// Token: 0x06005683 RID: 22147 RVA: 0x001DC954 File Offset: 0x001DAD54
	public virtual Quaternion GetLocalTrackingRotation()
	{
		Quaternion localCameraRot = VRCVrCamera.GetInstance().GetLocalCameraRot();
		Vector3 eulerAngles = localCameraRot.eulerAngles;
		Vector3 vector = localCameraRot * Vector3.forward;
		Vector3 vector2 = localCameraRot * Vector3.up;
		VRCTracking.HeadLevel headLevel = VRCTracking.HeadLevel.Level;
		if (Mathf.Abs(vector.y) > 0.5f)
		{
			Quaternion quaternion = Quaternion.identity;
			if (vector.y < 0f)
			{
				headLevel = VRCTracking.HeadLevel.Down;
				quaternion = Quaternion.LookRotation(vector2);
			}
			else
			{
				headLevel = VRCTracking.HeadLevel.Up;
				quaternion = Quaternion.LookRotation(-vector2);
			}
			eulerAngles = quaternion.eulerAngles;
		}
		Quaternion quaternion2 = Quaternion.Euler(0f, eulerAngles.y, 0f);
		if (this.lastHeadLevel != headLevel)
		{
			this.doHeadRotSlerp = true;
			this.lastHeadLevel = headLevel;
		}
		if (this.doHeadRotSlerp)
		{
			Quaternion quaternion3 = Quaternion.Slerp(this.lastHeadRot, quaternion2, Time.deltaTime * 2f);
			if (Quaternion.Angle(quaternion2, quaternion3) < 1f)
			{
				this.doHeadRotSlerp = false;
			}
			quaternion2 = quaternion3;
		}
		this.lastHeadRot = quaternion2;
		return quaternion2;
	}

	// Token: 0x06005684 RID: 22148 RVA: 0x001DCA66 File Offset: 0x001DAE66
	public virtual Transform GetTrackedTransform(VRCTracking.ID id)
	{
		return null;
	}

	// Token: 0x06005685 RID: 22149 RVA: 0x001DCA69 File Offset: 0x001DAE69
	public virtual bool IsTracked(VRCTracking.ID id)
	{
		return false;
	}

	// Token: 0x06005686 RID: 22150 RVA: 0x001DCA6C File Offset: 0x001DAE6C
	public virtual bool CanSupportHipTracking()
	{
		return false;
	}

	// Token: 0x06005687 RID: 22151 RVA: 0x001DCA6F File Offset: 0x001DAE6F
	public virtual bool CanSupportHipAndFeetTracking()
	{
		return false;
	}

	// Token: 0x17000C97 RID: 3223
	// (get) Token: 0x06005688 RID: 22152 RVA: 0x001DCA72 File Offset: 0x001DAE72
	public virtual bool calibrated
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06005689 RID: 22153 RVA: 0x001DCA75 File Offset: 0x001DAE75
	public virtual bool IsCalibratedForAvatar(string avatarId)
	{
		return false;
	}

	// Token: 0x0600568A RID: 22154 RVA: 0x001DCA78 File Offset: 0x001DAE78
	public virtual void PerformCalibration(Animator avatarAnim, bool includeHip = false, bool includeFeet = false)
	{
	}

	// Token: 0x0600568B RID: 22155 RVA: 0x001DCA7A File Offset: 0x001DAE7A
	public virtual void GenerateHapticEvent(VRCTracking.ID id, float duration, float amplitude, float frequency)
	{
	}

	// Token: 0x0600568C RID: 22156 RVA: 0x001DCA7C File Offset: 0x001DAE7C
	public virtual void SetControllerVisibility(bool flag)
	{
	}

	// Token: 0x0600568D RID: 22157 RVA: 0x001DCA7E File Offset: 0x001DAE7E
	public virtual ControllerUI GetControllerUI(ControllerHand hand)
	{
		return null;
	}

	// Token: 0x04003D68 RID: 15720
	public static float DefaultPlayerHeight = 1.675f;

	// Token: 0x04003D69 RID: 15721
	public static float DefaultEyeHeight = 1.573f;

	// Token: 0x04003D6A RID: 15722
	public static float DefaultArmLength = 0.76f;

	// Token: 0x04003D6B RID: 15723
	public static float DefaultSeatedEye = 1.22f;

	// Token: 0x04003D6C RID: 15724
	private VRCTracking.HeadLevel lastHeadLevel;

	// Token: 0x04003D6D RID: 15725
	private Quaternion lastHeadRot = Quaternion.identity;

	// Token: 0x04003D6E RID: 15726
	private bool doHeadRotSlerp;

	// Token: 0x02000B1F RID: 2847
	public enum ID
	{
		// Token: 0x04003D70 RID: 15728
		Hmd,
		// Token: 0x04003D71 RID: 15729
		HandTracker_LeftWrist,
		// Token: 0x04003D72 RID: 15730
		HandTracker_RightWrist,
		// Token: 0x04003D73 RID: 15731
		HandTracker_LeftPointer,
		// Token: 0x04003D74 RID: 15732
		HandTracker_RightPointer,
		// Token: 0x04003D75 RID: 15733
		HandTracker_LeftGun,
		// Token: 0x04003D76 RID: 15734
		HandTracker_RightGun,
		// Token: 0x04003D77 RID: 15735
		HandTracker_LeftGrip,
		// Token: 0x04003D78 RID: 15736
		HandTracker_RightGrip,
		// Token: 0x04003D79 RID: 15737
		HandTracker_LeftPalm,
		// Token: 0x04003D7A RID: 15738
		HandTracker_RightPalm,
		// Token: 0x04003D7B RID: 15739
		FootTracker_LeftFoot,
		// Token: 0x04003D7C RID: 15740
		FootTracker_RightFoot,
		// Token: 0x04003D7D RID: 15741
		BodyTracker_Hip
	}

	// Token: 0x02000B20 RID: 2848
	public enum HeadLevel
	{
		// Token: 0x04003D7F RID: 15743
		Level,
		// Token: 0x04003D80 RID: 15744
		Up,
		// Token: 0x04003D81 RID: 15745
		Down
	}
}
