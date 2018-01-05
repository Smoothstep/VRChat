using System;
using UnityEngine;

// Token: 0x02000476 RID: 1142
public class F3DAudioController : MonoBehaviour
{
	// Token: 0x06002780 RID: 10112 RVA: 0x000CC8E0 File Offset: 0x000CACE0
	private void Awake()
	{
		F3DAudioController.instance = this;
	}

	// Token: 0x06002781 RID: 10113 RVA: 0x000CC8E8 File Offset: 0x000CACE8
	private void Update()
	{
		this.timer_01 += Time.deltaTime;
		this.timer_02 += Time.deltaTime;
	}

	// Token: 0x06002782 RID: 10114 RVA: 0x000CC910 File Offset: 0x000CAD10
	public void VulcanShot(Vector3 pos)
	{
		if (this.timer_01 >= this.vulcanDelay)
		{
			AudioSource audioSource = F3DPool.instance.SpawnAudio(this.vulcanShot, pos, null);
			if (audioSource != null)
			{
				audioSource.pitch = UnityEngine.Random.Range(0.95f, 1f);
				audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.minDistance = 5f;
				audioSource.loop = false;
				audioSource.Play();
				this.timer_01 = 0f;
			}
		}
	}

	// Token: 0x06002783 RID: 10115 RVA: 0x000CC99C File Offset: 0x000CAD9C
	public void VulcanHit(Vector3 pos)
	{
		if (this.timer_02 >= this.vulcanHitDelay)
		{
			AudioSource audioSource = F3DPool.instance.SpawnAudio(this.vulcanHit[UnityEngine.Random.Range(0, this.vulcanHit.Length)], pos, null);
			if (audioSource != null)
			{
				audioSource.pitch = UnityEngine.Random.Range(0.95f, 1f);
				audioSource.volume = UnityEngine.Random.Range(0.6f, 1f);
				audioSource.minDistance = 7f;
				audioSource.loop = false;
				audioSource.Play();
				this.timer_02 = 0f;
			}
		}
	}

	// Token: 0x06002784 RID: 10116 RVA: 0x000CCA38 File Offset: 0x000CAE38
	public void SoloGunShot(Vector3 pos)
	{
		if (this.timer_01 >= this.soloGunDelay)
		{
			AudioSource audioSource = F3DPool.instance.SpawnAudio(this.soloGunShot, pos, null);
			if (audioSource != null)
			{
				audioSource.pitch = UnityEngine.Random.Range(0.95f, 1f);
				audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.minDistance = 30f;
				audioSource.loop = false;
				audioSource.Play();
				this.timer_01 = 0f;
			}
		}
	}

	// Token: 0x06002785 RID: 10117 RVA: 0x000CCAC4 File Offset: 0x000CAEC4
	public void SoloGunHit(Vector3 pos)
	{
		if (this.timer_02 >= this.soloGunHitDelay)
		{
			AudioSource audioSource = F3DPool.instance.SpawnAudio(this.soloGunHit[UnityEngine.Random.Range(0, this.soloGunHit.Length)], pos, null);
			if (audioSource != null)
			{
				audioSource.pitch = UnityEngine.Random.Range(0.95f, 1f);
				audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.minDistance = 50f;
				audioSource.loop = false;
				audioSource.Play();
				this.timer_02 = 0f;
			}
		}
	}

	// Token: 0x06002786 RID: 10118 RVA: 0x000CCB60 File Offset: 0x000CAF60
	public void SniperShot(Vector3 pos)
	{
		if (this.timer_01 >= this.sniperDelay)
		{
			AudioSource audioSource = F3DPool.instance.SpawnAudio(this.sniperShot, pos, null);
			if (audioSource != null)
			{
				audioSource.pitch = UnityEngine.Random.Range(0.9f, 1f);
				audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.minDistance = 6f;
				audioSource.loop = false;
				audioSource.Play();
				this.timer_01 = 0f;
			}
		}
	}

