using System;
using UnityEngine;

// Token: 0x0200077B RID: 1915
[Serializable]
public class PhotonTransformViewPositionModel
{
	// Token: 0x04002701 RID: 9985
	public bool SynchronizeEnabled;

	// Token: 0x04002702 RID: 9986
	public bool TeleportEnabled = true;

	// Token: 0x04002703 RID: 9987
	public float TeleportIfDistanceGreaterThan = 3f;

	// Token: 0x04002704 RID: 9988
	public PhotonTransformViewPositionModel.InterpolateOptions InterpolateOption = PhotonTransformViewPositionModel.InterpolateOptions.EstimatedSpeed;

	// Token: 0x04002705 RID: 9989
	public float InterpolateMoveTowardsSpeed = 1f;

	// Token: 0x04002706 RID: 9990
	public float InterpolateLerpSpeed = 1f;

	// Token: 0x04002707 RID: 9991
	public float InterpolateMoveTowardsAcceleration = 2f;

	// Token: 0x04002708 RID: 9992
	public float InterpolateMoveTowardsDeceleration = 2f;

	// Token: 0x04002709 RID: 9993
	public AnimationCurve InterpolateSpeedCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(-1f, 0f, 0f, float.PositiveInfinity),
		new Keyframe(0f, 1f, 0f, 0f),
		new Keyframe(1f, 1f, 0f, 1f),
		new Keyframe(4f, 4f, 1f, 0f)
	});

	// Token: 0x0400270A RID: 9994
	public PhotonTransformViewPositionModel.ExtrapolateOptions ExtrapolateOption;

	// Token: 0x0400270B RID: 9995
	public float ExtrapolateSpeed = 1f;

	// Token: 0x0400270C RID: 9996
	public bool ExtrapolateIncludingRoundTripTime = true;

	// Token: 0x0400270D RID: 9997
	public int ExtrapolateNumberOfStoredPositions = 1;

	// Token: 0x0400270E RID: 9998
	public bool DrawErrorGizmo = true;

	// Token: 0x0200077C RID: 1916
	public enum InterpolateOptions
	{
		// Token: 0x04002710 RID: 10000
		Disabled,
		// Token: 0x04002711 RID: 10001
		FixedSpeed,
		// Token: 0x04002712 RID: 10002
		EstimatedSpeed,
		// Token: 0x04002713 RID: 10003
		SynchronizeValues,
		// Token: 0x04002714 RID: 10004
		Lerp
	}

	// Token: 0x0200077D RID: 1917
	public enum ExtrapolateOptions
	{
		// Token: 0x04002716 RID: 10006
		Disabled,
		// Token: 0x04002717 RID: 10007
		SynchronizeValues,
		// Token: 0x04002718 RID: 10008
		EstimateSpeedAndTurn,
		// Token: 0x04002719 RID: 10009
		FixedSpeed
	}
}
