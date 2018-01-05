using System;
using OvrTouch.Hands;
using UnityEngine;

namespace OvrTouch.Services
{
	// Token: 0x02000724 RID: 1828
	public class Colorizer : MonoBehaviour
	{
		// Token: 0x06003B80 RID: 15232 RVA: 0x0012B71D File Offset: 0x00129B1D
		private void OnGrabBegin(GrabbableGrabMsg grabMsg)
		{
			this.SetColor(Colorizer.Const.ColorGrab);
		}

		// Token: 0x06003B81 RID: 15233 RVA: 0x0012B72A File Offset: 0x00129B2A
		private void OnGrabEnd(GrabbableGrabMsg grabMsg)
		{
			this.SetColor(this.m_color);
		}

		// Token: 0x06003B82 RID: 15234 RVA: 0x0012B738 File Offset: 0x00129B38
		private void OnOverlapBegin(GrabbableOverlapMsg overlapMsg)
		{
		}

		// Token: 0x06003B83 RID: 15235 RVA: 0x0012B73A File Offset: 0x00129B3A
		private void OnOverlapEnd(GrabbableOverlapMsg overlapMsg)
		{
		}

		// Token: 0x06003B84 RID: 15236 RVA: 0x0012B73C File Offset: 0x00129B3C
		private void Awake()
		{
			this.m_color = this.RandomColor();
			this.m_meshRenderers = base.GetComponentsInChildren<MeshRenderer>();
			this.SetColor(this.m_color);
		}

		// Token: 0x06003B85 RID: 15237 RVA: 0x0012B764 File Offset: 0x00129B64
		private void SetColor(Color color)
		{
			foreach (MeshRenderer meshRenderer in this.m_meshRenderers)
			{
				foreach (Material material in meshRenderer.materials)
				{
					material.color = color;
				}
			}
		}

		// Token: 0x06003B86 RID: 15238 RVA: 0x0012B7C0 File Offset: 0x00129BC0
		private Color RandomColor()
		{
			Color result = new Color(UnityEngine.Random.Range(0.1f, 0.95f), UnityEngine.Random.Range(0.1f, 0.95f), UnityEngine.Random.Range(0.1f, 0.95f), 1f);
			return result;
		}

		// Token: 0x04002449 RID: 9289
		private Color m_color = Color.black;

		// Token: 0x0400244A RID: 9290
		private MeshRenderer[] m_meshRenderers;

		// Token: 0x02000725 RID: 1829
		private static class Const
		{
			// Token: 0x0400244B RID: 9291
			public static readonly Color ColorGrab = new Color(1f, 0.5f, 0f, 1f);
		}
	}
}