	// Token: 0x06002787 RID: 10119 RVA: 0x000CCBEC File Offset: 0x000CAFEC
	public void SniperHit(Vector3 pos)
	{
		if (this.timer_02 >= this.sniperHitDelay)
		{
			AudioSource audioSource = F3DPool.instance.SpawnAudio(this.sniperHit[UnityEngine.Random.Range(0, this.sniperHit.Length)], pos, null);
			if (audioSource != null)
			{
				audioSource.pitch = UnityEngine.Random.Range(0.9f, 1f);
				audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.minDistance = 8f;
				audioSource.loop = false;
				audioSource.Play();
				this.timer_02 = 0f;
			}
		}
	}

	// Token: 0x06002788 RID: 10120 RVA: 0x000CCC88 File Offset: 0x000CB088
	public void ShotGunShot(Vector3 pos)
	{
		if (this.timer_01 >= this.shotGunDelay)
		{
			AudioSource audioSource = F3DPool.instance.SpawnAudio(this.shotGunShot, pos, null);
			if (audioSource != null)
			{
				audioSource.pitch = UnityEngine.Random.Range(0.9f, 1f);
				audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.minDistance = 8f;
				audioSource.loop = false;
				audioSource.Play();
				this.timer_01 = 0f;
			}
		}
	}

	// Token: 0x06002789 RID: 10121 RVA: 0x000CCD14 File Offset: 0x000CB114
	public void ShotGunHit(Vector3 pos)
	{
		if (this.timer_02 >= this.shotGunHitDelay)
		{
			AudioSource audioSource = F3DPool.instance.SpawnAudio(this.shotGunHit[UnityEngine.Random.Range(0, this.shotGunHit.Length)], pos, null);
			if (audioSource != null)
			{
				audioSource.pitch = UnityEngine.Random.Range(0.9f, 1f);
				audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.minDistance = 7f;
				audioSource.loop = false;
				audioSource.Play();
				this.timer_02 = 0f;
			}
		}
	}

	// Token: 0x0600278A RID: 10122 RVA: 0x000CCDB0 File Offset: 0x000CB1B0
	public void SeekerShot(Vector3 pos)
	{
		if (this.timer_01 >= this.seekerDelay)
		{
			AudioSource audioSource = F3DPool.instance.SpawnAudio(this.seekerShot, pos, null);
			if (audioSource != null)
			{
				audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.minDistance = 8f;
				audioSource.loop = false;
				audioSource.Play();
				this.timer_01 = 0f;
			}
		}
	}

	// Token: 0x0600278B RID: 10123 RVA: 0x000CCE3C File Offset: 0x000CB23C
	public void SeekerHit(Vector3 pos)
	{
		if (this.timer_02 >= this.seekerHitDelay)
		{
			AudioSource audioSource = F3DPool.instance.SpawnAudio(this.seekerHit[UnityEngine.Random.Range(0, this.seekerHit.Length)], pos, null);
			if (audioSource != null)
			{
				audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.minDistance = 25f;
				audioSource.loop = false;
				audioSource.Play();
				this.timer_02 = 0f;
			}
		}
	}

	// Token: 0x0600278C RID: 10124 RVA: 0x000CCED8 File Offset: 0x000CB2D8
	public void RailGunShot(Vector3 pos)
	{
		if (this.timer_01 >= this.railgunDelay)
		{
			AudioSource audioSource = F3DPool.instance.SpawnAudio(this.railgunShot, pos, null);
			if (audioSource != null)
			{
				audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.minDistance = 4f;
				audioSource.loop = false;
				audioSource.Play();
				this.timer_01 = 0f;
			}
		}
	}

	// Token: 0x0600278D RID: 10125 RVA: 0x000CCF64 File Offset: 0x000CB364
	public void RailGunHit(Vector3 pos)
	{
		if (this.timer_02 >= this.railgunHitDelay)
		{
			AudioSource audioSource = F3DPool.instance.SpawnAudio(this.railgunHit[UnityEngine.Random.Range(0, this.railgunHit.Length)], pos, null);
			if (audioSource != null)
			{
				audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.minDistance = 20f;
				audioSource.loop = false;
				audioSource.Play();
				this.timer_02 = 0f;
			}
		}
	}

