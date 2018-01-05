using System;
using UnityEngine;

// Token: 0x02000AA0 RID: 2720
public abstract class InputStateController : MonoBehaviour
{
	// Token: 0x17000C05 RID: 3077
	// (get) Token: 0x060051C5 RID: 20933 RVA: 0x001BF049 File Offset: 0x001BD449
	public VRCMotionState MotionState
	{
		get
		{
			if (this.motionState == null)
			{
				this.motionState = VRCPlayer.Instance.GetComponent<VRCMotionState>();
			}
			return this.motionState;
		}
	}

	// Token: 0x060051C6 RID: 20934 RVA: 0x001BF072 File Offset: 0x001BD472
	public virtual void OnActivate()
	{
	}

	// Token: 0x060051C7 RID: 20935 RVA: 0x001BF074 File Offset: 0x001BD474
	public void ResetLastPosition()
	{
		InputStateController.lastPosition = base.transform.position;
		InputStateController.lastRotation = base.transform.rotation;
	}

	// Token: 0x04003A0D RID: 14861
	private VRCMotionState motionState;

	// Token: 0x04003A0E RID: 14862
	public bool canInteract = true;

	// Token: 0x04003A0F RID: 14863
	public bool canResetOrientation = true;

	// Token: 0x04003A10 RID: 14864
	protected static Vector3 lastPosition = Vector3.zero;

	// Token: 0x04003A11 RID: 14865
	protected static Quaternion lastRotation = Quaternion.identity;
}
