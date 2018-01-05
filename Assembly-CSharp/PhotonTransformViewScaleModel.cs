using System;

// Token: 0x02000782 RID: 1922
[Serializable]
public class PhotonTransformViewScaleModel
{
	// Token: 0x04002726 RID: 10022
	public bool SynchronizeEnabled;

	// Token: 0x04002727 RID: 10023
	public PhotonTransformViewScaleModel.InterpolateOptions InterpolateOption;

	// Token: 0x04002728 RID: 10024
	public float InterpolateMoveTowardsSpeed = 1f;

	// Token: 0x04002729 RID: 10025
	public float InterpolateLerpSpeed;

	// Token: 0x02000783 RID: 1923
	public enum InterpolateOptions
	{
		// Token: 0x0400272B RID: 10027
		Disabled,
		// Token: 0x0400272C RID: 10028
		MoveTowards,
		// Token: 0x0400272D RID: 10029
		Lerp
	}
}