	// Token: 0x0600278E RID: 10126 RVA: 0x000CD000 File Offset: 0x000CB400
	public void PlasmaGunShot(Vector3 pos)
	{
		if (this.timer_01 >= this.plasmagunDelay)
		{
			AudioSource audioSource = F3DPool.instance.SpawnAudio(this.plasmagunShot, pos, null);
			if (audioSource != null)
			{
				audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.minDistance = 4f;
				audioSource.loop = false;
				audioSource.Play();
				this.timer_01 = 0f;
			}
		}
	}

	// Token: 0x0600278F RID: 10127 RVA: 0x000CD08C File Offset: 0x000CB48C
	public void PlasmaGunHit(Vector3 pos)
	{
		if (this.timer_02 >= this.plasmagunHitDelay)
		{
			AudioSource audioSource = F3DPool.instance.SpawnAudio(this.plasmagunHit[UnityEngine.Random.Range(0, this.plasmagunHit.Length)], pos, null);
			if (audioSource != null)
			{
				audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.minDistance = 50f;
				audioSource.loop = false;
				audioSource.Play();
				this.timer_02 = 0f;
			}
		}
	}

	// Token: 0x06002790 RID: 10128 RVA: 0x000CD128 File Offset: 0x000CB528
	public void PlasmaBeamLoop(Vector3 pos, Transform loopParent)
	{
		AudioSource audioSource = F3DPool.instance.SpawnAudio(this.plasmabeamOpen, pos, null);
		AudioSource audioSource2 = F3DPool.instance.SpawnAudio(this.plasmabeamLoop, pos, loopParent);
		if (audioSource != null && audioSource2 != null)
		{
			audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
			audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
			audioSource.minDistance = 50f;
			audioSource.loop = false;
			audioSource.Play();
			audioSource2.pitch = UnityEngine.Random.Range(0.95f, 1f);
			audioSource2.volume = UnityEngine.Random.Range(0.95f, 1f);
			audioSource2.loop = true;
			audioSource2.minDistance = 50f;
			audioSource2.Play();
		}
	}

	// Token: 0x06002791 RID: 10129 RVA: 0x000CD1F8 File Offset: 0x000CB5F8
	public void PlasmaBeamClose(Vector3 pos)
	{
		AudioSource audioSource = F3DPool.instance.SpawnAudio(this.plasmabeamClose, pos, null);
		if (audioSource != null)
		{
			audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
			audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
			audioSource.minDistance = 50f;
			audioSource.loop = false;
			audioSource.Play();
		}
	}

	// Token: 0x06002792 RID: 10130 RVA: 0x000CD268 File Offset: 0x000CB668
	public void PlasmaBeamHeavyLoop(Vector3 pos, Transform loopParent)
	{
		AudioSource audioSource = F3DPool.instance.SpawnAudio(this.plasmabeamHeavyOpen, pos, null);
		AudioSource audioSource2 = F3DPool.instance.SpawnAudio(this.plasmabeamHeavyLoop, pos, loopParent);
		if (audioSource != null && audioSource2 != null)
		{
			audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
			audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
			audioSource.minDistance = 50f;
			audioSource.loop = false;
			audioSource.Play();
			audioSource2.pitch = UnityEngine.Random.Range(0.95f, 1f);
			audioSource2.volume = UnityEngine.Random.Range(0.95f, 1f);
			audioSource2.loop = true;
			audioSource2.minDistance = 50f;
			audioSource2.Play();
		}
	}

	// Token: 0x06002793 RID: 10131 RVA: 0x000CD338 File Offset: 0x000CB738
	public void PlasmaBeamHeavyClose(Vector3 pos)
	{
		AudioSource audioSource = F3DPool.instance.SpawnAudio(this.plasmabeamHeavyClose, pos, null);
		if (audioSource != null)
		{
			audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
			audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
			audioSource.minDistance = 50f;
			audioSource.loop = false;
			audioSource.Play();
		}
	}

