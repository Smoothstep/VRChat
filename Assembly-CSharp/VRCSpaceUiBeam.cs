using System;
using System.Collections.Generic;
using UnityEngine;
using VRCSDK2;

// Token: 0x02000C5D RID: 3165
[RequireComponent(typeof(LineRenderer))]
public class VRCSpaceUiBeam : MonoBehaviour
{
	// Token: 0x06006249 RID: 25161 RVA: 0x0022E9DC File Offset: 0x0022CDDC
	private void Awake()
	{
		this.lineRenderer = base.GetComponent<LineRenderer>();
		this.particles = base.GetComponentsInChildren<ParticleSystem>(true);
		this.baseSizes = new float[this.particles.Length];
		for (int i = 0; i < this.particles.Length; i++)
		{
			this.baseSizes[i] = this.particles[i].main.startSizeMultiplier;
		}
		if (!this.AnimateUV && this.BeamFrames.Length > 0)
		{
			this.lineRenderer.material.mainTexture = this.BeamFrames[0];
		}
		this.initialBeamOffset = UnityEngine.Random.Range(0f, 5f);
	}

	// Token: 0x0600624A RID: 25162 RVA: 0x0022EA91 File Offset: 0x0022CE91
	private void OnSpawned()
	{
		if (this.BeamFrames.Length > 1)
		{
			this.Animate();
		}
		F3DAudioController.instance.PlasmaBeamHeavyLoop(base.transform.position, base.transform.parent);
	}

	// Token: 0x0600624B RID: 25163 RVA: 0x0022EAC8 File Offset: 0x0022CEC8
	private void OnDespawned()
	{
		this.frameNo = 0;
		if (this.FrameTimerID != -1)
		{
			F3DTime.time.RemoveTimer(this.FrameTimerID);
			this.FrameTimerID = -1;
		}
		F3DAudioController.instance.PlasmaBeamHeavyClose(base.transform.position);
	}

	// Token: 0x0600624C RID: 25164 RVA: 0x0022EB14 File Offset: 0x0022CF14
	private void Raycast(float scale)
	{
		this.hitPoint = default(RaycastHit);
		Ray ray = new Ray(base.transform.position, base.transform.forward);
		float x = this.MaxBeamLength * (this.beamScale / 10f);
		if (Physics.Raycast(ray, out this.hitPoint, this.MaxBeamLength, this.layerMask))
		{
			this.beamLength = Vector3.Distance(base.transform.position, this.hitPoint.point) - this.impactPullIn * scale;
			this.lineRenderer.SetPosition(1, new Vector3(0f, 0f, this.beamLength));
			x = this.beamLength * (this.beamScale / 10f);
			if (this.rayImpact)
			{
				this.rayImpact.position = this.hitPoint.point - base.transform.forward * this.impactPullIn * scale;
			}
		}
		else
		{
			this.beamLength = this.MaxBeamLength;
			this.lineRenderer.SetPosition(1, new Vector3(0f, 0f, this.beamLength));
			if (this.rayImpact)
			{
				this.rayImpact.position = base.transform.position + base.transform.forward * this.beamLength;
			}
		}
		if (this.rayMuzzle)
		{
			this.rayMuzzle.position = base.transform.position + base.transform.forward * this.muzzleAdvance;
		}
		this.lineRenderer.material.SetTextureScale("_MainTex", new Vector2(x, 1f));
		float num = VRCTrackingManager.GetTrackingScale();
		if (num <= 0f)
		{
			num = 1f;
		}
		this.lineRenderer.widthMultiplier = num;
	}

	// Token: 0x0600624D RID: 25165 RVA: 0x0022ED20 File Offset: 0x0022D120
	private bool DoesHitTerminateNavCursor(RaycastHit hit)
	{
		VRC_Pickup component = hit.transform.GetComponent<VRC_Pickup>();
		return !(component != null);
	}

