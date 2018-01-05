using System;

namespace RealisticEyeMovements
{
	// Token: 0x020008B7 RID: 2231
	[Serializable]
	public class EyeAndHeadAnimatorForExport
	{
		// Token: 0x04002D8C RID: 11660
		public string headBonePath;

		// Token: 0x04002D8D RID: 11661
		public float headSpeedModifier;

		// Token: 0x04002D8E RID: 11662
		public float headWeight;

		// Token: 0x04002D8F RID: 11663
		public bool useMicroSaccades;

		// Token: 0x04002D90 RID: 11664
		public bool useMacroSaccades;

		// Token: 0x04002D91 RID: 11665
		public bool kDrawSightlinesInEditor;

		// Token: 0x04002D92 RID: 11666
		public ControlData.ControlDataForExport controlData;

		// Token: 0x04002D93 RID: 11667
		public float kMinNextBlinkTime;

		// Token: 0x04002D94 RID: 11668
		public float kMaxNextBlinkTime;

		// Token: 0x04002D95 RID: 11669
		public bool eyelidsFollowEyesVertically;

		// Token: 0x04002D96 RID: 11670
		public float maxEyeHorizAngle;

		// Token: 0x04002D97 RID: 11671
		public float maxEyeHorizAngleTowardsNose;

		// Token: 0x04002D98 RID: 11672
		public float crossEyeCorrection;

		// Token: 0x04002D99 RID: 11673
		public float nervousness;

		// Token: 0x04002D9A RID: 11674
		public float limitHeadAngle;
	}
}