	// Token: 0x06002794 RID: 10132 RVA: 0x000CD3A8 File Offset: 0x000CB7A8
	public void LightningGunLoop(Vector3 pos, Transform loopParent)
	{
		AudioSource audioSource = F3DPool.instance.SpawnAudio(this.lightningGunOpen, pos, null);
		AudioSource audioSource2 = F3DPool.instance.SpawnAudio(this.lightningGunLoop, pos, loopParent.parent);
		if (audioSource != null && audioSource2 != null)
		{
			audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
			audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
			audioSource.minDistance = 50f;
			audioSource.loop = false;
			audioSource.Play();
			audioSource2.pitch = UnityEngine.Random.Range(0.95f, 1f);
			audioSource2.volume = UnityEngine.Random.Range(0.95f, 1f);
			audioSource2.loop = true;
			audioSource2.minDistance = 50f;
			audioSource2.Play();
		}
	}

	// Token: 0x06002795 RID: 10133 RVA: 0x000CD47C File Offset: 0x000CB87C
	public void LightningGunClose(Vector3 pos)
	{
		AudioSource audioSource = F3DPool.instance.SpawnAudio(this.lightningGunClose, pos, null);
		if (audioSource != null)
		{
			audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
			audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
			audioSource.minDistance = 50f;
			audioSource.loop = false;
			audioSource.Play();
		}
	}

	// Token: 0x06002796 RID: 10134 RVA: 0x000CD4EC File Offset: 0x000CB8EC
	public void FlameGunLoop(Vector3 pos, Transform loopParent)
	{
		AudioSource audioSource = F3DPool.instance.SpawnAudio(this.flameGunOpen, pos, null);
		AudioSource audioSource2 = F3DPool.instance.SpawnAudio(this.flameGunLoop, pos, loopParent.parent);
		if (audioSource != null && audioSource2 != null)
		{
			audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
			audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
			audioSource.minDistance = 50f;
			audioSource.loop = false;
			audioSource.Play();
			audioSource2.pitch = UnityEngine.Random.Range(0.95f, 1f);
			audioSource2.volume = UnityEngine.Random.Range(0.95f, 1f);
			audioSource2.loop = true;
			audioSource2.minDistance = 50f;
			audioSource2.Play();
		}
	}

	// Token: 0x06002797 RID: 10135 RVA: 0x000CD5C0 File Offset: 0x000CB9C0
	public void FlameGunClose(Vector3 pos)
	{
		AudioSource audioSource = F3DPool.instance.SpawnAudio(this.flameGunClose, pos, null);
		if (audioSource != null)
		{
			audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
			audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
			audioSource.minDistance = 50f;
			audioSource.loop = false;
			audioSource.Play();
		}
	}

	// Token: 0x06002798 RID: 10136 RVA: 0x000CD630 File Offset: 0x000CBA30
	public void LaserImpulseShot(Vector3 pos)
	{
		if (this.timer_01 >= this.laserImpulseDelay)
		{
			AudioSource audioSource = F3DPool.instance.SpawnAudio(this.laserImpulseShot, pos, null);
			if (audioSource != null)
			{
				audioSource.pitch = UnityEngine.Random.Range(0.9f, 1f);
				audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.minDistance = 20f;
				audioSource.loop = false;
				audioSource.Play();
				this.timer_01 = 0f;
			}
		}
	}

	// Token: 0x06002799 RID: 10137 RVA: 0x000CD6BC File Offset: 0x000CBABC
	public void LaserImpulseHit(Vector3 pos)
	{
		if (this.timer_02 >= this.laserImpulseHitDelay)
		{
			AudioSource audioSource = F3DPool.instance.SpawnAudio(this.laserImpulseHit[UnityEngine.Random.Range(0, this.plasmagunHit.Length)], pos, null);
			if (audioSource != null)
			{
				audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.minDistance = 20f;
				audioSource.loop = false;
				audioSource.Play();
				this.timer_02 = 0f;
			}
		}
	}

	// Token: 0x04001568 RID: 5480
	public static F3DAudioController instance;

	// Token: 0x04001569 RID: 5481
	private float timer_01;

	// Token: 0x0400156A RID: 5482
	private float timer_02;

	// Token: 0x0400156B RID: 5483
	[Header("Vulcan")]
	public AudioClip[] vulcanHit;

