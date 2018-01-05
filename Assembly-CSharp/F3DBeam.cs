using System;
using UnityEngine;

// Token: 0x02000477 RID: 1143
[RequireComponent(typeof(LineRenderer))]
public class F3DBeam : MonoBehaviour
{
	// Token: 0x0600279B RID: 10139 RVA: 0x000CD77C File Offset: 0x000CBB7C
	private void Awake()
	{
		this.lineRenderer = base.GetComponent<LineRenderer>();
		if (!this.AnimateUV && this.BeamFrames.Length > 0)
		{
			this.lineRenderer.material.mainTexture = this.BeamFrames[0];
		}
		this.initialBeamOffset = UnityEngine.Random.Range(0f, 5f);
	}

	// Token: 0x0600279C RID: 10140 RVA: 0x000CD7DC File Offset: 0x000CBBDC
	private void OnSpawned()
	{
		if (this.OneShot)
		{
			this.Raycast();
		}
		if (this.BeamFrames.Length > 1)
		{
			this.Animate();
		}
		F3DFXType f3DFXType = this.fxType;
		if (f3DFXType != F3DFXType.PlasmaBeam)
		{
			if (f3DFXType == F3DFXType.PlasmaBeamHeavy)
			{
				F3DAudioController.instance.PlasmaBeamHeavyLoop(base.transform.position, base.transform.parent);
			}
		}
		else
		{
			F3DAudioController.instance.PlasmaBeamLoop(base.transform.position, base.transform.parent);
		}
	}

	// Token: 0x0600279D RID: 10141 RVA: 0x000CD878 File Offset: 0x000CBC78
	private void OnDespawned()
	{
		this.frameNo = 0;
		if (this.FrameTimerID != -1)
		{
			F3DTime.time.RemoveTimer(this.FrameTimerID);
			this.FrameTimerID = -1;
		}
		F3DFXType f3DFXType = this.fxType;
		if (f3DFXType != F3DFXType.PlasmaBeam)
		{
			if (f3DFXType == F3DFXType.PlasmaBeamHeavy)
			{
				F3DAudioController.instance.PlasmaBeamHeavyClose(base.transform.position);
			}
		}
		else
		{
			F3DAudioController.instance.PlasmaBeamClose(base.transform.position);
		}
	}

	// Token: 0x0600279E RID: 10142 RVA: 0x000CD904 File Offset: 0x000CBD04
	private void Raycast()
	{
		this.hitPoint = default(RaycastHit);
		Ray ray = new Ray(base.transform.position, base.transform.forward);
		float x = this.MaxBeamLength * (this.beamScale / 10f);
		if (Physics.Raycast(ray, out this.hitPoint, this.MaxBeamLength, this.layerMask))
		{
			this.beamLength = Vector3.Distance(base.transform.position, this.hitPoint.point) - this.impactPullIn;
			this.lineRenderer.SetPosition(1, new Vector3(0f, 0f, this.beamLength));
			x = this.beamLength * (this.beamScale / 10f);
			switch (this.fxType)
			{
			case F3DFXType.Sniper:
				F3DFXController.instance.SniperImpact(this.hitPoint.point + this.hitPoint.normal * 0.2f);
				this.ApplyForce(4f);
				break;
			case F3DFXType.RailGun:
				F3DFXController.instance.RailgunImpact(this.hitPoint.point + this.hitPoint.normal * 0.2f);
				this.ApplyForce(7f);
				break;
			case F3DFXType.PlasmaBeam:
				this.ApplyForce(0.5f);
				break;
			case F3DFXType.PlasmaBeamHeavy:
				this.ApplyForce(2f);
				break;
			}
			if (this.rayImpact)
			{
				this.rayImpact.position = this.hitPoint.point - base.transform.forward * this.impactPullIn;
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
	}

	// Token: 0x0600279F RID: 10143 RVA: 0x000CDBB8 File Offset: 0x000CBFB8
	private void OnFrameStep()
	{
		this.lineRenderer.material.mainTexture = this.BeamFrames[this.frameNo];
		this.frameNo++;
		if (this.frameNo == this.BeamFrames.Length)
		{
			this.frameNo = 0;
		}
	}

	// Token: 0x060027A0 RID: 10144 RVA: 0x000CDC0C File Offset: 0x000CC00C
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

	// Token: 0x060027A1 RID: 10145 RVA: 0x000CDC80 File Offset: 0x000CC080
	private void ApplyForce(float force)
	{
		if (this.hitPoint.rigidbody != null && this.ApplyPushForce)
		{
			this.hitPoint.rigidbody.AddForceAtPosition(base.transform.forward * force, this.hitPoint.point, ForceMode.VelocityChange);
		}
	}

	// Token: 0x060027A2 RID: 10146 RVA: 0x000CDCDC File Offset: 0x000CC0DC
	private void Update()
	{
		if (this.AnimateUV)
		{
			this.lineRenderer.material.SetTextureOffset("_MainTex", new Vector2(Time.time * this.UVTime + this.initialBeamOffset, 0f));
		}
		if (!this.OneShot)
		{
			this.Raycast();
		}
	}

	// Token: 0x04001597 RID: 5527
	public LayerMask layerMask;

	// Token: 0x04001598 RID: 5528
	public F3DFXType fxType;

	// Token: 0x04001599 RID: 5529
	public bool OneShot;

	// Token: 0x0400159A RID: 5530
	public Texture[] BeamFrames;

	// Token: 0x0400159B RID: 5531
	public float FrameStep;

	// Token: 0x0400159C RID: 5532
	public bool ApplyPushForce = true;

	// Token: 0x0400159D RID: 5533
	public float beamScale;

	// Token: 0x0400159E RID: 5534
	public float MaxBeamLength;

	// Token: 0x0400159F RID: 5535
	public bool AnimateUV;

	// Token: 0x040015A0 RID: 5536
	public float UVTime;

	// Token: 0x040015A1 RID: 5537
	public Transform rayImpact;

	// Token: 0x040015A2 RID: 5538
	public Transform rayMuzzle;

	// Token: 0x040015A3 RID: 5539
	private LineRenderer lineRenderer;

	// Token: 0x040015A4 RID: 5540
	private RaycastHit hitPoint;

	// Token: 0x040015A5 RID: 5541
	private int frameNo;

	// Token: 0x040015A6 RID: 5542
	private int FrameTimerID;

	// Token: 0x040015A7 RID: 5543
	private float beamLength;

	// Token: 0x040015A8 RID: 5544
	private float initialBeamOffset;

	// Token: 0x040015A9 RID: 5545
	public float impactPullIn = 0.5f;

	// Token: 0x040015AA RID: 5546
	public float muzzleAdvance = 0.1f;
}
