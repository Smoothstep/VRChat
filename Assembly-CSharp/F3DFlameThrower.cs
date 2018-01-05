using System;
using UnityEngine;

// Token: 0x0200047C RID: 1148
public class F3DFlameThrower : MonoBehaviour
{
	// Token: 0x060027CA RID: 10186 RVA: 0x000CEE05 File Offset: 0x000CD205
	private void Start()
	{
		this.ps = base.GetComponent<ParticleSystem>();
	}

	// Token: 0x060027CB RID: 10187 RVA: 0x000CEE13 File Offset: 0x000CD213
	private void OnSpawned()
	{
		this.despawn = false;
		F3DAudioController.instance.FlameGunLoop(base.transform.position, base.transform);
		this.lightState = 1;
		this.pLight.intensity = 0f;
	}

	// Token: 0x060027CC RID: 10188 RVA: 0x000CEE4E File Offset: 0x000CD24E
	private void OnDespawned()
	{
	}

	// Token: 0x060027CD RID: 10189 RVA: 0x000CEE50 File Offset: 0x000CD250
	private void OnDespawn()
	{
		F3DPool.instance.Despawn(base.transform);
	}

	// Token: 0x060027CE RID: 10190 RVA: 0x000CEE64 File Offset: 0x000CD264
	private void Update()
	{
		if (Input.GetMouseButtonUp(0) && !this.despawn)
		{
			this.despawn = true;
			F3DTime.time.AddTimer(1f, 1, new Action(this.OnDespawn));
			this.ps.Stop();
			if (this.heat)
			{
				this.heat.Stop();
			}
			F3DAudioController.instance.FlameGunClose(base.transform.position);
			this.pLight.intensity = 0.6f;
			this.lightState = -1;
		}
		if (this.lightState == 1)
		{
			this.pLight.intensity = Mathf.Lerp(this.pLight.intensity, 0.7f, Time.deltaTime * 10f);
			if (this.pLight.intensity >= 0.5f)
			{
				this.lightState = 0;
			}
		}
		else if (this.lightState == -1)
		{
			this.pLight.intensity = Mathf.Lerp(this.pLight.intensity, -0.1f, Time.deltaTime * 10f);
			if (this.pLight.intensity <= 0f)
			{
				this.lightState = 0;
			}
		}
	}

	// Token: 0x040015E5 RID: 5605
	public Light pLight;

	// Token: 0x040015E6 RID: 5606
	public ParticleSystem heat;

	// Token: 0x040015E7 RID: 5607
	private int lightState;

	// Token: 0x040015E8 RID: 5608
	private bool despawn;

	// Token: 0x040015E9 RID: 5609
	private ParticleSystem ps;
}