	// Token: 0x0400156C RID: 5484
	public AudioClip vulcanShot;

	// Token: 0x0400156D RID: 5485
	public float vulcanDelay;

	// Token: 0x0400156E RID: 5486
	public float vulcanHitDelay;

	// Token: 0x0400156F RID: 5487
	[Header("Solo gun")]
	public AudioClip[] soloGunHit;

	// Token: 0x04001570 RID: 5488
	public AudioClip soloGunShot;

	// Token: 0x04001571 RID: 5489
	public float soloGunDelay;

	// Token: 0x04001572 RID: 5490
	public float soloGunHitDelay;

	// Token: 0x04001573 RID: 5491
	[Header("Sniper")]
	public AudioClip[] sniperHit;

	// Token: 0x04001574 RID: 5492
	public AudioClip sniperShot;

	// Token: 0x04001575 RID: 5493
	public float sniperDelay;

	// Token: 0x04001576 RID: 5494
	public float sniperHitDelay;

	// Token: 0x04001577 RID: 5495
	[Header("Shot gun")]
	public AudioClip[] shotGunHit;

	// Token: 0x04001578 RID: 5496
	public AudioClip shotGunShot;

	// Token: 0x04001579 RID: 5497
	public float shotGunDelay;

	// Token: 0x0400157A RID: 5498
	public float shotGunHitDelay;

	// Token: 0x0400157B RID: 5499
	[Header("Seeker")]
	public AudioClip[] seekerHit;

	// Token: 0x0400157C RID: 5500
	public AudioClip seekerShot;

	// Token: 0x0400157D RID: 5501
	public float seekerDelay;

	// Token: 0x0400157E RID: 5502
	public float seekerHitDelay;

	// Token: 0x0400157F RID: 5503
	[Header("Rail gun")]
	public AudioClip[] railgunHit;

	// Token: 0x04001580 RID: 5504
	public AudioClip railgunShot;

	// Token: 0x04001581 RID: 5505
	public float railgunDelay;

	// Token: 0x04001582 RID: 5506
	public float railgunHitDelay;

	// Token: 0x04001583 RID: 5507
	[Header("Plasma gun")]
	public AudioClip[] plasmagunHit;

	// Token: 0x04001584 RID: 5508
	public AudioClip plasmagunShot;

	// Token: 0x04001585 RID: 5509
	public float plasmagunDelay;

	// Token: 0x04001586 RID: 5510
	public float plasmagunHitDelay;

	// Token: 0x04001587 RID: 5511
	[Header("Plasma beam")]
	public AudioClip plasmabeamOpen;

	// Token: 0x04001588 RID: 5512
	public AudioClip plasmabeamLoop;

	// Token: 0x04001589 RID: 5513
	public AudioClip plasmabeamClose;

	// Token: 0x0400158A RID: 5514
	[Header("Plasma beam heavy")]
	public AudioClip plasmabeamHeavyOpen;

	// Token: 0x0400158B RID: 5515
	public AudioClip plasmabeamHeavyLoop;

	// Token: 0x0400158C RID: 5516
	public AudioClip plasmabeamHeavyClose;

	// Token: 0x0400158D RID: 5517
	[Header("Lightning gun")]
	public AudioClip lightningGunOpen;

	// Token: 0x0400158E RID: 5518
	public AudioClip lightningGunLoop;

	// Token: 0x0400158F RID: 5519
	public AudioClip lightningGunClose;

	// Token: 0x04001590 RID: 5520
	[Header("Flame gun")]
	public AudioClip flameGunOpen;

	// Token: 0x04001591 RID: 5521
	public AudioClip flameGunLoop;

	// Token: 0x04001592 RID: 5522
	public AudioClip flameGunClose;

	// Token: 0x04001593 RID: 5523
	[Header("Laser impulse")]
	public AudioClip[] laserImpulseHit;

	// Token: 0x04001594 RID: 5524
	public AudioClip laserImpulseShot;

	// Token: 0x04001595 RID: 5525
	public float laserImpulseDelay;

	// Token: 0x04001596 RID: 5526
	public float laserImpulseHitDelay;
}
