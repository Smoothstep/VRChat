using System;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BD2 RID: 3026
	public class Balloon : MonoBehaviour
	{
		// Token: 0x06005DAF RID: 23983 RVA: 0x0020C508 File Offset: 0x0020A908
		private void Start()
		{
			this.destructTime = Time.time + this.lifetime + UnityEngine.Random.value;
			this.hand = base.GetComponentInParent<Hand>();
			this.balloonRigidbody = base.GetComponent<Rigidbody>();
		}

		// Token: 0x06005DB0 RID: 23984 RVA: 0x0020C53C File Offset: 0x0020A93C
		private void Update()
		{
			if (this.destructTime != 0f && Time.time > this.destructTime)
			{
				if (this.burstOnLifetimeEnd)
				{
					this.SpawnParticles(this.lifetimeEndParticlePrefab, this.lifetimeEndSound);
				}
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x06005DB1 RID: 23985 RVA: 0x0020C594 File Offset: 0x0020A994
		private void SpawnParticles(GameObject particlePrefab, SoundPlayOneshot sound)
		{
			if (this.bParticlesSpawned)
			{
				return;
			}
			this.bParticlesSpawned = true;
			if (particlePrefab != null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(particlePrefab, base.transform.position, base.transform.rotation);
				gameObject.GetComponent<ParticleSystem>().Play();
				UnityEngine.Object.Destroy(gameObject, 2f);
			}
			if (sound != null)
			{
				float num = Time.time - Balloon.s_flLastDeathSound;
				if (num < 0.1f)
				{
					sound.volMax *= 0.25f;
					sound.volMin *= 0.25f;
				}
				sound.Play();
				Balloon.s_flLastDeathSound = Time.time;
			}
		}

		// Token: 0x06005DB2 RID: 23986 RVA: 0x0020C64C File Offset: 0x0020AA4C
		private void FixedUpdate()
		{
			if (this.balloonRigidbody.velocity.sqrMagnitude > this.maxVelocity)
			{
				this.balloonRigidbody.velocity *= 0.97f;
			}
		}

		// Token: 0x06005DB3 RID: 23987 RVA: 0x0020C692 File Offset: 0x0020AA92
		private void ApplyDamage()
		{
			this.SpawnParticles(this.popPrefab, null);
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06005DB4 RID: 23988 RVA: 0x0020C6AC File Offset: 0x0020AAAC
		private void OnCollisionEnter(Collision collision)
		{
			if (this.bParticlesSpawned)
			{
				return;
			}
			Hand x = null;
			BalloonHapticBump component = collision.gameObject.GetComponent<BalloonHapticBump>();
			if (component != null && component.physParent != null)
			{
				x = component.physParent.GetComponentInParent<Hand>();
			}
			if (Time.time > this.lastSoundTime + this.soundDelay)
			{
				if (x != null)
				{
					if (Time.time > this.releaseTime + this.soundDelay)
					{
						this.collisionSound.Play();
						this.lastSoundTime = Time.time;
					}
				}
				else
				{
					this.collisionSound.Play();
					this.lastSoundTime = Time.time;
				}
			}
			if (this.destructTime > 0f)
			{
				return;
			}
			if (this.balloonRigidbody.velocity.magnitude > this.maxVelocity * 10f)
			{
				this.balloonRigidbody.velocity = this.balloonRigidbody.velocity.normalized * this.maxVelocity;
			}
			if (this.hand != null)
			{
				ushort durationMicroSec = (ushort)Mathf.Clamp(Util.RemapNumber(collision.relativeVelocity.magnitude, 0f, 3f, 500f, 800f), 500f, 800f);
				this.hand.controller.TriggerHapticPulse(durationMicroSec, EVRButtonId.k_EButton_Axis0);
			}
		}

		// Token: 0x06005DB5 RID: 23989 RVA: 0x0020C824 File Offset: 0x0020AC24
		public void SetColor(Balloon.BalloonColor color)
		{
			base.GetComponentInChildren<MeshRenderer>().material.color = this.BalloonColorToRGB(color);
		}

		// Token: 0x06005DB6 RID: 23990 RVA: 0x0020C840 File Offset: 0x0020AC40
		private Color BalloonColorToRGB(Balloon.BalloonColor balloonColorVar)
		{
			Color result = new Color(255f, 0f, 0f);
			switch (balloonColorVar)
			{
			case Balloon.BalloonColor.Red:
				return new Color(237f, 29f, 37f, 255f) / 255f;
			case Balloon.BalloonColor.OrangeRed:
				return new Color(241f, 91f, 35f, 255f) / 255f;
			case Balloon.BalloonColor.Orange:
				return new Color(245f, 140f, 31f, 255f) / 255f;
			case Balloon.BalloonColor.YellowOrange:
				return new Color(253f, 185f, 19f, 255f) / 255f;
			case Balloon.BalloonColor.Yellow:
				return new Color(254f, 243f, 0f, 255f) / 255f;
			case Balloon.BalloonColor.GreenYellow:
				return new Color(172f, 209f, 54f, 255f) / 255f;
			case Balloon.BalloonColor.Green:
				return new Color(0f, 167f, 79f, 255f) / 255f;
			case Balloon.BalloonColor.BlueGreen:
				return new Color(108f, 202f, 189f, 255f) / 255f;
			case Balloon.BalloonColor.Blue:
				return new Color(0f, 119f, 178f, 255f) / 255f;
			case Balloon.BalloonColor.VioletBlue:
				return new Color(82f, 80f, 162f, 255f) / 255f;
			case Balloon.BalloonColor.Violet:
				return new Color(102f, 46f, 143f, 255f) / 255f;
			case Balloon.BalloonColor.RedViolet:
				return new Color(182f, 36f, 102f, 255f) / 255f;
			case Balloon.BalloonColor.LightGray:
				return new Color(192f, 192f, 192f, 255f) / 255f;
			case Balloon.BalloonColor.DarkGray:
				return new Color(128f, 128f, 128f, 255f) / 255f;
			case Balloon.BalloonColor.Random:
			{
				int balloonColorVar2 = UnityEngine.Random.Range(0, 12);
				return this.BalloonColorToRGB((Balloon.BalloonColor)balloonColorVar2);
			}
			default:
				return result;
			}
		}

		// Token: 0x0400432D RID: 17197
		private Hand hand;

		// Token: 0x0400432E RID: 17198
		public GameObject popPrefab;

		// Token: 0x0400432F RID: 17199
		public float maxVelocity = 5f;

		// Token: 0x04004330 RID: 17200
		public float lifetime = 15f;

		// Token: 0x04004331 RID: 17201
		public bool burstOnLifetimeEnd;

		// Token: 0x04004332 RID: 17202
		public GameObject lifetimeEndParticlePrefab;

		// Token: 0x04004333 RID: 17203
		public SoundPlayOneshot lifetimeEndSound;

		// Token: 0x04004334 RID: 17204
		private float destructTime;

		// Token: 0x04004335 RID: 17205
		private float releaseTime = 99999f;

		// Token: 0x04004336 RID: 17206
		public SoundPlayOneshot collisionSound;

		// Token: 0x04004337 RID: 17207
		private float lastSoundTime;

		// Token: 0x04004338 RID: 17208
		private float soundDelay = 0.2f;

		// Token: 0x04004339 RID: 17209
		private Rigidbody balloonRigidbody;

		// Token: 0x0400433A RID: 17210
		private bool bParticlesSpawned;

		// Token: 0x0400433B RID: 17211
		private static float s_flLastDeathSound;

		// Token: 0x02000BD3 RID: 3027
		public enum BalloonColor
		{
			// Token: 0x0400433D RID: 17213
			Red,
			// Token: 0x0400433E RID: 17214
			OrangeRed,
			// Token: 0x0400433F RID: 17215
			Orange,
			// Token: 0x04004340 RID: 17216
			YellowOrange,
			// Token: 0x04004341 RID: 17217
			Yellow,
			// Token: 0x04004342 RID: 17218
			GreenYellow,
			// Token: 0x04004343 RID: 17219
			Green,
			// Token: 0x04004344 RID: 17220
			BlueGreen,
			// Token: 0x04004345 RID: 17221
			Blue,
			// Token: 0x04004346 RID: 17222
			VioletBlue,
			// Token: 0x04004347 RID: 17223
			Violet,
			// Token: 0x04004348 RID: 17224
			RedViolet,
			// Token: 0x04004349 RID: 17225
			LightGray,
			// Token: 0x0400434A RID: 17226
			DarkGray,
			// Token: 0x0400434B RID: 17227
			Random
		}
	}
}
