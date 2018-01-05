using System;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BD8 RID: 3032
	public class FireSource : MonoBehaviour
	{
		// Token: 0x06005DC7 RID: 24007 RVA: 0x0020CFFF File Offset: 0x0020B3FF
		private void Start()
		{
			if (this.startActive)
			{
				this.StartBurning();
			}
		}

		// Token: 0x06005DC8 RID: 24008 RVA: 0x0020D014 File Offset: 0x0020B414
		private void Update()
		{
			if (this.burnTime != 0f && Time.time > this.ignitionTime + this.burnTime && this.isBurning)
			{
				this.isBurning = false;
				if (this.customParticles != null)
				{
					this.customParticles.Stop();
				}
				else
				{
					UnityEngine.Object.Destroy(this.fireObject);
				}
			}
		}

		// Token: 0x06005DC9 RID: 24009 RVA: 0x0020D086 File Offset: 0x0020B486
		private void OnTriggerEnter(Collider other)
		{
			if (this.isBurning && this.canSpreadFromThisSource)
			{
				other.SendMessageUpwards("FireExposure", SendMessageOptions.DontRequireReceiver);
			}
		}

		// Token: 0x06005DCA RID: 24010 RVA: 0x0020D0AC File Offset: 0x0020B4AC
		private void FireExposure()
		{
			if (this.fireObject == null)
			{
				base.Invoke("StartBurning", this.ignitionDelay);
			}
			if (this.hand = base.GetComponentInParent<Hand>())
			{
				this.hand.controller.TriggerHapticPulse(1000, EVRButtonId.k_EButton_Axis0);
			}
		}

		// Token: 0x06005DCB RID: 24011 RVA: 0x0020D10C File Offset: 0x0020B50C
		private void StartBurning()
		{
			this.isBurning = true;
			this.ignitionTime = Time.time;
			if (this.ignitionSound != null)
			{
				this.ignitionSound.Play();
			}
			if (this.customParticles != null)
			{
				this.customParticles.Play();
			}
			else if (this.fireParticlePrefab != null)
			{
				this.fireObject = UnityEngine.Object.Instantiate<GameObject>(this.fireParticlePrefab, base.transform.position, base.transform.rotation);
				this.fireObject.transform.parent = base.transform;
			}
		}

		// Token: 0x04004360 RID: 17248
		public GameObject fireParticlePrefab;

		// Token: 0x04004361 RID: 17249
		public bool startActive;

		// Token: 0x04004362 RID: 17250
		private GameObject fireObject;

		// Token: 0x04004363 RID: 17251
		public ParticleSystem customParticles;

		// Token: 0x04004364 RID: 17252
		public bool isBurning;

		// Token: 0x04004365 RID: 17253
		public float burnTime;

		// Token: 0x04004366 RID: 17254
		public float ignitionDelay;

		// Token: 0x04004367 RID: 17255
		private float ignitionTime;

		// Token: 0x04004368 RID: 17256
		private Hand hand;

		// Token: 0x04004369 RID: 17257
		public AudioSource ignitionSound;

		// Token: 0x0400436A RID: 17258
		public bool canSpreadFromThisSource = true;
	}
}
