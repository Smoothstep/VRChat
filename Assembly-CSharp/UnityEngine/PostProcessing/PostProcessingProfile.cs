using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000832 RID: 2098
	public class PostProcessingProfile : ScriptableObject
	{
		// Token: 0x04002A53 RID: 10835
		public BuiltinDebugViewsModel debugViews = new BuiltinDebugViewsModel();

		// Token: 0x04002A54 RID: 10836
		public FogModel fog = new FogModel();

		// Token: 0x04002A55 RID: 10837
		public AntialiasingModel antialiasing = new AntialiasingModel();

		// Token: 0x04002A56 RID: 10838
		public AmbientOcclusionModel ambientOcclusion = new AmbientOcclusionModel();

		// Token: 0x04002A57 RID: 10839
		public ScreenSpaceReflectionModel screenSpaceReflection = new ScreenSpaceReflectionModel();

		// Token: 0x04002A58 RID: 10840
		public DepthOfFieldModel depthOfField = new DepthOfFieldModel();

		// Token: 0x04002A59 RID: 10841
		public MotionBlurModel motionBlur = new MotionBlurModel();

		// Token: 0x04002A5A RID: 10842
		public EyeAdaptationModel eyeAdaptation = new EyeAdaptationModel();

		// Token: 0x04002A5B RID: 10843
		public BloomModel bloom = new BloomModel();

		// Token: 0x04002A5C RID: 10844
		public ColorGradingModel colorGrading = new ColorGradingModel();

		// Token: 0x04002A5D RID: 10845
		public UserLutModel userLut = new UserLutModel();

		// Token: 0x04002A5E RID: 10846
		public ChromaticAberrationModel chromaticAberration = new ChromaticAberrationModel();

		// Token: 0x04002A5F RID: 10847
		public GrainModel grain = new GrainModel();

		// Token: 0x04002A60 RID: 10848
		public VignetteModel vignette = new VignetteModel();

		// Token: 0x04002A61 RID: 10849
		public DitheringModel dithering = new DitheringModel();
	}
}
