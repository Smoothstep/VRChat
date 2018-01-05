using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020007EC RID: 2028
	public sealed class UserLutComponent : PostProcessingComponentRenderTexture<UserLutModel>
	{
		// Token: 0x17000A3C RID: 2620
		// (get) Token: 0x060040B7 RID: 16567 RVA: 0x00147874 File Offset: 0x00145C74
		public override bool active
		{
			get
			{
				UserLutModel.Settings settings = base.model.settings;
				return base.model.enabled && settings.lut != null && settings.contribution > 0f && settings.lut.height == (int)Mathf.Sqrt((float)settings.lut.width) && !this.context.interrupted;
			}
		}

		// Token: 0x060040B8 RID: 16568 RVA: 0x001478F8 File Offset: 0x00145CF8
		public override void Prepare(Material uberMaterial)
		{
			UserLutModel.Settings settings = base.model.settings;
			uberMaterial.EnableKeyword("USER_LUT");
			uberMaterial.SetTexture(UserLutComponent.Uniforms._UserLut, settings.lut);
			uberMaterial.SetVector(UserLutComponent.Uniforms._UserLut_Params, new Vector4(1f / (float)settings.lut.width, 1f / (float)settings.lut.height, (float)settings.lut.height - 1f, settings.contribution));
		}

		// Token: 0x060040B9 RID: 16569 RVA: 0x00147980 File Offset: 0x00145D80
		public void OnGUI()
		{
			UserLutModel.Settings settings = base.model.settings;
			Rect position = new Rect(this.context.viewport.x * (float)Screen.width + 8f, 8f, (float)settings.lut.width, (float)settings.lut.height);
			GUI.DrawTexture(position, settings.lut);
		}

		// Token: 0x020007ED RID: 2029
		private static class Uniforms
		{
			// Token: 0x04002959 RID: 10585
			internal static readonly int _UserLut = Shader.PropertyToID("_UserLut");

			// Token: 0x0400295A RID: 10586
			internal static readonly int _UserLut_Params = Shader.PropertyToID("_UserLut_Params");
		}
	}
}
