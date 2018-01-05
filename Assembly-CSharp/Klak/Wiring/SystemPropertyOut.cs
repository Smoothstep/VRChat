using System;
using UnityEngine;

namespace Klak.Wiring
{
	// Token: 0x02000555 RID: 1365
	[AddComponentMenu("Klak/Wiring/Output/System Property Out")]
	public class SystemPropertyOut : NodeBase
	{
		// Token: 0x17000724 RID: 1828
		// (set) Token: 0x06002EF9 RID: 12025 RVA: 0x000E46B1 File Offset: 0x000E2AB1
		[Inlet]
		public float timeScale
		{
			set
			{
				if (base.enabled)
				{
					Time.timeScale = value;
				}
			}
		}

		// Token: 0x17000725 RID: 1829
		// (set) Token: 0x06002EFA RID: 12026 RVA: 0x000E46C4 File Offset: 0x000E2AC4
		[Inlet]
		public Vector3 gravity
		{
			set
			{
				if (base.enabled)
				{
					Physics.gravity = value;
				}
			}
		}

		// Token: 0x17000726 RID: 1830
		// (set) Token: 0x06002EFB RID: 12027 RVA: 0x000E46D7 File Offset: 0x000E2AD7
		[Inlet]
		public float ambientIntensity
		{
			set
			{
				if (base.enabled)
				{
					RenderSettings.ambientIntensity = value;
				}
			}
		}

		// Token: 0x17000727 RID: 1831
		// (set) Token: 0x06002EFC RID: 12028 RVA: 0x000E46EA File Offset: 0x000E2AEA
		[Inlet]
		public float reflectionIntensity
		{
			set
			{
				if (base.enabled)
				{
					RenderSettings.reflectionIntensity = value;
				}
			}
		}

		// Token: 0x17000728 RID: 1832
		// (set) Token: 0x06002EFD RID: 12029 RVA: 0x000E46FD File Offset: 0x000E2AFD
		[Inlet]
		public Color fogColor
		{
			set
			{
				if (base.enabled)
				{
					RenderSettings.fogColor = value;
				}
			}
		}

		// Token: 0x17000729 RID: 1833
		// (set) Token: 0x06002EFE RID: 12030 RVA: 0x000E4710 File Offset: 0x000E2B10
		[Inlet]
		public float fogDensity
		{
			set
			{
				if (base.enabled)
				{
					RenderSettings.fogDensity = value;
				}
			}
		}

		// Token: 0x1700072A RID: 1834
		// (set) Token: 0x06002EFF RID: 12031 RVA: 0x000E4723 File Offset: 0x000E2B23
		[Inlet]
		public float fogStartDistance
		{
			set
			{
				if (base.enabled)
				{
					RenderSettings.fogStartDistance = value;
				}
			}
		}

		// Token: 0x1700072B RID: 1835
		// (set) Token: 0x06002F00 RID: 12032 RVA: 0x000E4736 File Offset: 0x000E2B36
		[Inlet]
		public float fogEndDistance
		{
			set
			{
				if (base.enabled)
				{
					RenderSettings.fogEndDistance = value;
				}
			}
		}
	}
}
