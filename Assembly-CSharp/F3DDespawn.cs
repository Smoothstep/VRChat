using System;
using UnityEngine;

// Token: 0x02000479 RID: 1145
public class F3DDespawn : MonoBehaviour
{
	// Token: 0x060027A7 RID: 10151 RVA: 0x000CDEE8 File Offset: 0x000CC2E8
	private void Awake()
	{
		this.aSrc = base.GetComponent<AudioSource>();
	}

	// Token: 0x060027A8 RID: 10152 RVA: 0x000CDEF6 File Offset: 0x000CC2F6
	public void OnSpawned()
	{
		if (!this.DespawnOnMouseUp)
		{
			F3DTime.time.AddTimer(this.DespawnDelay, 1, new Action(this.DespawnOnTimer));
		}
	}

	// Token: 0x060027A9 RID: 10153 RVA: 0x000CDF21 File Offset: 0x000CC321
	public void OnDespawned()
	{
	}

	// Token: 0x060027AA RID: 10154 RVA: 0x000CDF24 File Offset: 0x000CC324
	public void DespawnOnTimer()
	{
		if (this.aSrc != null)
		{
			if (this.aSrc.loop)
			{
				this.DespawnOnMouseUp = true;
			}
			else
			{
				this.DespawnOnMouseUp = false;
				this.Despawn();
			}
		}
		else
		{
			this.Despawn();
		}
	}

	// Token: 0x060027AB RID: 10155 RVA: 0x000CDF76 File Offset: 0x000CC376
	public void Despawn()
	{
		F3DPool.instance.Despawn(base.transform);
	}

	// Token: 0x060027AC RID: 10156 RVA: 0x000CDF88 File Offset: 0x000CC388
	private void Update()
	{
		if (Input.GetMouseButtonUp(0) && ((this.aSrc != null && this.aSrc.loop) || this.DespawnOnMouseUp))
		{
			this.Despawn();
		}
	}

	// Token: 0x040015B2 RID: 5554
	public float DespawnDelay;

	// Token: 0x040015B3 RID: 5555
	public bool DespawnOnMouseUp;

	// Token: 0x040015B4 RID: 5556
	private AudioSource aSrc;
}
