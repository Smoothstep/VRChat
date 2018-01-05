using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BE2 RID: 3042
	public class TeleportArc : MonoBehaviour
	{
		// Token: 0x06005E27 RID: 24103 RVA: 0x00210319 File Offset: 0x0020E719
		private void Start()
		{
			this.arcTimeOffset = Time.time;
		}

		// Token: 0x06005E28 RID: 24104 RVA: 0x00210328 File Offset: 0x0020E728
		private void Update()
		{
			if (this.thickness != this.prevThickness || this.segmentCount != this.prevSegmentCount)
			{
				this.CreateLineRendererObjects();
				this.prevThickness = this.thickness;
				this.prevSegmentCount = this.segmentCount;
			}
		}

		// Token: 0x06005E29 RID: 24105 RVA: 0x00210378 File Offset: 0x0020E778
		private void CreateLineRendererObjects()
		{
			if (this.arcObjectsTransfrom != null)
			{
				UnityEngine.Object.Destroy(this.arcObjectsTransfrom.gameObject);
			}
			GameObject gameObject = new GameObject("ArcObjects");
			this.arcObjectsTransfrom = gameObject.transform;
			this.arcObjectsTransfrom.SetParent(base.transform);
			this.lineRenderers = new LineRenderer[this.segmentCount];
			for (int i = 0; i < this.segmentCount; i++)
			{
				GameObject gameObject2 = new GameObject("LineRenderer_" + i);
				gameObject2.transform.SetParent(this.arcObjectsTransfrom);
				this.lineRenderers[i] = gameObject2.AddComponent<LineRenderer>();
				this.lineRenderers[i].receiveShadows = false;
				this.lineRenderers[i].reflectionProbeUsage = ReflectionProbeUsage.Off;
				this.lineRenderers[i].lightProbeUsage = LightProbeUsage.Off;
				this.lineRenderers[i].shadowCastingMode = ShadowCastingMode.Off;
				this.lineRenderers[i].material = this.material;
				this.lineRenderers[i].startWidth = this.thickness;
				this.lineRenderers[i].endWidth = this.thickness;
				this.lineRenderers[i].enabled = false;
			}
		}

		// Token: 0x06005E2A RID: 24106 RVA: 0x002104AA File Offset: 0x0020E8AA
		public void SetArcData(Vector3 position, Vector3 velocity, bool gravity, bool pointerAtBadAngle)
		{
			this.startPos = position;
			this.projectileVelocity = velocity;
			this.useGravity = gravity;
			if (this.arcInvalid && !pointerAtBadAngle)
			{
				this.arcTimeOffset = Time.time;
			}
			this.arcInvalid = pointerAtBadAngle;
		}

		// Token: 0x06005E2B RID: 24107 RVA: 0x002104E6 File Offset: 0x0020E8E6
		public void Show()
		{
			this.showArc = true;
			if (this.lineRenderers == null)
			{
				this.CreateLineRendererObjects();
			}
		}

		// Token: 0x06005E2C RID: 24108 RVA: 0x00210500 File Offset: 0x0020E900
		public void Hide()
		{
			if (this.showArc)
			{
				this.HideLineSegments(0, this.segmentCount);
			}
			this.showArc = false;
		}

		// Token: 0x06005E2D RID: 24109 RVA: 0x00210524 File Offset: 0x0020E924
		public bool DrawArc(out RaycastHit hitInfo)
		{
			float num = this.arcDuration / (float)this.segmentCount;
			float num2 = (Time.time - this.arcTimeOffset) * this.arcSpeed;
			if (num2 > num + this.segmentBreak)
			{
				this.arcTimeOffset = Time.time;
				num2 = 0f;
			}
			float num3 = num2;
			float num4 = this.FindProjectileCollision(out hitInfo);
			if (this.arcInvalid)
			{
				this.lineRenderers[0].enabled = true;
				this.lineRenderers[0].SetPosition(0, this.GetArcPositionAtTime(0f));
				this.lineRenderers[0].SetPosition(1, this.GetArcPositionAtTime((num4 >= num) ? num : num4));
				this.HideLineSegments(1, this.segmentCount);
			}
			else
			{
				int num5 = 0;
				if (num3 > this.segmentBreak)
				{
					float num6 = num2 - this.segmentBreak;
					if (num4 < num6)
					{
						num6 = num4;
					}
					this.DrawArcSegment(0, 0f, num6);
					num5 = 1;
				}
				bool flag = false;
				int i = 0;
				if (num3 < num4)
				{
					for (i = num5; i < this.segmentCount; i++)
					{
						float num7 = num3 + num;
						if (num7 >= this.arcDuration)
						{
							num7 = this.arcDuration;
							flag = true;
						}
						if (num7 >= num4)
						{
							num7 = num4;
							flag = true;
						}
						this.DrawArcSegment(i, num3, num7);
						num3 += num + this.segmentBreak;
						if (flag || num3 >= this.arcDuration || num3 >= num4)
						{
							break;
						}
					}
				}
				else
				{
					i--;
				}
				this.HideLineSegments(i + 1, this.segmentCount);
			}
			return num4 != float.MaxValue;
		}

		// Token: 0x06005E2E RID: 24110 RVA: 0x002106C7 File Offset: 0x0020EAC7
		private void DrawArcSegment(int index, float startTime, float endTime)
		{
			this.lineRenderers[index].enabled = true;
			this.lineRenderers[index].SetPosition(0, this.GetArcPositionAtTime(startTime));
			this.lineRenderers[index].SetPosition(1, this.GetArcPositionAtTime(endTime));
		}

		// Token: 0x06005E2F RID: 24111 RVA: 0x00210704 File Offset: 0x0020EB04
		public void SetColor(Color color)
		{
			for (int i = 0; i < this.segmentCount; i++)
			{
				this.lineRenderers[i].startColor = color;
				this.lineRenderers[i].endColor = color;
			}
		}

		// Token: 0x06005E30 RID: 24112 RVA: 0x00210744 File Offset: 0x0020EB44
		private float FindProjectileCollision(out RaycastHit hitInfo)
		{
			float num = this.arcDuration / (float)this.segmentCount;
			float num2 = 0f;
			hitInfo = default(RaycastHit);
			Vector3 vector = this.GetArcPositionAtTime(num2);
			for (int i = 0; i < this.segmentCount; i++)
			{
				float num3 = num2 + num;
				Vector3 arcPositionAtTime = this.GetArcPositionAtTime(num3);
				if (Physics.Linecast(vector, arcPositionAtTime, out hitInfo, this.traceLayerMask) && hitInfo.collider.GetComponent<IgnoreTeleportTrace>() == null)
				{
					Util.DrawCross(hitInfo.point, Color.red, 0.5f);
					float num4 = Vector3.Distance(vector, arcPositionAtTime);
					return num2 + num * (hitInfo.distance / num4);
				}
				num2 = num3;
				vector = arcPositionAtTime;
			}
			return float.MaxValue;
		}

		// Token: 0x06005E31 RID: 24113 RVA: 0x00210804 File Offset: 0x0020EC04
		public Vector3 GetArcPositionAtTime(float time)
		{
			Vector3 a = (!this.useGravity) ? Vector3.zero : Physics.gravity;
			return this.startPos + (this.projectileVelocity * time + 0.5f * time * time * a);
		}

		// Token: 0x06005E32 RID: 24114 RVA: 0x0021085C File Offset: 0x0020EC5C
		private void HideLineSegments(int startSegment, int endSegment)
		{
			if (this.lineRenderers != null)
			{
				for (int i = startSegment; i < endSegment; i++)
				{
					this.lineRenderers[i].enabled = false;
				}
			}
		}

		// Token: 0x040043FF RID: 17407
		public int segmentCount = 60;

		// Token: 0x04004400 RID: 17408
		public float thickness = 0.01f;

		// Token: 0x04004401 RID: 17409
		[Tooltip("The amount of time in seconds to predict the motion of the projectile.")]
		public float arcDuration = 3f;

		// Token: 0x04004402 RID: 17410
		[Tooltip("The amount of time in seconds between each segment of the projectile.")]
		public float segmentBreak = 0.025f;

		// Token: 0x04004403 RID: 17411
		[Tooltip("The speed at which the line segments of the arc move.")]
		public float arcSpeed = 0.2f;

		// Token: 0x04004404 RID: 17412
		public Material material;

		// Token: 0x04004405 RID: 17413
		[HideInInspector]
		public int traceLayerMask;

		// Token: 0x04004406 RID: 17414
		private LineRenderer[] lineRenderers;

		// Token: 0x04004407 RID: 17415
		private float arcTimeOffset;

		// Token: 0x04004408 RID: 17416
		private float prevThickness;

		// Token: 0x04004409 RID: 17417
		private int prevSegmentCount;

		// Token: 0x0400440A RID: 17418
		private bool showArc = true;

		// Token: 0x0400440B RID: 17419
		private Vector3 startPos;

		// Token: 0x0400440C RID: 17420
		private Vector3 projectileVelocity;

		// Token: 0x0400440D RID: 17421
		private bool useGravity = true;

		// Token: 0x0400440E RID: 17422
		private Transform arcObjectsTransfrom;

		// Token: 0x0400440F RID: 17423
		private bool arcInvalid;
	}
}
