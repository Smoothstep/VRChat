using System;
using UnityEngine;

// Token: 0x02000C63 RID: 3171
public class VRCUiBackgroundFade : MonoBehaviour
{
	// Token: 0x06006286 RID: 25222 RVA: 0x0023232C File Offset: 0x0023072C
	private void Start()
	{
		this.renderers = base.GetComponentsInChildren<Renderer>();
		this.rendererDefaultAlpha = new float[this.renderers.Length];
		this.renderColorProperty = new string[this.renderers.Length];
		for (int i = 0; i < this.renderers.Length; i++)
		{
			if (this.renderQueueOverride != -1)
			{
				this.renderers[i].material.renderQueue = this.renderQueueOverride;
			}
			this.renderColorProperty[i] = "_Color";
			foreach (string text in this.colorVariables)
			{
				if (this.renderers[i].material.HasProperty(text))
				{
					this.renderColorProperty[i] = text;
					break;
				}
			}
			this.rendererDefaultAlpha[i] = this.renderers[i].material.GetColor(this.renderColorProperty[i]).a;
		}
		this.audioSources = base.GetComponentsInChildren<AudioSource>();
		this.audioVolumes = new float[this.audioSources.Length];
		for (int k = 0; k < this.audioSources.Length; k++)
		{
			this.audioVolumes[k] = this.audioSources[k].volume;
		}
	}

	// Token: 0x06006287 RID: 25223 RVA: 0x00232478 File Offset: 0x00230878
	private void Update()
	{
		if (this.complete)
		{
			return;
		}
		this.complete = true;
		for (int i = 0; i < this.renderers.Length; i++)
		{
			Color color = this.renderers[i].material.GetColor(this.renderColorProperty[i]);
			color.a = Mathf.MoveTowards(color.a, this.TargetFade * this.rendererDefaultAlpha[i], 1f * Time.deltaTime);
			this.renderers[i].material.SetColor(this.renderColorProperty[i], color);
			if (color.a != this.TargetFade * this.rendererDefaultAlpha[i])
			{
				this.complete = false;
			}
			if (color.a == 0f)
			{
				this.renderers[i].enabled = false;
			}
			else
			{
				this.renderers[i].enabled = true;
			}
		}
		for (int j = 0; j < this.audioSources.Length; j++)
		{
			this.audioSources[j].volume = Mathf.MoveTowards(this.audioSources[j].volume, this.TargetFade * this.audioVolumes[j], 1f * Time.deltaTime);
			if (this.audioSources[j].volume != this.TargetFade)
			{
				this.complete = false;
			}
		}
		if (this.complete && this.completeAction != null)
		{
			this.completeAction();
			this.completeAction = null;
		}
	}

	// Token: 0x06006288 RID: 25224 RVA: 0x00232601 File Offset: 0x00230A01
	public void FadeTo(float fade, Action action)
	{
		if (this.completeAction != null)
		{
			Debug.LogError("Queing a fade before previous fade completes.");
		}
		this.complete = false;
		this.TargetFade = fade;
		this.completeAction = action;
	}

	// Token: 0x040047EE RID: 18414
	private Renderer[] renderers;

	// Token: 0x040047EF RID: 18415
	private AudioSource[] audioSources;

	// Token: 0x040047F0 RID: 18416
	private float[] audioVolumes;

	// Token: 0x040047F1 RID: 18417
	public string[] colorVariables = new string[]
	{
		"_Tint",
		"_Color",
		"_MainColor"
	};

	// Token: 0x040047F2 RID: 18418
	private float[] rendererDefaultAlpha;

	// Token: 0x040047F3 RID: 18419
	private string[] renderColorProperty;

	// Token: 0x040047F4 RID: 18420
	public float TargetFade = 1f;

	// Token: 0x040047F5 RID: 18421
	public int renderQueueOverride = -1;

	// Token: 0x040047F6 RID: 18422
	private bool complete;

	// Token: 0x040047F7 RID: 18423
	private Action completeAction;
}
