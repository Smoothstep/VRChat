using System;
using UnityEngine;

// Token: 0x0200047D RID: 1149
[RequireComponent(typeof(LineRenderer))]
public class F3DLightning : MonoBehaviour
{
	// Token: 0x060027D0 RID: 10192 RVA: 0x000CEFB0 File Offset: 0x000CD3B0
	private void Awake()
	{
		this.lineRenderer = base.GetComponent<LineRenderer>();
		if (!this.AnimateUV && this.BeamFrames.Length > 0)
		{
			this.lineRenderer.material.mainTexture = this.BeamFrames[0];
		}
		this.initialBeamOffset = UnityEngine.Random.Range(0f, 5f);
	}

	// Token: 0x060027D1 RID: 10193 RVA: 0x000CF010 File Offset: 0x000CD410
	private void OnSpawned()
	{
		if (this.BeamFrames.Length > 1)
		{
			this.Animate();
		}
		if (this.Oscillate && this.Points > 0)
		{
			this.OscillateTimerID = F3DTime.time.AddTimer(this.OscillateTime, new Action(this.OnOscillate));
		}
		if (F3DAudioController.instance)
		{
			F3DAudioController.instance.LightningGunLoop(base.transform.position, base.transform);
		}
	}

	// Token: 0x060027D2 RID: 10194 RVA: 0x000CF094 File Offset: 0x000CD494
	private void OnDespawned()
	{
		this.frameNo = 0;
		if (this.FrameTimerID != -1)
		{
			F3DTime.time.RemoveTimer(this.FrameTimerID);
			this.FrameTimerID = -1;
		}
		if (this.OscillateTimerID != -1)
		{
			F3DTime.time.RemoveTimer(this.OscillateTimerID);
			this.OscillateTimerID = -1;
		}
		if (F3DAudioController.instance)
		{
			F3DAudioController.instance.LightningGunClose(base.transform.position);
		}
	}

	// Token: 0x060027D3 RID: 10195 RVA: 0x000CF114 File Offset: 0x000CD514
	private void Raycast()
	{
		this.hitPoint = default(RaycastHit);
		Ray ray = new Ray(base.transform.position, base.transform.forward);
		float x = this.MaxBeamLength * (this.beamScale / 10f);
		if (Physics.Raycast(ray, out this.hitPoint, this.MaxBeamLength, this.layerMask))
		{
			this.beamLength = Vector3.Distance(base.transform.position, this.hitPoint.point);
			if (!this.Oscillate)
			{
				this.lineRenderer.SetPosition(1, new Vector3(0f, 0f, this.beamLength));
			}
			x = this.beamLength * (this.beamScale / 10f);
			this.ApplyForce(0.1f);
			if (this.rayImpact)
			{
				this.rayImpact.position = this.hitPoint.point - base.transform.forward * 0.5f;
			}
		}
		else
		{
			this.beamLength = this.MaxBeamLength;
			if (!this.Oscillate)
			{
				this.lineRenderer.SetPosition(1, new Vector3(0f, 0f, this.beamLength));
			}
			if (this.rayImpact)
			{
				this.rayImpact.position = base.transform.position + base.transform.forward * this.beamLength;
			}
		}
		if (this.rayMuzzle)
		{
			this.rayMuzzle.position = base.transform.position + base.transform.forward * 0.1f;
		}
		this.lineRenderer.material.SetTextureScale("_MainTex", new Vector2(x, 1f));
	}

	// Token: 0x060027D4 RID: 10196 RVA: 0x000CF30D File Offset: 0x000CD70D
	private float GetRandomNoise()
	{
		return UnityEngine.Random.Range(-this.Amplitude, this.Amplitude);
	}

	// Token: 0x060027D5 RID: 10197 RVA: 0x000CF324 File Offset: 0x000CD724
	private void OnFrameStep()
	{
		if (this.RandomizeFrames)
		{
			this.frameNo = UnityEngine.Random.Range(0, this.BeamFrames.Length);
		}
		this.lineRenderer.material.mainTexture = this.BeamFrames[this.frameNo];
		this.frameNo++;
		if (this.frameNo == this.BeamFrames.Length)
		{
			this.frameNo = 0;
		}
	}

