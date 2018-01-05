using System;
using UnityEngine;

// Token: 0x02000478 RID: 1144
[RequireComponent(typeof(LineRenderer))]
public class F3DCurvedBeam : MonoBehaviour
{
	// Token: 0x060027A4 RID: 10148 RVA: 0x000CDD3F File Offset: 0x000CC13F
	private void Start()
	{
		this.lineRenderer = base.GetComponent<LineRenderer>();
		this.initialBeamOffset = UnityEngine.Random.Range(0f, 5f);
		this.lineRenderer.positionCount = this.curvePoints;
	}

	// Token: 0x060027A5 RID: 10149 RVA: 0x000CDD74 File Offset: 0x000CC174
	private void Update()
	{
		this.lineRenderer.material.SetTextureOffset("_MainTex", new Vector2(Time.time * this.UVTime + this.initialBeamOffset, 0f));
		float num = Vector3.Distance(base.transform.position, this.dest.position);
		this.lineRenderer.SetPosition(0, base.transform.position);
		float num2 = 3.14159274f / (float)(this.curvePoints - 1);
		for (int i = 1; i < this.curvePoints - 1; i++)
		{
			float d = num / (float)(this.curvePoints - 1) * (float)i;
			Vector3 vector = Vector3.Normalize(this.dest.position - base.transform.position) * d;
			float d2 = Mathf.Sin(num2 * (float)i) * this.curveHeight;
			vector += base.transform.up * d2;
			this.lineRenderer.SetPosition(i, base.transform.position + vector);
		}
		this.lineRenderer.SetPosition(this.curvePoints - 1, this.dest.position);
		float x = num * (this.beamScale / 10f);
		this.lineRenderer.material.SetTextureScale("_MainTex", new Vector2(x, 1f));
	}

	// Token: 0x040015AB RID: 5547
	public Transform dest;

	// Token: 0x040015AC RID: 5548
	public float beamScale;

	// Token: 0x040015AD RID: 5549
	public float UVTime;

	// Token: 0x040015AE RID: 5550
	private LineRenderer lineRenderer;

	// Token: 0x040015AF RID: 5551
	public int curvePoints;

	// Token: 0x040015B0 RID: 5552
	public float curveHeight;

	// Token: 0x040015B1 RID: 5553
	private float initialBeamOffset;
}
