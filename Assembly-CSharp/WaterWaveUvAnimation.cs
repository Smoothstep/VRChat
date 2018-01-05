using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000892 RID: 2194
public class WaterWaveUvAnimation : MonoBehaviour
{
	// Token: 0x06004373 RID: 17267 RVA: 0x00163C29 File Offset: 0x00162029
	private void Start()
	{
		this.mat = base.GetComponent<Renderer>().material;
		this.delta = 1f / (float)this.fps * this.speed;
		base.StartCoroutine(this.updateTiling());
	}

	// Token: 0x06004374 RID: 17268 RVA: 0x00163C64 File Offset: 0x00162064
	private IEnumerator updateTiling()
	{
		for (;;)
		{
			this.offset += this.delta;
			this.offsetHeight += this.delta;
			if (this.offset >= 1f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			Vector2 vec = new Vector2(0f, this.offset);
			this.mat.SetTextureOffset("_BumpMap", vec);
			this.mat.SetFloat("_OffsetYHeightMap", this.offset);
			if (this.offset < 0.3f)
			{
				this.mat.SetColor("_Color", new Color(this.color.r, this.color.g, this.color.b, this.offset / 0.3f));
			}
			if (this.offset > 0.7f)
			{
				this.mat.SetColor("_Color", new Color(this.color.r, this.color.g, this.color.b, (1f - this.offset) / 0.3f));
			}
			yield return new WaitForSeconds(1f / (float)this.fps);
		}
		yield break;
	}

	// Token: 0x04002C03 RID: 11267
	public float speed = 1f;

	// Token: 0x04002C04 RID: 11268
	public int fps = 30;

	// Token: 0x04002C05 RID: 11269
	public Color color;

	// Token: 0x04002C06 RID: 11270
	private Material mat;

	// Token: 0x04002C07 RID: 11271
	private float offset;

	// Token: 0x04002C08 RID: 11272
	private float offsetHeight;

	// Token: 0x04002C09 RID: 11273
	private float delta;
}
