using System;
using UnityEngine;

// Token: 0x020008C4 RID: 2244
public class SU_Asteroid : MonoBehaviour
{
	// Token: 0x060044A0 RID: 17568 RVA: 0x0016EB38 File Offset: 0x0016CF38
	private void Start()
	{
		this._cacheTransform = base.transform;
		this.SetPolyCount(this.polyCount);
	}

	// Token: 0x060044A1 RID: 17569 RVA: 0x0016EB54 File Offset: 0x0016CF54
	private void Update()
	{
		if (this._cacheTransform != null)
		{
			this._cacheTransform.Rotate(this.rotationalAxis * this.rotationSpeed * Time.deltaTime);
			this._cacheTransform.Translate(this.driftAxis * this.driftSpeed * Time.deltaTime, Space.World);
		}
	}

	// Token: 0x060044A2 RID: 17570 RVA: 0x0016EBBF File Offset: 0x0016CFBF
	public void SetPolyCount(SU_Asteroid.PolyCount _newPolyCount)
	{
		this.SetPolyCount(_newPolyCount, false);
	}

	// Token: 0x060044A3 RID: 17571 RVA: 0x0016EBCC File Offset: 0x0016CFCC
	public void SetPolyCount(SU_Asteroid.PolyCount _newPolyCount, bool _collider)
	{
		if (!_collider)
		{
			this.polyCount = _newPolyCount;
			if (_newPolyCount != SU_Asteroid.PolyCount.LOW)
			{
				if (_newPolyCount != SU_Asteroid.PolyCount.MEDIUM)
				{
					if (_newPolyCount == SU_Asteroid.PolyCount.HIGH)
					{
						base.transform.GetComponent<MeshFilter>().sharedMesh = this.meshHighPoly.GetComponent<MeshFilter>().sharedMesh;
					}
				}
				else
				{
					base.transform.GetComponent<MeshFilter>().sharedMesh = this.meshMediumPoly.GetComponent<MeshFilter>().sharedMesh;
				}
			}
			else
			{
				base.transform.GetComponent<MeshFilter>().sharedMesh = this.meshLowPoly.GetComponent<MeshFilter>().sharedMesh;
			}
		}
		else
		{
			this.polyCountCollider = _newPolyCount;
			if (_newPolyCount != SU_Asteroid.PolyCount.LOW)
			{
				if (_newPolyCount != SU_Asteroid.PolyCount.MEDIUM)
				{
					if (_newPolyCount == SU_Asteroid.PolyCount.HIGH)
					{
						base.transform.GetComponent<MeshCollider>().sharedMesh = this.meshHighPoly.GetComponent<MeshFilter>().sharedMesh;
					}
				}
				else
				{
					base.transform.GetComponent<MeshCollider>().sharedMesh = this.meshMediumPoly.GetComponent<MeshFilter>().sharedMesh;
				}
			}
			else
			{
				base.transform.GetComponent<MeshCollider>().sharedMesh = this.meshLowPoly.GetComponent<MeshFilter>().sharedMesh;
			}
		}
	}

	// Token: 0x04002E57 RID: 11863
	public SU_Asteroid.PolyCount polyCount;

	// Token: 0x04002E58 RID: 11864
	public SU_Asteroid.PolyCount polyCountCollider = SU_Asteroid.PolyCount.LOW;

	// Token: 0x04002E59 RID: 11865
	public Transform meshLowPoly;

	// Token: 0x04002E5A RID: 11866
	public Transform meshMediumPoly;

	// Token: 0x04002E5B RID: 11867
	public Transform meshHighPoly;

	// Token: 0x04002E5C RID: 11868
	public float rotationSpeed;

	// Token: 0x04002E5D RID: 11869
	public Vector3 rotationalAxis = Vector3.up;

	// Token: 0x04002E5E RID: 11870
	public float driftSpeed;

	// Token: 0x04002E5F RID: 11871
	public Vector3 driftAxis = Vector3.up;

	// Token: 0x04002E60 RID: 11872
	private Transform _cacheTransform;

	// Token: 0x020008C5 RID: 2245
	public enum PolyCount
	{
		// Token: 0x04002E62 RID: 11874
		HIGH,
		// Token: 0x04002E63 RID: 11875
		MEDIUM,
		// Token: 0x04002E64 RID: 11876
		LOW
	}
}
