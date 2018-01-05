using System;
using UnityEngine;

// Token: 0x02000700 RID: 1792
public class OVRLipSyncContextTextureFlip : MonoBehaviour
{
	// Token: 0x06003AB9 RID: 15033 RVA: 0x00128898 File Offset: 0x00126C98
	public void Initialize()
	{
		if (this.material == null)
		{
			Debug.Log("LipSyncContextTextureFlip.Start WARNING: Please set required public components!");
			return;
		}
		this.phonemeContext = base.GetComponent<OVRLipSyncContext>();
		if (this.phonemeContext == null)
		{
			Debug.Log("LipSyncContextTextureFlip.Start WARNING: No phoneme context component set to object");
		}
	}

	// Token: 0x06003ABA RID: 15034 RVA: 0x001288E8 File Offset: 0x00126CE8
	private void Update()
	{
		if (this.phonemeContext != null && this.material != null && this.phonemeContext.GetCurrentPhonemeFrame(ref this.frame) == 0)
		{
			for (int i = 0; i < this.frame.Visemes.Length; i++)
			{
				this.oldFrame.Visemes[i] = this.oldFrame.Visemes[i] * this.smoothing + this.frame.Visemes[i] * (1f - this.smoothing);
			}
			this.SetVisemeToTexture();
		}
	}

	// Token: 0x06003ABB RID: 15035 RVA: 0x00128990 File Offset: 0x00126D90
	private void SetVisemeToTexture()
	{
		int num = -1;
		float num2 = 0f;
		for (int i = 0; i < this.oldFrame.Visemes.Length; i++)
		{
			if (this.oldFrame.Visemes[i] > num2)
			{
				num = i;
				num2 = this.oldFrame.Visemes[i];
			}
		}
		if (num != -1 && num < this.Textures.Length)
		{
			Texture texture = this.Textures[num];
			if (texture != null)
			{
				this.material.SetTexture("_MainTex", texture);
			}
		}
	}

	// Token: 0x0400237B RID: 9083
	public Material material;

	// Token: 0x0400237C RID: 9084
	public Texture[] Textures = new Texture[15];

	// Token: 0x0400237D RID: 9085
	public float smoothing;

	// Token: 0x0400237E RID: 9086
	private OVRLipSyncContext phonemeContext;

	// Token: 0x0400237F RID: 9087
	private OVRLipSync.ovrLipSyncFrame frame = new OVRLipSync.ovrLipSyncFrame(0);

	// Token: 0x04002380 RID: 9088
	private OVRLipSync.ovrLipSyncFrame oldFrame = new OVRLipSync.ovrLipSyncFrame(0);
}
