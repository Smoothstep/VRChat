using System;

// Token: 0x0200077F RID: 1919
[Serializable]
public class PhotonTransformViewRotationModel
{
	// Token: 0x0400271C RID: 10012
	public bool SynchronizeEnabled;

	// Token: 0x0400271D RID: 10013
	public PhotonTransformViewRotationModel.InterpolateOptions InterpolateOption = PhotonTransformViewRotationModel.InterpolateOptions.RotateTowards;

	// Token: 0x0400271E RID: 10014
	public float InterpolateRotateTowardsSpeed = 180f;

	// Token: 0x0400271F RID: 10015
	public float InterpolateLerpSpeed = 5f;

	// Token: 0x02000780 RID: 1920
	public enum InterpolateOptions
	{
		// Token: 0x04002721 RID: 10017
		Disabled,
		// Token: 0x04002722 RID: 10018
		RotateTowards,
		// Token: 0x04002723 RID: 10019
		Lerp
	}
}
