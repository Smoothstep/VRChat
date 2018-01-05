using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000826 RID: 2086
	[Serializable]
	public class UserLutModel : PostProcessingModel
	{
		// Token: 0x17000A69 RID: 2665
		// (get) Token: 0x06004119 RID: 16665 RVA: 0x00148B7D File Offset: 0x00146F7D
		// (set) Token: 0x0600411A RID: 16666 RVA: 0x00148B85 File Offset: 0x00146F85
		public UserLutModel.Settings settings
		{
			get
			{
				return this.m_Settings;
			}
			set
			{
				this.m_Settings = value;
			}
		}

		// Token: 0x0600411B RID: 16667 RVA: 0x00148B8E File Offset: 0x00146F8E
		public override void Reset()
		{
			this.m_Settings = UserLutModel.Settings.defaultSettings;
		}

		// Token: 0x04002A1E RID: 10782
		[SerializeField]
		private UserLutModel.Settings m_Settings = UserLutModel.Settings.defaultSettings;

		// Token: 0x02000827 RID: 2087
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000A6A RID: 2666
			// (get) Token: 0x0600411C RID: 16668 RVA: 0x00148B9C File Offset: 0x00146F9C
			public static UserLutModel.Settings defaultSettings
			{
				get
				{
					return new UserLutModel.Settings
					{
						lut = null,
						contribution = 1f
					};
				}
			}

			// Token: 0x04002A1F RID: 10783
			[Tooltip("Custom lookup texture (strip format, e.g. 256x16).")]
			public Texture2D lut;

			// Token: 0x04002A20 RID: 10784
			[Range(0f, 1f)]
			[Tooltip("Blending factor.")]
			public float contribution;
		}
	}
}
