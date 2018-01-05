using System;
using UnityEngine;

// Token: 0x02000A9C RID: 2716
public class ImmobileInputController : InputStateController
{
	// Token: 0x060051A3 RID: 20899 RVA: 0x001BFBBE File Offset: 0x001BDFBE
	private void Awake()
	{
		this.headCamMouseRotator = VRCVrCamera.GetInstance().GetComponentInChildren<NeckMouseRotator>();
	}

	// Token: 0x060051A4 RID: 20900 RVA: 0x001BFBD0 File Offset: 0x001BDFD0
	private void Start()
	{
	}

	// Token: 0x060051A5 RID: 20901 RVA: 0x001BFBD4 File Offset: 0x001BDFD4
	public override void OnActivate()
	{
		VRCTrackingManager.ImmobilizePlayer(true);
		if (this.headCamMouseRotator != null)
		{
			float trackingScale = VRCTrackingManager.GetTrackingScale();
			this.headCamMouseRotator.neckLookUpDist = -0.1f * trackingScale;
			this.headCamMouseRotator.neckLookDownDist = 0f;
			this.headCamMouseRotator.rotationRange.x = -70f;
			this.headCamMouseRotator.rotationRange.z = 80f;
			this.headCamMouseRotator.rotationRange.y = 180f;
		}
		this.motionState = VRCPlayer.Instance.GetComponent<VRCMotionState>();
	}

	// Token: 0x060051A6 RID: 20902 RVA: 0x001BFC70 File Offset: 0x001BE070
	public void FixedUpdate()
	{
		Vector3 playerWorldMotion = base.transform.position - InputStateController.lastPosition;
		Quaternion playerWorldRotation = Quaternion.Inverse(InputStateController.lastRotation) * base.transform.rotation;
		VRCTrackingManager.ApplyPlayerMotion(playerWorldMotion, playerWorldRotation);
		this.motionState.isLocomoting = false;
		this.motionState.IsImmobilized = true;
		if (!this.motionState.InVehicle)
		{
			VRCTrackingManager.SetPlayerNearTracking(VRCTrackingManager.GetWorldTrackingMotion().magnitude < 0.5f);
		}
		InputStateController.lastPosition = base.transform.position;
		InputStateController.lastRotation = base.transform.rotation;
	}

	// Token: 0x040039EB RID: 14827
	private NeckMouseRotator headCamMouseRotator;

	// Token: 0x040039EC RID: 14828
	private VRCMotionState motionState;
}