	// Token: 0x060027D6 RID: 10198 RVA: 0x000CF398 File Offset: 0x000CD798
	private void OnOscillate()
	{
		int num = (int)(this.beamLength / 10f * (float)this.Points);
		if (num < 2)
		{
			this.lineRenderer.positionCount = 2;
			this.lineRenderer.SetPosition(0, Vector3.zero);
			this.lineRenderer.SetPosition(1, new Vector3(0f, 0f, this.beamLength));
		}
		else
		{
			this.lineRenderer.positionCount = num;
			this.lineRenderer.SetPosition(0, Vector3.zero);
			for (int i = 1; i < num - 1; i++)
			{
				this.lineRenderer.SetPosition(i, new Vector3(this.GetRandomNoise(), this.GetRandomNoise(), this.beamLength / (float)(num - 1) * (float)i));
			}
			this.lineRenderer.SetPosition(num - 1, new Vector3(0f, 0f, this.beamLength));
		}
	}

	// Token: 0x060027D7 RID: 10199 RVA: 0x000CF484 File Offset: 0x000CD884
	private void Animate()
	{
		this.frameNo = 0;
		this.lineRenderer.material.mainTexture = this.BeamFrames[this.frameNo];
		this.FrameTimerID = F3DTime.time.AddTimer(this.FrameStep, new Action(this.OnFrameStep));
		this.frameNo = 1;
	}

	// Token: 0x060027D8 RID: 10200 RVA: 0x000CF4E0 File Offset: 0x000CD8E0
	private void ApplyForce(float force)
	{
		if (this.hitPoint.rigidbody != null)
		{
			this.hitPoint.rigidbody.AddForceAtPosition(base.transform.forward * force, this.hitPoint.point, ForceMode.VelocityChange);
		}
	}

	// Token: 0x060027D9 RID: 10201 RVA: 0x000CF530 File Offset: 0x000CD930
	private void Update()
	{
		if (this.AnimateUV)
		{
			this.lineRenderer.material.SetTextureOffset("_MainTex", new Vector2(Time.time * this.UVTime + this.initialBeamOffset, 0f));
		}
		this.Raycast();
	}

	// Token: 0x040015EA RID: 5610
	public LayerMask layerMask;

	// Token: 0x040015EB RID: 5611
	public Texture[] BeamFrames;

	// Token: 0x040015EC RID: 5612
	public float FrameStep;

	// Token: 0x040015ED RID: 5613
	public bool RandomizeFrames;

	// Token: 0x040015EE RID: 5614
	public int Points;

	// Token: 0x040015EF RID: 5615
	public float MaxBeamLength;

	// Token: 0x040015F0 RID: 5616
	public float beamScale;

	// Token: 0x040015F1 RID: 5617
	public bool AnimateUV;

	// Token: 0x040015F2 RID: 5618
	public float UVTime;

	// Token: 0x040015F3 RID: 5619
	public bool Oscillate;

	// Token: 0x040015F4 RID: 5620
	public float Amplitude;

	// Token: 0x040015F5 RID: 5621
	public float OscillateTime;

	// Token: 0x040015F6 RID: 5622
	public Transform rayImpact;

	// Token: 0x040015F7 RID: 5623
	public Transform rayMuzzle;

	// Token: 0x040015F8 RID: 5624
	private LineRenderer lineRenderer;

	// Token: 0x040015F9 RID: 5625
	private RaycastHit hitPoint;

	// Token: 0x040015FA RID: 5626
	private int frameNo;

	// Token: 0x040015FB RID: 5627
	private int FrameTimerID;

	// Token: 0x040015FC RID: 5628
	private int OscillateTimerID;

	// Token: 0x040015FD RID: 5629
	private float beamLength;

	// Token: 0x040015FE RID: 5630
	private float initialBeamOffset;
}
