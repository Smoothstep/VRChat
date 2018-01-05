using System;

// Token: 0x02000B07 RID: 2823
public class UIInputController : InputStateController
{
	// Token: 0x06005585 RID: 21893 RVA: 0x001D82FB File Offset: 0x001D66FB
	private void Awake()
	{
		this.canInteract = false;
		this.canResetOrientation = false;
		this.headCamMouseRotator = VRCVrCamera.GetInstance().GetComponentInChildren<NeckMouseRotator>();
	}

	// Token: 0x06005586 RID: 21894 RVA: 0x001D831C File Offset: 0x001D671C
	public override void OnActivate()
	{
		VRCTrackingManager.ImmobilizePlayer(true);
		this.motionState = VRCPlayer.Instance.GetComponent<VRCMotionState>();
		this.motionState.isLocomoting = false;
		this.motionState.IsImmobilized = true;
		if (this.headCamMouseRotator != null)
		{
			this.headCamMouseRotator.neckLookUpDist = 0f;
			this.headCamMouseRotator.neckLookDownDist = 0f;
			this.headCamMouseRotator.rotationRange.x = 0f;
			this.headCamMouseRotator.rotationRange.z = 0f;
			this.headCamMouseRotator.rotationRange.y = 0f;
		}
	}

	// Token: 0x04003C6E RID: 15470
	private NeckMouseRotator headCamMouseRotator;

	// Token: 0x04003C6F RID: 15471
	private VRCMotionState motionState;
}
