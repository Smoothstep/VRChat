using System;
using UnityEngine;

// Token: 0x02000445 RID: 1093
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Vintage")]
public class CC_Vintage : CC_LookupFilter
{
	// Token: 0x060026CA RID: 9930 RVA: 0x000BF52C File Offset: 0x000BD92C
	protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.filter != this.m_CurrentFilter)
		{
			this.m_CurrentFilter = this.filter;
			if (this.filter == CC_Vintage.Filter.None)
			{
				this.lookupTexture = null;
			}
			else
			{
				this.lookupTexture = Resources.Load<Texture2D>("Instagram/" + this.filter.ToString());
			}
		}
		base.OnRenderImage(source, destination);
	}

	// Token: 0x04001405 RID: 5125
	public CC_Vintage.Filter filter;

	// Token: 0x04001406 RID: 5126
	protected CC_Vintage.Filter m_CurrentFilter;

	// Token: 0x02000446 RID: 1094
	public enum Filter
	{
		// Token: 0x04001408 RID: 5128
		None,
		// Token: 0x04001409 RID: 5129
		F1977,
		// Token: 0x0400140A RID: 5130
		Aden,
		// Token: 0x0400140B RID: 5131
		Amaro,
		// Token: 0x0400140C RID: 5132
		Brannan,
		// Token: 0x0400140D RID: 5133
		Crema,
		// Token: 0x0400140E RID: 5134
		Earlybird,
		// Token: 0x0400140F RID: 5135
		Hefe,
		// Token: 0x04001410 RID: 5136
		Hudson,
		// Token: 0x04001411 RID: 5137
		Inkwell,
		// Token: 0x04001412 RID: 5138
		Juno,
		// Token: 0x04001413 RID: 5139
		Kelvin,
		// Token: 0x04001414 RID: 5140
		Lark,
		// Token: 0x04001415 RID: 5141
		LoFi,
		// Token: 0x04001416 RID: 5142
		Ludwig,
		// Token: 0x04001417 RID: 5143
		Mayfair,
		// Token: 0x04001418 RID: 5144
		Nashville,
		// Token: 0x04001419 RID: 5145
		Perpetua,
		// Token: 0x0400141A RID: 5146
		Reyes,
		// Token: 0x0400141B RID: 5147
		Rise,
		// Token: 0x0400141C RID: 5148
		Sierra,
		// Token: 0x0400141D RID: 5149
		Slumber,
		// Token: 0x0400141E RID: 5150
		Sutro,
		// Token: 0x0400141F RID: 5151
		Toaster,
		// Token: 0x04001420 RID: 5152
		Valencia,
		// Token: 0x04001421 RID: 5153
		Walden,
		// Token: 0x04001422 RID: 5154
		Willow,
		// Token: 0x04001423 RID: 5155
		XProII
	}
}
