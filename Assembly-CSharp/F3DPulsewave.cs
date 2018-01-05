using System;
using UnityEngine;

// Token: 0x02000484 RID: 1156
public class F3DPulsewave : MonoBehaviour
{
	// Token: 0x060027F7 RID: 10231 RVA: 0x000D0184 File Offset: 0x000CE584
	private void Awake()
	{
		this.transform = base.GetComponent<Transform>();
		this.meshRenderer = base.GetComponent<MeshRenderer>();
		this.tintColorRef = Shader.PropertyToID("_TintColor");
		this.defaultColor = this.meshRenderer.material.GetColor(this.tintColorRef);
	}

	// Token: 0x060027F8 RID: 10232 RVA: 0x000D01D5 File Offset: 0x000CE5D5
	private void Start()
	{
		if (this.DebugLoop)
		{
			this.OnSpawned();
		}
	}

	// Token: 0x060027F9 RID: 10233 RVA: 0x000D01E8 File Offset: 0x000CE5E8
	private void OnSpawned()
	{
		this.transform.localScale = new Vector3(0f, 0f, 0f);
		this.isEnabled = true;
		this.isFadeOut = false;
		this.timerID = F3DTime.time.AddTimer(this.FadeOutDelay, new Action(this.OnFadeOut));
		this.meshRenderer.material.SetColor(this.tintColorRef, this.defaultColor);
		this.color = this.defaultColor;
	}

	// Token: 0x060027FA RID: 10234 RVA: 0x000D026C File Offset: 0x000CE66C
	private void OnDespawned()
	{
		if (this.timerID >= 0)
		{
			F3DTime.time.RemoveTimer(this.timerID);
			this.timerID = -1;
		}
	}

	// Token: 0x060027FB RID: 10235 RVA: 0x000D0291 File Offset: 0x000CE691
	private void OnFadeOut()
	{
		this.isFadeOut = true;
	}

	// Token: 0x060027FC RID: 10236 RVA: 0x000D029C File Offset: 0x000CE69C
	private void Update()
	{
		if (this.isEnabled)
		{
			this.transform.localScale = Vector3.Lerp(this.transform.localScale, this.ScaleSize, Time.deltaTime * this.ScaleTime);
			if (this.isFadeOut)
			{
				this.color = Color.Lerp(this.color, new Color(0f, 0f, 0f, -0.1f), Time.deltaTime * this.FadeOutTime);
				this.meshRenderer.material.SetColor(this.tintColorRef, this.color);
				if (this.color.a <= 0f)
				{
					this.isEnabled = false;
					if (this.DebugLoop)
					{
						this.OnDespawned();
						this.OnSpawned();
					}
				}
			}
		}
	}

	// Token: 0x0400162E RID: 5678
	public float FadeOutDelay;

	// Token: 0x0400162F RID: 5679
	public float FadeOutTime;

	// Token: 0x04001630 RID: 5680
	public float ScaleTime;

	// Token: 0x04001631 RID: 5681
	public Vector3 ScaleSize;

	// Token: 0x04001632 RID: 5682
	public bool DebugLoop;

	// Token: 0x04001633 RID: 5683
	private new Transform transform;

	// Token: 0x04001634 RID: 5684
	private MeshRenderer meshRenderer;

	// Token: 0x04001635 RID: 5685
	private int timerID = -1;

	// Token: 0x04001636 RID: 5686
	private bool isFadeOut;

	// Token: 0x04001637 RID: 5687
	private bool isEnabled;

	// Token: 0x04001638 RID: 5688
	private Color defaultColor;

	// Token: 0x04001639 RID: 5689
	private Color color;

	// Token: 0x0400163A RID: 5690
	private int tintColorRef;
}
