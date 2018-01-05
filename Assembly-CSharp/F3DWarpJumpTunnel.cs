using System;
using UnityEngine;

// Token: 0x0200048C RID: 1164
public class F3DWarpJumpTunnel : MonoBehaviour
{
	// Token: 0x0600281B RID: 10267 RVA: 0x000D0C0A File Offset: 0x000CF00A
	private void Awake()
	{
		this.transform = base.GetComponent<Transform>();
		this.meshRenderer = base.GetComponent<MeshRenderer>();
		this.alphaID = Shader.PropertyToID("_Alpha");
	}

	// Token: 0x0600281C RID: 10268 RVA: 0x000D0C34 File Offset: 0x000CF034
	public void OnSpawned()
	{
		this.grow = false;
		this.meshRenderer.material.SetFloat(this.alphaID, 0f);
		F3DTime.time.AddTimer(this.StartDelay, 1, new Action(this.ToggleGrow));
		F3DTime.time.AddTimer(this.FadeDelay, 1, new Action(this.ToggleGrow));
		this.transform.localScale = new Vector3(1f, 1f, 1f);
		this.transform.localRotation = this.transform.localRotation * Quaternion.Euler(0f, 0f, (float)UnityEngine.Random.Range(-360, 360));
	}

	// Token: 0x0600281D RID: 10269 RVA: 0x000D0CF8 File Offset: 0x000CF0F8
	private void ToggleGrow()
	{
		this.grow = !this.grow;
	}

	// Token: 0x0600281E RID: 10270 RVA: 0x000D0D0C File Offset: 0x000CF10C
	private void Update()
	{
		this.transform.Rotate(0f, 0f, this.RotationSpeed * Time.deltaTime);
		if (this.grow)
		{
			this.transform.localScale = Vector3.Lerp(this.transform.localScale, this.ScaleTo, Time.deltaTime * this.ScaleTime);
			this.alpha = Mathf.Lerp(this.alpha, 1f, Time.deltaTime * this.ColorTime);
			this.meshRenderer.material.SetFloat(this.alphaID, this.alpha);
		}
		else
		{
			this.alpha = Mathf.Lerp(this.alpha, 0f, Time.deltaTime * this.ColorFadeTime);
			this.meshRenderer.material.SetFloat(this.alphaID, this.alpha);
		}
	}

	// Token: 0x04001660 RID: 5728
	private new Transform transform;

	// Token: 0x04001661 RID: 5729
	private MeshRenderer meshRenderer;

	// Token: 0x04001662 RID: 5730
	public float StartDelay;

	// Token: 0x04001663 RID: 5731
	public float FadeDelay;

	// Token: 0x04001664 RID: 5732
	public Vector3 ScaleTo;

	// Token: 0x04001665 RID: 5733
	public float ScaleTime;

	// Token: 0x04001666 RID: 5734
	public float ColorTime;

	// Token: 0x04001667 RID: 5735
	public float ColorFadeTime;

	// Token: 0x04001668 RID: 5736
	public float RotationSpeed;

	// Token: 0x04001669 RID: 5737
	private bool grow;

	// Token: 0x0400166A RID: 5738
	private float alpha;

	// Token: 0x0400166B RID: 5739
	private int alphaID;
}
