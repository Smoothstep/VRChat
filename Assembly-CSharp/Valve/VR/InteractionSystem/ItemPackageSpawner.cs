using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BB4 RID: 2996
	[RequireComponent(typeof(Interactable))]
	public class ItemPackageSpawner : MonoBehaviour
	{
		// Token: 0x17000D36 RID: 3382
		// (get) Token: 0x06005CBB RID: 23739 RVA: 0x002061A8 File Offset: 0x002045A8
		// (set) Token: 0x06005CBC RID: 23740 RVA: 0x002061B0 File Offset: 0x002045B0
		public ItemPackage itemPackage
		{
			get
			{
				return this._itemPackage;
			}
			set
			{
				this.CreatePreviewObject();
			}
		}

		// Token: 0x06005CBD RID: 23741 RVA: 0x002061B8 File Offset: 0x002045B8
		private void CreatePreviewObject()
		{
			if (!this.useItemPackagePreview)
			{
				return;
			}
			this.ClearPreview();
			if (this.useItemPackagePreview)
			{
				if (this.itemPackage == null)
				{
					return;
				}
				if (!this.useFadedPreview)
				{
					if (this.itemPackage.previewPrefab != null)
					{
						this.previewObject = UnityEngine.Object.Instantiate<GameObject>(this.itemPackage.previewPrefab, base.transform.position, Quaternion.identity);
						this.previewObject.transform.parent = base.transform;
						this.previewObject.transform.localRotation = Quaternion.identity;
					}
				}
				else if (this.itemPackage.fadedPreviewPrefab != null)
				{
					this.previewObject = UnityEngine.Object.Instantiate<GameObject>(this.itemPackage.fadedPreviewPrefab, base.transform.position, Quaternion.identity);
					this.previewObject.transform.parent = base.transform;
					this.previewObject.transform.localRotation = Quaternion.identity;
				}
			}
		}

		// Token: 0x06005CBE RID: 23742 RVA: 0x002062D2 File Offset: 0x002046D2
		private void Start()
		{
			this.VerifyItemPackage();
		}

		// Token: 0x06005CBF RID: 23743 RVA: 0x002062DA File Offset: 0x002046DA
		private void VerifyItemPackage()
		{
			if (this.itemPackage == null)
			{
				this.ItemPackageNotValid();
			}
			if (this.itemPackage.itemPrefab == null)
			{
				this.ItemPackageNotValid();
			}
		}

		// Token: 0x06005CC0 RID: 23744 RVA: 0x0020630F File Offset: 0x0020470F
		private void ItemPackageNotValid()
		{
			Debug.LogError("ItemPackage assigned to " + base.gameObject.name + " is not valid. Destroying this game object.");
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06005CC1 RID: 23745 RVA: 0x0020633C File Offset: 0x0020473C
		private void ClearPreview()
		{
			IEnumerator enumerator = base.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					if (Time.time > 0f)
					{
						UnityEngine.Object.Destroy(transform.gameObject);
					}
					else
					{
						UnityEngine.Object.DestroyImmediate(transform.gameObject);
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}

		// Token: 0x06005CC2 RID: 23746 RVA: 0x002063C4 File Offset: 0x002047C4
		private void Update()
		{
			if (this.itemIsSpawned && this.spawnedItem == null)
			{
				this.itemIsSpawned = false;
				this.useFadedPreview = false;
				this.dropEvent.Invoke();
				this.CreatePreviewObject();
			}
		}

		// Token: 0x06005CC3 RID: 23747 RVA: 0x00206404 File Offset: 0x00204804
		private void OnHandHoverBegin(Hand hand)
		{
			ItemPackage attachedItemPackage = this.GetAttachedItemPackage(hand);
			if (attachedItemPackage == this.itemPackage && this.takeBackItem && !this.requireTriggerPressToReturn)
			{
				this.TakeBackItem(hand);
			}
			if (!this.requireTriggerPressToTake)
			{
				this.SpawnAndAttachObject(hand);
			}
			if (this.requireTriggerPressToTake && this.showTriggerHint)
			{
				ControllerButtonHints.ShowTextHint(hand, EVRButtonId.k_EButton_Axis1, "PickUp", true);
			}
		}

		// Token: 0x06005CC4 RID: 23748 RVA: 0x0020647D File Offset: 0x0020487D
		private void TakeBackItem(Hand hand)
		{
			this.RemoveMatchingItemsFromHandStack(this.itemPackage, hand);
			if (this.itemPackage.packageType == ItemPackage.ItemPackageType.TwoHanded)
			{
				this.RemoveMatchingItemsFromHandStack(this.itemPackage, hand.otherHand);
			}
		}

		// Token: 0x06005CC5 RID: 23749 RVA: 0x002064B0 File Offset: 0x002048B0
		private ItemPackage GetAttachedItemPackage(Hand hand)
		{
			GameObject currentAttachedObject = hand.currentAttachedObject;
			if (currentAttachedObject == null)
			{
				return null;
			}
			ItemPackageReference component = hand.currentAttachedObject.GetComponent<ItemPackageReference>();
			if (component == null)
			{
				return null;
			}
			return component.itemPackage;
		}

		// Token: 0x06005CC6 RID: 23750 RVA: 0x002064F4 File Offset: 0x002048F4
		private void HandHoverUpdate(Hand hand)
		{
			if (this.requireTriggerPressToTake && hand.controller != null && hand.controller.GetHairTriggerDown())
			{
				this.SpawnAndAttachObject(hand);
			}
		}

		// Token: 0x06005CC7 RID: 23751 RVA: 0x00206523 File Offset: 0x00204923
		private void OnHandHoverEnd(Hand hand)
		{
			if (!this.justPickedUpItem && this.requireTriggerPressToTake && this.showTriggerHint)
			{
				ControllerButtonHints.HideTextHint(hand, EVRButtonId.k_EButton_Axis1);
			}
			this.justPickedUpItem = false;
		}

		// Token: 0x06005CC8 RID: 23752 RVA: 0x00206558 File Offset: 0x00204958
		private void RemoveMatchingItemsFromHandStack(ItemPackage package, Hand hand)
		{
			for (int i = 0; i < hand.AttachedObjects.Count; i++)
			{
				ItemPackageReference component = hand.AttachedObjects[i].attachedObject.GetComponent<ItemPackageReference>();
				if (component != null)
				{
					ItemPackage itemPackage = component.itemPackage;
					if (itemPackage != null && itemPackage == package)
					{
						GameObject attachedObject = hand.AttachedObjects[i].attachedObject;
						hand.DetachObject(attachedObject, true);
					}
				}
			}
		}

		// Token: 0x06005CC9 RID: 23753 RVA: 0x002065E8 File Offset: 0x002049E8
		private void RemoveMatchingItemTypesFromHand(ItemPackage.ItemPackageType packageType, Hand hand)
		{
			for (int i = 0; i < hand.AttachedObjects.Count; i++)
			{
				ItemPackageReference component = hand.AttachedObjects[i].attachedObject.GetComponent<ItemPackageReference>();
				if (component != null && component.itemPackage.packageType == packageType)
				{
					GameObject attachedObject = hand.AttachedObjects[i].attachedObject;
					hand.DetachObject(attachedObject, true);
				}
			}
		}

		// Token: 0x06005CCA RID: 23754 RVA: 0x00206668 File Offset: 0x00204A68
		private void SpawnAndAttachObject(Hand hand)
		{
			if (hand.otherHand != null)
			{
				ItemPackage attachedItemPackage = this.GetAttachedItemPackage(hand.otherHand);
				if (attachedItemPackage == this.itemPackage)
				{
					this.TakeBackItem(hand.otherHand);
				}
			}
			if (this.showTriggerHint)
			{
				ControllerButtonHints.HideTextHint(hand, EVRButtonId.k_EButton_Axis1);
			}
			if (this.itemPackage.otherHandItemPrefab != null && hand.otherHand.hoverLocked)
			{
				return;
			}
			if (this.itemPackage.packageType == ItemPackage.ItemPackageType.OneHanded)
			{
				this.RemoveMatchingItemTypesFromHand(ItemPackage.ItemPackageType.OneHanded, hand);
				this.RemoveMatchingItemTypesFromHand(ItemPackage.ItemPackageType.TwoHanded, hand);
				this.RemoveMatchingItemTypesFromHand(ItemPackage.ItemPackageType.TwoHanded, hand.otherHand);
			}
			if (this.itemPackage.packageType == ItemPackage.ItemPackageType.TwoHanded)
			{
				this.RemoveMatchingItemTypesFromHand(ItemPackage.ItemPackageType.OneHanded, hand);
				this.RemoveMatchingItemTypesFromHand(ItemPackage.ItemPackageType.OneHanded, hand.otherHand);
				this.RemoveMatchingItemTypesFromHand(ItemPackage.ItemPackageType.TwoHanded, hand);
				this.RemoveMatchingItemTypesFromHand(ItemPackage.ItemPackageType.TwoHanded, hand.otherHand);
			}
			this.spawnedItem = UnityEngine.Object.Instantiate<GameObject>(this.itemPackage.itemPrefab);
			this.spawnedItem.SetActive(true);
			hand.AttachObject(this.spawnedItem, this.attachmentFlags, this.attachmentPoint);
			if (this.itemPackage.otherHandItemPrefab != null && hand.otherHand.controller != null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.itemPackage.otherHandItemPrefab);
				gameObject.SetActive(true);
				hand.otherHand.AttachObject(gameObject, this.attachmentFlags, string.Empty);
			}
			this.itemIsSpawned = true;
			this.justPickedUpItem = true;
			if (this.takeBackItem)
			{
				this.useFadedPreview = true;
				this.pickupEvent.Invoke();
				this.CreatePreviewObject();
			}
		}

		// Token: 0x0400424C RID: 16972
		public ItemPackage _itemPackage;

		// Token: 0x0400424D RID: 16973
		private bool useItemPackagePreview = true;

		// Token: 0x0400424E RID: 16974
		private bool useFadedPreview;

		// Token: 0x0400424F RID: 16975
		private GameObject previewObject;

		// Token: 0x04004250 RID: 16976
		public bool requireTriggerPressToTake;

		// Token: 0x04004251 RID: 16977
		public bool requireTriggerPressToReturn;

		// Token: 0x04004252 RID: 16978
		public bool showTriggerHint;

		// Token: 0x04004253 RID: 16979
		[EnumFlags]
		public Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.SnapOnAttach | Hand.AttachmentFlags.DetachOthers | Hand.AttachmentFlags.DetachFromOtherHand | Hand.AttachmentFlags.ParentToHand;

		// Token: 0x04004254 RID: 16980
		public string attachmentPoint;

		// Token: 0x04004255 RID: 16981
		public bool takeBackItem;

		// Token: 0x04004256 RID: 16982
		public bool acceptDifferentItems;

		// Token: 0x04004257 RID: 16983
		private GameObject spawnedItem;

		// Token: 0x04004258 RID: 16984
		private bool itemIsSpawned;

		// Token: 0x04004259 RID: 16985
		public UnityEvent pickupEvent;

		// Token: 0x0400425A RID: 16986
		public UnityEvent dropEvent;

		// Token: 0x0400425B RID: 16987
		public bool justPickedUpItem;
	}
}
