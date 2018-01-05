using System;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BD6 RID: 3030
	public class BalloonSpawner : MonoBehaviour
	{
		// Token: 0x06005DC0 RID: 24000 RVA: 0x0020CDB0 File Offset: 0x0020B1B0
		private void Start()
		{
			if (this.balloonPrefab == null)
			{
				return;
			}
			if (this.autoSpawn && this.spawnAtStartup)
			{
				this.SpawnBalloon(this.color);
				this.nextSpawnTime = UnityEngine.Random.Range(this.minSpawnTime, this.maxSpawnTime) + Time.time;
			}
		}

		// Token: 0x06005DC1 RID: 24001 RVA: 0x0020CE10 File Offset: 0x0020B210
		private void Update()
		{
			if (this.balloonPrefab == null)
			{
				return;
			}
			if (Time.time > this.nextSpawnTime && this.autoSpawn)
			{
				this.SpawnBalloon(this.color);
				this.nextSpawnTime = UnityEngine.Random.Range(this.minSpawnTime, this.maxSpawnTime) + Time.time;
			}
		}

		// Token: 0x06005DC2 RID: 24002 RVA: 0x0020CE74 File Offset: 0x0020B274
		public GameObject SpawnBalloon(Balloon.BalloonColor color = Balloon.BalloonColor.Red)
		{
			if (this.balloonPrefab == null)
			{
				return null;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.balloonPrefab, base.transform.position, base.transform.rotation);
			gameObject.transform.localScale = new Vector3(this.scale, this.scale, this.scale);
			if (this.attachBalloon)
			{
				gameObject.transform.parent = base.transform;
			}
			if (this.sendSpawnMessageToParent && base.transform.parent != null)
			{
				base.transform.parent.SendMessage("OnBalloonSpawned", gameObject, SendMessageOptions.DontRequireReceiver);
			}
			if (this.playSounds)
			{
				if (this.inflateSound != null)
				{
					this.inflateSound.Play();
				}
				if (this.stretchSound != null)
				{
					this.stretchSound.Play();
				}
			}
			gameObject.GetComponentInChildren<Balloon>().SetColor(color);
			if (this.spawnDirectionTransform != null)
			{
				gameObject.GetComponentInChildren<Rigidbody>().AddForce(this.spawnDirectionTransform.forward * this.spawnForce);
			}
			return gameObject;
		}

		// Token: 0x06005DC3 RID: 24003 RVA: 0x0020CFAE File Offset: 0x0020B3AE
		public void SpawnBalloonFromEvent(int color)
		{
			this.SpawnBalloon((Balloon.BalloonColor)color);
		}

		// Token: 0x04004351 RID: 17233
		public float minSpawnTime = 5f;

		// Token: 0x04004352 RID: 17234
		public float maxSpawnTime = 15f;

		// Token: 0x04004353 RID: 17235
		private float nextSpawnTime;

		// Token: 0x04004354 RID: 17236
		public GameObject balloonPrefab;

		// Token: 0x04004355 RID: 17237
		public bool autoSpawn = true;

		// Token: 0x04004356 RID: 17238
		public bool spawnAtStartup = true;

		// Token: 0x04004357 RID: 17239
		public bool playSounds = true;

		// Token: 0x04004358 RID: 17240
		public SoundPlayOneshot inflateSound;

		// Token: 0x04004359 RID: 17241
		public SoundPlayOneshot stretchSound;

		// Token: 0x0400435A RID: 17242
		public bool sendSpawnMessageToParent;

		// Token: 0x0400435B RID: 17243
		public float scale = 1f;

		// Token: 0x0400435C RID: 17244
		public Transform spawnDirectionTransform;

		// Token: 0x0400435D RID: 17245
		public float spawnForce;

		// Token: 0x0400435E RID: 17246
		public bool attachBalloon;

		// Token: 0x0400435F RID: 17247
		public Balloon.BalloonColor color = Balloon.BalloonColor.Random;
	}
}
