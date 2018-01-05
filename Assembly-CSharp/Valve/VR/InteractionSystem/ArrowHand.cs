using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BD0 RID: 3024
	public class ArrowHand : MonoBehaviour
	{
		// Token: 0x06005DA1 RID: 23969 RVA: 0x0020BCE2 File Offset: 0x0020A0E2
		private void Awake()
		{
			this.allowTeleport = base.GetComponent<AllowTeleportWhileAttachedToHand>();
			this.allowTeleport.teleportAllowed = true;
			this.allowTeleport.overrideHoverLock = false;
			this.arrowList = new List<GameObject>();
		}

		// Token: 0x06005DA2 RID: 23970 RVA: 0x0020BD13 File Offset: 0x0020A113
		private void OnAttachedToHand(Hand attachedHand)
		{
			this.hand = attachedHand;
			this.FindBow();
		}

		// Token: 0x06005DA3 RID: 23971 RVA: 0x0020BD24 File Offset: 0x0020A124
		private GameObject InstantiateArrow()
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.arrowPrefab, this.arrowNockTransform.position, this.arrowNockTransform.rotation);
			gameObject.name = "Bow Arrow";
			gameObject.transform.parent = this.arrowNockTransform;
			Util.ResetTransform(gameObject.transform, true);
			this.arrowList.Add(gameObject);
			while (this.arrowList.Count > this.maxArrowCount)
			{
				GameObject gameObject2 = this.arrowList[0];
				this.arrowList.RemoveAt(0);
				if (gameObject2)
				{
					UnityEngine.Object.Destroy(gameObject2);
				}
			}
			return gameObject;
		}

		// Token: 0x06005DA4 RID: 23972 RVA: 0x0020BDD0 File Offset: 0x0020A1D0
		private void HandAttachedUpdate(Hand hand)
		{
			if (this.bow == null)
			{
				this.FindBow();
			}
			if (this.bow == null)
			{
				return;
			}
			if (this.allowArrowSpawn && this.currentArrow == null)
			{
				this.currentArrow = this.InstantiateArrow();
				this.arrowSpawnSound.Play();
			}
			float num = Vector3.Distance(base.transform.parent.position, this.bow.nockTransform.position);
			if (!this.nocked)
			{
				if (num < this.rotationLerpThreshold)
				{
					float t = Util.RemapNumber(num, this.rotationLerpThreshold, this.lerpCompleteDistance, 0f, 1f);
					this.arrowNockTransform.rotation = Quaternion.Lerp(this.arrowNockTransform.parent.rotation, this.bow.nockRestTransform.rotation, t);
				}
				else
				{
					this.arrowNockTransform.localRotation = Quaternion.identity;
				}
				if (num < this.positionLerpThreshold)
				{
					float num2 = Util.RemapNumber(num, this.positionLerpThreshold, this.lerpCompleteDistance, 0f, 1f);
					num2 = Mathf.Clamp(num2, 0f, 1f);
					this.arrowNockTransform.position = Vector3.Lerp(this.arrowNockTransform.parent.position, this.bow.nockRestTransform.position, num2);
				}
				else
				{
					this.arrowNockTransform.position = this.arrowNockTransform.parent.position;
				}
				if (num < this.lerpCompleteDistance)
				{
					if (!this.arrowLerpComplete)
					{
						this.arrowLerpComplete = true;
						hand.controller.TriggerHapticPulse(500, EVRButtonId.k_EButton_Axis0);
					}
				}
				else if (this.arrowLerpComplete)
				{
					this.arrowLerpComplete = false;
				}
				if (num < this.nockDistance)
				{
					if (!this.inNockRange)
					{
						this.inNockRange = true;
						this.bow.ArrowInPosition();
					}
				}
				else if (this.inNockRange)
				{
					this.inNockRange = false;
				}
				if (num < this.nockDistance && hand.controller.GetPress(8589934592UL) && !this.nocked)
				{
					if (this.currentArrow == null)
					{
						this.currentArrow = this.InstantiateArrow();
					}
					this.nocked = true;
					this.bow.StartNock(this);
					hand.HoverLock(base.GetComponent<Interactable>());
					this.allowTeleport.teleportAllowed = false;
					this.currentArrow.transform.parent = this.bow.nockTransform;
					Util.ResetTransform(this.currentArrow.transform, true);
					Util.ResetTransform(this.arrowNockTransform, true);
				}
			}
			if (this.nocked && (!hand.controller.GetPress(8589934592UL) || hand.controller.GetPressUp(8589934592UL)))
			{
				if (this.bow.pulled)
				{
					this.FireArrow();
				}
				else
				{
					this.arrowNockTransform.rotation = this.currentArrow.transform.rotation;
					this.currentArrow.transform.parent = this.arrowNockTransform;
					Util.ResetTransform(this.currentArrow.transform, true);
					this.nocked = false;
					this.bow.ReleaseNock();
					hand.HoverUnlock(base.GetComponent<Interactable>());
					this.allowTeleport.teleportAllowed = true;
				}
				this.bow.StartRotationLerp();
			}
		}

		// Token: 0x06005DA5 RID: 23973 RVA: 0x0020C16A File Offset: 0x0020A56A
		private void OnDetachedFromHand(Hand hand)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06005DA6 RID: 23974 RVA: 0x0020C178 File Offset: 0x0020A578
		private void FireArrow()
		{
			this.currentArrow.transform.parent = null;
			Arrow component = this.currentArrow.GetComponent<Arrow>();
			component.shaftRB.isKinematic = false;
			component.shaftRB.useGravity = true;
			component.shaftRB.transform.GetComponent<BoxCollider>().enabled = true;
			component.arrowHeadRB.isKinematic = false;
			component.arrowHeadRB.useGravity = true;
			component.arrowHeadRB.transform.GetComponent<BoxCollider>().enabled = true;
			component.arrowHeadRB.AddForce(this.currentArrow.transform.forward * this.bow.GetArrowVelocity(), ForceMode.VelocityChange);
			component.arrowHeadRB.AddTorque(this.currentArrow.transform.forward * 10f);
			this.nocked = false;
			this.currentArrow.GetComponent<Arrow>().ArrowReleased(this.bow.GetArrowVelocity());
			this.bow.ArrowReleased();
			this.allowArrowSpawn = false;
			base.Invoke("EnableArrowSpawn", 0.5f);
			base.StartCoroutine(this.ArrowReleaseHaptics());
			this.currentArrow = null;
			this.allowTeleport.teleportAllowed = true;
		}

		// Token: 0x06005DA7 RID: 23975 RVA: 0x0020C2B3 File Offset: 0x0020A6B3
		private void EnableArrowSpawn()
		{
			this.allowArrowSpawn = true;
		}

		// Token: 0x06005DA8 RID: 23976 RVA: 0x0020C2BC File Offset: 0x0020A6BC
		private IEnumerator ArrowReleaseHaptics()
		{
			yield return new WaitForSeconds(0.05f);
			this.hand.otherHand.controller.TriggerHapticPulse(1500, EVRButtonId.k_EButton_Axis0);
			yield return new WaitForSeconds(0.05f);
			this.hand.otherHand.controller.TriggerHapticPulse(800, EVRButtonId.k_EButton_Axis0);
			yield return new WaitForSeconds(0.05f);
			this.hand.otherHand.controller.TriggerHapticPulse(500, EVRButtonId.k_EButton_Axis0);
			yield return new WaitForSeconds(0.05f);
			this.hand.otherHand.controller.TriggerHapticPulse(300, EVRButtonId.k_EButton_Axis0);
			yield break;
		}

		// Token: 0x06005DA9 RID: 23977 RVA: 0x0020C2D7 File Offset: 0x0020A6D7
		private void OnHandFocusLost(Hand hand)
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06005DAA RID: 23978 RVA: 0x0020C2E5 File Offset: 0x0020A6E5
		private void OnHandFocusAcquired(Hand hand)
		{
			base.gameObject.SetActive(true);
		}

		// Token: 0x06005DAB RID: 23979 RVA: 0x0020C2F3 File Offset: 0x0020A6F3
		private void FindBow()
		{
			this.bow = this.hand.otherHand.GetComponentInChildren<Longbow>();
		}

		// Token: 0x0400431C RID: 17180
		private Hand hand;

		// Token: 0x0400431D RID: 17181
		private Longbow bow;

		// Token: 0x0400431E RID: 17182
		private GameObject currentArrow;

		// Token: 0x0400431F RID: 17183
		public GameObject arrowPrefab;

		// Token: 0x04004320 RID: 17184
		public Transform arrowNockTransform;

		// Token: 0x04004321 RID: 17185
		public float nockDistance = 0.1f;

		// Token: 0x04004322 RID: 17186
		public float lerpCompleteDistance = 0.08f;

		// Token: 0x04004323 RID: 17187
		public float rotationLerpThreshold = 0.15f;

		// Token: 0x04004324 RID: 17188
		public float positionLerpThreshold = 0.15f;

		// Token: 0x04004325 RID: 17189
		private bool allowArrowSpawn = true;

		// Token: 0x04004326 RID: 17190
		private bool nocked;

		// Token: 0x04004327 RID: 17191
		private bool inNockRange;

		// Token: 0x04004328 RID: 17192
		private bool arrowLerpComplete;

		// Token: 0x04004329 RID: 17193
		public SoundPlayOneshot arrowSpawnSound;

		// Token: 0x0400432A RID: 17194
		private AllowTeleportWhileAttachedToHand allowTeleport;

		// Token: 0x0400432B RID: 17195
		public int maxArrowCount = 10;

		// Token: 0x0400432C RID: 17196
		private List<GameObject> arrowList;
	}
}
