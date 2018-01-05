using System;
using UnityEngine;

// Token: 0x02000A52 RID: 2642
public class ApplyWorldOffset : MonoBehaviour
{
	// Token: 0x06005009 RID: 20489 RVA: 0x001B591C File Offset: 0x001B3D1C
	private void Awake()
	{
		this.offset = base.transform.localPosition;
		this.currentOffset = this.offset;
		this.offsetNorm = this.currentOffset.normalized;
		if (this.ShowMarkerObject)
		{
			GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			gameObject.transform.parent = base.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.rotation = Quaternion.identity;
			gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
		}
	}

	// Token: 0x0600500A RID: 20490 RVA: 0x001B59BC File Offset: 0x001B3DBC
	private void Update()
	{
		if (this.Animate)
		{
			this.currentOffset += Time.deltaTime * this.Speed * this.offsetNorm * this.direction;
			if (this.direction > 0f && this.currentOffset.magnitude > this.offset.magnitude)
			{
				this.direction *= -1f;
			}
			else if (this.direction < 0f && Vector3.Dot(this.currentOffset, this.offset) <= 0f)
			{
				this.direction *= -1f;
			}
		}
		else
		{
			this.currentOffset = this.offset;
		}
		if (base.transform.parent != null)
		{
			base.transform.position = base.transform.parent.position + this.currentOffset;
		}
	}

	// Token: 0x0600500B RID: 20491 RVA: 0x001B5AD3 File Offset: 0x001B3ED3
	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(base.transform.position, 0.25f);
	}

	// Token: 0x040038DE RID: 14558
	public Vector3 offset;

	// Token: 0x040038DF RID: 14559
	public bool Animate = true;

	// Token: 0x040038E0 RID: 14560
	public float Speed = 2f;

	// Token: 0x040038E1 RID: 14561
	public bool ShowMarkerObject = true;

	// Token: 0x040038E2 RID: 14562
	private Vector3 currentOffset;

	// Token: 0x040038E3 RID: 14563
	private Vector3 offsetNorm;

	// Token: 0x040038E4 RID: 14564
	private float direction = 1f;
}
