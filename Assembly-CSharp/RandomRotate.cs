using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008A5 RID: 2213
public class RandomRotate : MonoBehaviour
{
	// Token: 0x060043DA RID: 17370 RVA: 0x00166920 File Offset: 0x00164D20
	private void Start()
	{
		this.deltaTime = 1f / (float)this.fps;
		this.rangeX = (float)UnityEngine.Random.Range(0, 10);
		this.rangeY = (float)UnityEngine.Random.Range(0, 10);
		this.rangeZ = (float)UnityEngine.Random.Range(0, 10);
	}

	// Token: 0x060043DB RID: 17371 RVA: 0x0016696D File Offset: 0x00164D6D
	private void OnBecameVisible()
	{
		this.isVisible = true;
		base.StartCoroutine(this.UpdateRotation());
	}

	// Token: 0x060043DC RID: 17372 RVA: 0x00166983 File Offset: 0x00164D83
	private void OnBecameInvisible()
	{
		this.isVisible = false;
	}

	// Token: 0x060043DD RID: 17373 RVA: 0x0016698C File Offset: 0x00164D8C
	private IEnumerator UpdateRotation()
	{
		while (this.isVisible)
		{
			if (this.isRotate)
			{
				base.transform.Rotate(this.deltaTime * Mathf.Sin(Time.time + this.rangeX) * (float)this.x, this.deltaTime * Mathf.Sin(Time.time + this.rangeY) * (float)this.y, this.deltaTime * Mathf.Sin(Time.time + this.rangeZ) * (float)this.z);
			}
			yield return new WaitForSeconds(this.deltaTime);
		}
		yield break;
	}

	// Token: 0x04002CC4 RID: 11460
	public bool isRotate = true;

	// Token: 0x04002CC5 RID: 11461
	public int fps = 30;

	// Token: 0x04002CC6 RID: 11462
	public int x = 100;

	// Token: 0x04002CC7 RID: 11463
	public int y = 200;

	// Token: 0x04002CC8 RID: 11464
	public int z = 300;

	// Token: 0x04002CC9 RID: 11465
	private float rangeX;

	// Token: 0x04002CCA RID: 11466
	private float rangeY;

	// Token: 0x04002CCB RID: 11467
	private float rangeZ;

	// Token: 0x04002CCC RID: 11468
	private float deltaTime;

	// Token: 0x04002CCD RID: 11469
	private bool isVisible;
}
