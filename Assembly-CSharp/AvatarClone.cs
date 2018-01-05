using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000A30 RID: 2608
public class AvatarClone : MonoBehaviour
{
	// Token: 0x06004E87 RID: 20103 RVA: 0x001A5088 File Offset: 0x001A3488
	private void CopyBlendShapes(SkinnedMeshRenderer skin, SkinnedMeshRenderer skinClone)
	{
		if (skin != null && skinClone != null)
		{
			Mesh sharedMesh = skin.sharedMesh;
			Mesh sharedMesh2 = skinClone.sharedMesh;
			if (sharedMesh != null && sharedMesh.blendShapeCount > 0 && sharedMesh.blendShapeCount == sharedMesh2.blendShapeCount)
			{
				for (int i = 0; i < sharedMesh.blendShapeCount; i++)
				{
					float blendShapeWeight = skin.GetBlendShapeWeight(i);
					skinClone.SetBlendShapeWeight(i, blendShapeWeight);
				}
			}
		}
	}

	// Token: 0x06004E88 RID: 20104 RVA: 0x001A510C File Offset: 0x001A350C
	private void LateUpdate()
	{
		if (!this._initialized)
		{
			this.LateInit();
		}
		if (this.OriginalAvatar == null)
		{
			return;
		}
		Vector3 position = this.OriginalAvatar.transform.position;
		Quaternion rotation = this.OriginalAvatar.transform.rotation;
		base.transform.position = position;
		base.transform.rotation = rotation;
		int num = 0;
		while (num < this.cloneTransforms.Length && num < this.origTransforms.Length)
		{
			if (!(this.origTransforms[num] == null))
			{
				Vector3 position2 = this.origTransforms[num].position;
				Quaternion rotation2 = this.origTransforms[num].rotation;
				this.cloneTransforms[num].position = position2;
				this.cloneTransforms[num].rotation = rotation2;
			}
			num++;
		}
		int num2 = 0;
		while (num2 < this.cloneRenderers.Length && num2 < this.origRenderers.Length)
		{
			if (!(this.cloneRenderers[num2] == null) && !(this.origRenderers[num2] == null))
			{
				this.cloneRenderers[num2].enabled = this.origRenderers[num2].enabled;
				this.cloneRenderers[num2].sharedMaterials = this.origRenderers[num2].sharedMaterials;
				if (this.CloneType == AvatarCloneType.Mirror)
				{
					SkinnedMeshRenderer skin = this.origRenderers[num2] as SkinnedMeshRenderer;
					SkinnedMeshRenderer skinClone = this.cloneRenderers[num2] as SkinnedMeshRenderer;
					this.CopyBlendShapes(skin, skinClone);
				}
			}
			num2++;
		}
	}

	// Token: 0x06004E89 RID: 20105 RVA: 0x001A52B8 File Offset: 0x001A36B8
	private void LateInit()
	{
		if (this.OriginalAvatar != null)
		{
			this.StripComponents();
			List<Transform> list = new List<Transform>(this.OriginalAvatar.GetComponentsInChildren<Transform>());
			int count = list.Count;
			List<Transform> list2 = new List<Transform>(base.transform.GetComponentsInChildren<Transform>());
			int count2 = list2.Count;
			this.origTransforms = new Transform[count2];
			this.cloneTransforms = new Transform[count2];
			int num = 0;
			using (List<Transform>.Enumerator enumerator = list2.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Transform ct = enumerator.Current;
					if (!(ct == base.transform))
					{
						Transform transform = list.Find((Transform t) => t.name == ct.name);
						if (transform != null)
						{
							this.origTransforms[num] = transform;
							this.cloneTransforms[num++] = ct;
						}
						else
						{
							Debug.LogError("AvatarClone: could not find original transform matching: " + ct.name);
						}
					}
				}
			}
			List<Renderer> list3 = new List<Renderer>(this.OriginalAvatar.GetComponentsInChildren<Renderer>());
			List<Renderer> list4 = new List<Renderer>(base.transform.GetComponentsInChildren<Renderer>());
			this.origRenderers = this.OriginalAvatar.GetComponentsInChildren<Renderer>();
			this.cloneRenderers = base.GetComponentsInChildren<Renderer>();
			num = 0;
			using (List<Renderer>.Enumerator enumerator2 = list4.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Renderer cr = enumerator2.Current;
					Renderer renderer = list3.Find((Renderer r) => r.gameObject.name == cr.gameObject.name);
					if (renderer != null)
					{
						this.origRenderers[num] = renderer;
						this.cloneRenderers[num++] = cr;
					}
					else if (cr.transform != base.transform)
					{
						Debug.LogError("AvatarClone: could not find original renderer matching: " + cr.name);
					}
				}
			}
			for (int i = 0; i < this.origRenderers.Length; i++)
			{
				this.origRenderers[i].shadowCastingMode = ShadowCastingMode.Off;
			}
			for (int j = 0; j < this.cloneRenderers.Length; j++)
			{
				this.cloneRenderers[j].shadowCastingMode = ((this.CloneType != AvatarCloneType.Shadow) ? ShadowCastingMode.On : ShadowCastingMode.ShadowsOnly);
			}
			this._initialized = true;
		}
	}

	// Token: 0x06004E8A RID: 20106 RVA: 0x001A5578 File Offset: 0x001A3978
	public void StripComponents()
	{
		Component[] components = base.gameObject.GetComponents<Component>();
		for (int i = 1; i < components.Length; i++)
		{
			Component component = components[i];
			Renderer x = components[i] as Renderer;
			MeshFilter x2 = components[i] as MeshFilter;
			if (component != this && x == null && x2 == null)
			{
				UnityEngine.Object.Destroy(component);
			}
		}
	}

	// Token: 0x040036A4 RID: 13988
	public GameObject OriginalAvatar;

	// Token: 0x040036A5 RID: 13989
	public AvatarCloneType CloneType;

	// Token: 0x040036A6 RID: 13990
	private Transform[] origTransforms;

	// Token: 0x040036A7 RID: 13991
	private Transform[] cloneTransforms;

	// Token: 0x040036A8 RID: 13992
	private Renderer[] origRenderers;

	// Token: 0x040036A9 RID: 13993
	private Renderer[] cloneRenderers;

	// Token: 0x040036AA RID: 13994
	private bool _initialized;
}
