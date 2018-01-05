using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x020008ED RID: 2285
	public class ShaderPropAnimator : MonoBehaviour
	{
		// Token: 0x06004548 RID: 17736 RVA: 0x001737EB File Offset: 0x00171BEB
		private void Awake()
		{
			this.m_Renderer = base.GetComponent<Renderer>();
			this.m_Material = this.m_Renderer.material;
		}

		// Token: 0x06004549 RID: 17737 RVA: 0x0017380A File Offset: 0x00171C0A
		private void Start()
		{
			base.StartCoroutine(this.AnimateProperties());
		}

		// Token: 0x0600454A RID: 17738 RVA: 0x0017381C File Offset: 0x00171C1C
		private IEnumerator AnimateProperties()
		{
			this.m_frame = UnityEngine.Random.Range(0f, 1f);
			for (;;)
			{
				float glowPower = this.GlowCurve.Evaluate(this.m_frame);
				this.m_Material.SetFloat(ShaderUtilities.ID_GlowPower, glowPower);
				this.m_frame += Time.deltaTime * UnityEngine.Random.Range(0.2f, 0.3f);
				yield return new WaitForEndOfFrame();
			}
			yield break;
		}

		// Token: 0x04002F58 RID: 12120
		private Renderer m_Renderer;

		// Token: 0x04002F59 RID: 12121
		private Material m_Material;

		// Token: 0x04002F5A RID: 12122
		public AnimationCurve GlowCurve;

		// Token: 0x04002F5B RID: 12123
		public float m_frame;
	}
}
