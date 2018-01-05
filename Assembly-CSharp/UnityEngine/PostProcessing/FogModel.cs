using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000819 RID: 2073
	[Serializable]
	public class FogModel : PostProcessingModel
	{
		// Token: 0x17000A61 RID: 2657
		// (get) Token: 0x06004105 RID: 16645 RVA: 0x0014894E File Offset: 0x00146D4E
		// (set) Token: 0x06004106 RID: 16646 RVA: 0x00148956 File Offset: 0x00146D56
		public FogModel.Settings settings
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

		// Token: 0x06004107 RID: 16647 RVA: 0x0014895F File Offset: 0x00146D5F
		public override void Reset()
		{
			this.m_Settings = FogModel.Settings.defaultSettings;
		}

		// Token: 0x040029FC RID: 10748
		[SerializeField]
		private FogModel.Settings m_Settings = FogModel.Settings.defaultSettings;

		// Token: 0x0200081A RID: 2074
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000A62 RID: 2658
			// (get) Token: 0x06004108 RID: 16648 RVA: 0x0014896C File Offset: 0x00146D6C
			public static FogModel.Settings defaultSettings
			{
				get
				{
					return new FogModel.Settings
					{
						excludeSkybox = true
					};
				}
			}

			// Token: 0x040029FD RID: 10749
			[Tooltip("Should the fog affect the skybox?")]
			public bool excludeSkybox;
		}
	}
}
