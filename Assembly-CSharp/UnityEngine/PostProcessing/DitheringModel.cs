using System;
using System.Runtime.InteropServices;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000814 RID: 2068
	[Serializable]
	public class DitheringModel : PostProcessingModel
	{
		// Token: 0x17000A5D RID: 2653
		// (get) Token: 0x060040FB RID: 16635 RVA: 0x00148849 File Offset: 0x00146C49
		// (set) Token: 0x060040FC RID: 16636 RVA: 0x00148851 File Offset: 0x00146C51
		public DitheringModel.Settings settings
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

		// Token: 0x060040FD RID: 16637 RVA: 0x0014885A File Offset: 0x00146C5A
		public override void Reset()
		{
			this.m_Settings = DitheringModel.Settings.defaultSettings;
		}

		// Token: 0x040029EC RID: 10732
		[SerializeField]
		private DitheringModel.Settings m_Settings = DitheringModel.Settings.defaultSettings;

		// Token: 0x02000815 RID: 2069
		[Serializable]
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct Settings
		{
			// Token: 0x17000A5E RID: 2654
			// (get) Token: 0x060040FE RID: 16638 RVA: 0x00148868 File Offset: 0x00146C68
			public static DitheringModel.Settings defaultSettings
			{
				get
				{
					return default(DitheringModel.Settings);
				}
			}
		}
	}
}
