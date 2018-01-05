using System;
using UnityEngine;

// Token: 0x02000891 RID: 2193
public class WaterUvAnimation : MonoBehaviour
{
	// Token: 0x06004370 RID: 17264 RVA: 0x00163B36 File Offset: 0x00161F36
	private void Awake()
	{
		this.mat = base.GetComponent<Renderer>().materials[this.MaterialNomber];
	}

	// Token: 0x06004371 RID: 17265 RVA: 0x00163B50 File Offset: 0x00161F50
	private void Update()
	{
		if (this.IsReverse)
		{
			this.offset -= Time.deltaTime * this.Speed;
			if (this.offset < 0f)
			{
				this.offset = 1f;
			}
		}
		else
		{
			this.offset += Time.deltaTime * this.Speed;
			if (this.offset > 1f)
			{
				this.offset = 0f;
			}
		}
		Vector2 value = new Vector2(0f, this.offset);
		this.mat.SetTextureOffset("_BumpMap", value);
		this.mat.SetFloat("_OffsetYHeightMap", this.offset);
	}

	// Token: 0x04002BFA RID: 11258
	public bool IsReverse;

	// Token: 0x04002BFB RID: 11259
	public float Speed = 1f;

	// Token: 0x04002BFC RID: 11260
	public int MaterialNomber;

	// Token: 0x04002BFD RID: 11261
	private Material mat;

	// Token: 0x04002BFE RID: 11262
	private float deltaFps;

	// Token: 0x04002BFF RID: 11263
	private bool isVisible;

	// Token: 0x04002C00 RID: 11264
	private bool isCorutineStarted;

	// Token: 0x04002C01 RID: 11265
	private float offset;

	// Token: 0x04002C02 RID: 11266
	private float delta;
}
