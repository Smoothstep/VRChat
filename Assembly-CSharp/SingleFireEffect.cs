using System;
using UnityEngine;

// Token: 0x02000AF6 RID: 2806
public class SingleFireEffect : MonoBehaviour
{
	// Token: 0x060054DE RID: 21726 RVA: 0x001D4456 File Offset: 0x001D2856
	private void Awake()
	{
		this.lifeTime = 0f;
	}

	// Token: 0x060054DF RID: 21727 RVA: 0x001D4464 File Offset: 0x001D2864
	private void Update()
	{
		this.lifeTime += Time.deltaTime;
		if (this.lifeTime >= this.deathTime)
		{
			if (this.networked)
			{
				PhotonNetwork.Destroy(base.gameObject);
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x04003BEE RID: 15342
	public float deathTime = 5f;

	// Token: 0x04003BEF RID: 15343
	public bool networked;

	// Token: 0x04003BF0 RID: 15344
	private float lifeTime;
}
