using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000831 RID: 2097
	[Serializable]
	public abstract class PostProcessingModel
	{
		// Token: 0x17000A75 RID: 2677
		// (get) Token: 0x0600414F RID: 16719 RVA: 0x00147BB8 File Offset: 0x00145FB8
		// (set) Token: 0x06004150 RID: 16720 RVA: 0x00147BC0 File Offset: 0x00145FC0
		public bool enabled
		{
			get
			{
				return this.m_Enabled;
			}
			set
			{
				this.m_Enabled = value;
				if (value)
				{
					this.OnValidate();
				}
			}
		}

		// Token: 0x06004151 RID: 16721
		public abstract void Reset();

		// Token: 0x06004152 RID: 16722 RVA: 0x00147BD5 File Offset: 0x00145FD5
		public virtual void OnValidate()
		{
		}

		// Token: 0x04002A52 RID: 10834
		[SerializeField]
		[GetSet("enabled")]
		private bool m_Enabled;
	}
}
