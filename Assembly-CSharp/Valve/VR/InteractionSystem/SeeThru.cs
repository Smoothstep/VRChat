using System;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BBE RID: 3006
	public class SeeThru : MonoBehaviour
	{
		// Token: 0x06005D00 RID: 23808 RVA: 0x00207980 File Offset: 0x00205D80
		private void Awake()
		{
			this.interactable = base.GetComponentInParent<Interactable>();
			this.seeThru = new GameObject("_see_thru");
			this.seeThru.transform.parent = base.transform;
			this.seeThru.transform.localPosition = Vector3.zero;
			this.seeThru.transform.localRotation = Quaternion.identity;
			this.seeThru.transform.localScale = Vector3.one;
			MeshFilter component = base.GetComponent<MeshFilter>();
			if (component != null)
			{
				MeshFilter meshFilter = this.seeThru.AddComponent<MeshFilter>();
				meshFilter.sharedMesh = component.sharedMesh;
			}
			MeshRenderer component2 = base.GetComponent<MeshRenderer>();
			if (component2 != null)
			{
				this.sourceRenderer = component2;
				this.destRenderer = this.seeThru.AddComponent<MeshRenderer>();
			}
			SkinnedMeshRenderer component3 = base.GetComponent<SkinnedMeshRenderer>();
			if (component3 != null)
			{
				SkinnedMeshRenderer skinnedMeshRenderer = this.seeThru.AddComponent<SkinnedMeshRenderer>();
				this.sourceRenderer = component3;
				this.destRenderer = skinnedMeshRenderer;
				skinnedMeshRenderer.sharedMesh = component3.sharedMesh;
				skinnedMeshRenderer.rootBone = component3.rootBone;
				skinnedMeshRenderer.bones = component3.bones;
				skinnedMeshRenderer.quality = component3.quality;
				skinnedMeshRenderer.updateWhenOffscreen = component3.updateWhenOffscreen;
			}
			if (this.sourceRenderer != null && this.destRenderer != null)
			{
				int num = this.sourceRenderer.sharedMaterials.Length;
				Material[] array = new Material[num];
				for (int i = 0; i < num; i++)
				{
					array[i] = this.seeThruMaterial;
				}
				this.destRenderer.sharedMaterials = array;
				for (int j = 0; j < this.destRenderer.materials.Length; j++)
				{
					this.destRenderer.materials[j].renderQueue = 2001;
				}
				for (int k = 0; k < this.sourceRenderer.materials.Length; k++)
				{
					if (this.sourceRenderer.materials[k].renderQueue == 2000)
					{
						this.sourceRenderer.materials[k].renderQueue = 2002;
					}
				}
			}
			this.seeThru.gameObject.SetActive(false);
		}

		// Token: 0x06005D01 RID: 23809 RVA: 0x00207BCF File Offset: 0x00205FCF
		private void OnEnable()
		{
			this.interactable.onAttachedToHand += this.AttachedToHand;
			this.interactable.onDetachedFromHand += this.DetachedFromHand;
		}

		// Token: 0x06005D02 RID: 23810 RVA: 0x00207BFF File Offset: 0x00205FFF
		private void OnDisable()
		{
			this.interactable.onAttachedToHand -= this.AttachedToHand;
			this.interactable.onDetachedFromHand -= this.DetachedFromHand;
		}

		// Token: 0x06005D03 RID: 23811 RVA: 0x00207C2F File Offset: 0x0020602F
		private void AttachedToHand(Hand hand)
		{
			this.seeThru.SetActive(true);
		}

		// Token: 0x06005D04 RID: 23812 RVA: 0x00207C3D File Offset: 0x0020603D
		private void DetachedFromHand(Hand hand)
		{
			this.seeThru.SetActive(false);
		}

		// Token: 0x06005D05 RID: 23813 RVA: 0x00207C4C File Offset: 0x0020604C
		private void Update()
		{
			if (this.seeThru.activeInHierarchy)
			{
				int num = Mathf.Min(this.sourceRenderer.materials.Length, this.destRenderer.materials.Length);
				for (int i = 0; i < num; i++)
				{
					this.destRenderer.materials[i].mainTexture = this.sourceRenderer.materials[i].mainTexture;
					this.destRenderer.materials[i].color = this.destRenderer.materials[i].color * this.sourceRenderer.materials[i].color;
				}
			}
		}

		// Token: 0x0400429C RID: 17052
		public Material seeThruMaterial;

		// Token: 0x0400429D RID: 17053
		private GameObject seeThru;

		// Token: 0x0400429E RID: 17054
		private Interactable interactable;

		// Token: 0x0400429F RID: 17055
		private Renderer sourceRenderer;

		// Token: 0x040042A0 RID: 17056
		private Renderer destRenderer;
	}
}
