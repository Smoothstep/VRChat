using System;
using UnityEngine;

// Token: 0x0200042E RID: 1070
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Glitch")]
public class CC_Glitch : CC_Base
{
	// Token: 0x170005E9 RID: 1513
	// (get) Token: 0x0600268E RID: 9870 RVA: 0x000BE20F File Offset: 0x000BC60F
	public bool IsActive
	{
		get
		{
			return this.m_Activated;
		}
	}

	// Token: 0x0600268F RID: 9871 RVA: 0x000BE217 File Offset: 0x000BC617
	protected virtual void OnEnable()
	{
		this.m_Camera = base.GetComponent<Camera>();
	}

	// Token: 0x06002690 RID: 9872 RVA: 0x000BE225 File Offset: 0x000BC625
	protected override void Start()
	{
		base.Start();
		this.m_DurationTimerEnd = UnityEngine.Random.Range(this.randomDuration.x, this.randomDuration.y);
	}

	// Token: 0x06002691 RID: 9873 RVA: 0x000BE250 File Offset: 0x000BC650
	protected virtual void Update()
	{
		if (this.m_Activated)
		{
			this.m_DurationTimer += Time.deltaTime;
			if (this.m_DurationTimer >= this.m_DurationTimerEnd)
			{
				this.m_DurationTimer = 0f;
				this.m_Activated = false;
				this.m_EveryTimerEnd = UnityEngine.Random.Range(this.randomEvery.x, this.randomEvery.y);
			}
		}
		else
		{
			this.m_EveryTimer += Time.deltaTime;
			if (this.m_EveryTimer >= this.m_EveryTimerEnd)
			{
				this.m_EveryTimer = 0f;
				this.m_Activated = true;
				this.m_DurationTimerEnd = UnityEngine.Random.Range(this.randomDuration.x, this.randomDuration.y);
			}
		}
	}

	// Token: 0x06002692 RID: 9874 RVA: 0x000BE31C File Offset: 0x000BC71C
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!this.m_Activated)
		{
			Graphics.Blit(source, destination);
			return;
		}
		if (this.mode == CC_Glitch.Mode.Interferences)
		{
			this.DoInterferences(source, destination, this.interferencesSettings);
		}
		else if (this.mode == CC_Glitch.Mode.Tearing)
		{
			this.DoTearing(source, destination, this.tearingSettings);
		}
		else
		{
			RenderTexture temporary = RenderTexture.GetTemporary(this.m_Camera.pixelWidth, this.m_Camera.pixelHeight, 0, RenderTextureFormat.ARGB32);
			this.DoTearing(source, temporary, this.tearingSettings);
			this.DoInterferences(temporary, destination, this.interferencesSettings);
			temporary.Release();
		}
	}

	// Token: 0x06002693 RID: 9875 RVA: 0x000BE3B9 File Offset: 0x000BC7B9
	protected virtual void DoInterferences(RenderTexture source, RenderTexture destination, CC_Glitch.InterferenceSettings settings)
	{
		base.material.SetVector("_Params", new Vector3(settings.speed, settings.density, settings.maxDisplacement));
		Graphics.Blit(source, destination, base.material, 0);
	}

	// Token: 0x06002694 RID: 9876 RVA: 0x000BE3F8 File Offset: 0x000BC7F8
	protected virtual void DoTearing(RenderTexture source, RenderTexture destination, CC_Glitch.TearingSettings settings)
	{
		base.material.SetVector("_Params", new Vector4(settings.speed, settings.intensity, settings.maxDisplacement, settings.yuvOffset));
		int pass = 1;
		if (settings.allowFlipping && settings.yuvColorBleeding)
		{
			pass = 4;
		}
		else if (settings.allowFlipping)
		{
			pass = 2;
		}
		else if (settings.yuvColorBleeding)
		{
			pass = 3;
		}
		Graphics.Blit(source, destination, base.material, pass);
	}

	// Token: 0x04001389 RID: 5001
	public bool randomActivation;

	// Token: 0x0400138A RID: 5002
	public Vector2 randomEvery = new Vector2(1f, 2f);

	// Token: 0x0400138B RID: 5003
	public Vector2 randomDuration = new Vector2(1f, 2f);

	// Token: 0x0400138C RID: 5004
	public CC_Glitch.Mode mode;

	// Token: 0x0400138D RID: 5005
	public CC_Glitch.InterferenceSettings interferencesSettings;

	// Token: 0x0400138E RID: 5006
	public CC_Glitch.TearingSettings tearingSettings;

	// Token: 0x0400138F RID: 5007
	protected Camera m_Camera;

	// Token: 0x04001390 RID: 5008
	protected bool m_Activated = true;

	// Token: 0x04001391 RID: 5009
	protected float m_EveryTimer;

	// Token: 0x04001392 RID: 5010
	protected float m_EveryTimerEnd;

	// Token: 0x04001393 RID: 5011
	protected float m_DurationTimer;

	// Token: 0x04001394 RID: 5012
	protected float m_DurationTimerEnd;

	// Token: 0x0200042F RID: 1071
	public enum Mode
	{
		// Token: 0x04001396 RID: 5014
		Interferences,
		// Token: 0x04001397 RID: 5015
		Tearing,
		// Token: 0x04001398 RID: 5016
		Complete
	}

	// Token: 0x02000430 RID: 1072
	[Serializable]
	public class InterferenceSettings
	{
		// Token: 0x04001399 RID: 5017
		public float speed = 10f;

		// Token: 0x0400139A RID: 5018
		public float density = 8f;

		// Token: 0x0400139B RID: 5019
		public float maxDisplacement = 2f;
	}

	// Token: 0x02000431 RID: 1073
	[Serializable]
	public class TearingSettings
	{
		// Token: 0x0400139C RID: 5020
		public float speed = 1f;

		// Token: 0x0400139D RID: 5021
		[Range(0f, 1f)]
		public float intensity = 0.25f;

		// Token: 0x0400139E RID: 5022
		[Range(0f, 0.5f)]
		public float maxDisplacement = 0.05f;

		// Token: 0x0400139F RID: 5023
		public bool allowFlipping;

		// Token: 0x040013A0 RID: 5024
		public bool yuvColorBleeding = true;

		// Token: 0x040013A1 RID: 5025
		[Range(-2f, 2f)]
		public float yuvOffset = 0.5f;
	}
}