	// Token: 0x0600624E RID: 25166 RVA: 0x0022ED4C File Offset: 0x0022D14C
	public Vector3 Arccast(float scale = 1f)
	{
		List<Vector3> list = new List<Vector3>();
		list.Add(Vector3.zero);
		Vector3 vector = base.transform.position;
		Vector3 vector2 = base.transform.forward;
		this.hitPoint = default(RaycastHit);
		float x = this.MaxBeamLength * (this.beamScale / 10f);
		this.beamLength = 0f;
		bool flag = false;
		for (float num = 1f; num <= this.MaxBeamLength; num += 1f)
		{
			vector2 = (vector2 + this.arcGravity).normalized;
			Ray ray = new Ray(vector, vector2);
			RaycastHit[] array = Physics.RaycastAll(ray, 1f, this.layerMask, QueryTriggerInteraction.Ignore);
			foreach (RaycastHit hit in array)
			{
				if (this.DoesHitTerminateNavCursor(hit))
				{
					this.beamLength += hit.distance - this.impactPullIn * scale;
					list.Add(base.transform.InverseTransformPoint(hit.point));
					this.hitPoint = hit;
					flag = true;
					break;
				}
			}
			if (flag)
			{
				break;
			}
			this.beamLength += 1f;
			list.Add(base.transform.InverseTransformPoint(vector));
			vector += vector2;
		}
		this.lineRenderer.SetPosition(1, new Vector3(0f, 0f, this.beamLength));
		x = this.beamLength * (this.beamScale / 10f);
		this.lineRenderer.material.SetTextureScale("_MainTex", new Vector2(x, 1f));
		this.lineRenderer.positionCount = list.Count;
		this.lineRenderer.SetPositions(list.ToArray());
		if (this.rayImpact)
		{
			this.rayImpact.position = this.hitPoint.point;
		}
		if (this.rayMuzzle)
		{
			this.rayMuzzle.position = base.transform.position + base.transform.forward * this.muzzleAdvance;
		}
		if (!flag)
		{
			return base.transform.position + base.transform.forward * 100f;
		}
		return this.hitPoint.point;
	}

	// Token: 0x0600624F RID: 25167 RVA: 0x0022EFE0 File Offset: 0x0022D3E0
	private void OnFrameStep()
	{
		this.lineRenderer.material.mainTexture = this.BeamFrames[this.frameNo];
		this.frameNo++;
		if (this.frameNo == this.BeamFrames.Length)
		{
			this.frameNo = 0;
		}
	}

	// Token: 0x06006250 RID: 25168 RVA: 0x0022F034 File Offset: 0x0022D434
	private void Animate()
	{
		if (this.BeamFrames.Length > 1)
		{
			this.frameNo = 0;
			this.lineRenderer.material.mainTexture = this.BeamFrames[this.frameNo];
			this.FrameTimerID = F3DTime.time.AddTimer(this.FrameStep, this.BeamFrames.Length - 1, new Action(this.OnFrameStep));
			this.frameNo = 1;
		}
	}

	// Token: 0x06006251 RID: 25169 RVA: 0x0022F0A8 File Offset: 0x0022D4A8
	private void Update()
	{
		float num = VRCTrackingManager.GetTrackingScale();
		if (num <= 0f)
		{
			num = 1f;
		}
		if (!Mathf.Approximately(this.lastParticleScale, num))
		{
			for (int i = 0; i < this.particles.Length; i++)
			{
				float startSizeMultiplier = num * this.baseSizes[i];
                // HMM.
                this.particles[i].startSize = startSizeMultiplier;
				//this.particles[i].main.startSizeMultiplier = startSizeMultiplier;
			}
		}
		this.lastParticleScale = num;
		if (this.AnimateUV)
		{
			this.lineRenderer.material.SetTextureOffset("_MainTex", new Vector2(Time.time * this.UVTime + this.initialBeamOffset, 0f));
		}
		if (!this.isArc)
		{
			this.Raycast(num);
		}
	}

	// Token: 0x040047AE RID: 18350
	public LayerMask layerMask;

	// Token: 0x040047AF RID: 18351
	public Texture[] BeamFrames;

	// Token: 0x040047B0 RID: 18352
	public float FrameStep;

	// Token: 0x040047B1 RID: 18353
	public float beamScale;

	// Token: 0x040047B2 RID: 18354
	public float MaxBeamLength;

	// Token: 0x040047B3 RID: 18355
	public bool AnimateUV;

	// Token: 0x040047B4 RID: 18356
	public float UVTime;

	// Token: 0x040047B5 RID: 18357
	public Transform rayImpact;

	// Token: 0x040047B6 RID: 18358
	public Transform rayMuzzle;

	// Token: 0x040047B7 RID: 18359
	private LineRenderer lineRenderer;

	// Token: 0x040047B8 RID: 18360
	private RaycastHit hitPoint;

	// Token: 0x040047B9 RID: 18361
	private int frameNo;

	// Token: 0x040047BA RID: 18362
	private int FrameTimerID;

	// Token: 0x040047BB RID: 18363
	private float beamLength;

	// Token: 0x040047BC RID: 18364
	private float initialBeamOffset;

	// Token: 0x040047BD RID: 18365
	public float impactPullIn = 0.5f;

	// Token: 0x040047BE RID: 18366
	public float muzzleAdvance = 0.1f;

	// Token: 0x040047BF RID: 18367
	public bool isArc;

	// Token: 0x040047C0 RID: 18368
	public Vector3 arcGravity = new Vector3(0f, -0.05f, 0f);

	// Token: 0x040047C1 RID: 18369
	private float lastParticleScale;

	// Token: 0x040047C2 RID: 18370
	private ParticleSystem[] particles;

	// Token: 0x040047C3 RID: 18371
	private float[] baseSizes;
}
