using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C8A RID: 3210
public class VRCUiSelectedOutline : MonoBehaviour
{
	// Token: 0x060063B8 RID: 25528 RVA: 0x00236850 File Offset: 0x00234C50
	private void Start()
	{
		this.meshFilter = base.GetComponent<MeshFilter>();
		this.meshRenderer = base.GetComponent<MeshRenderer>();
		this.meshRenderer.material.renderQueue = 3997;
		this.initialScale = base.transform.localScale;
		this.sphere = (Resources.Load("PrimitiveMeshes/sphere") as Mesh);
		this.capsule = (Resources.Load("PrimitiveMeshes/capsule") as Mesh);
		this.cube = (Resources.Load("PrimitiveMeshes/cube") as Mesh);
	}

	// Token: 0x060063B9 RID: 25529 RVA: 0x002368DC File Offset: 0x00234CDC
	public void Clone(Component interactable)
	{
		if (interactable == null)
		{
			this.meshFilter.sharedMesh = null;
			HighlightsFX.Instance.EnableOutline(this.meshRenderer, false);
			foreach (MeshRenderer renderer in this.outlinedRenderers)
			{
				HighlightsFX.Instance.EnableOutline(renderer, false);
			}
			this.outlinedRenderers.Clear();
			return;
		}
		foreach (MeshRenderer renderer2 in this.outlinedRenderers)
		{
			HighlightsFX.Instance.EnableOutline(renderer2, false);
		}
		this.outlinedRenderers.Clear();
		interactable.GetComponentsInChildren<MeshRenderer>(false, this.outlinedRenderers);
		this.outlinedRenderers.RemoveAll(delegate(MeshRenderer r)
		{
			if (!r.enabled)
			{
				return true;
			}
			MeshFilter component2 = r.GetComponent<MeshFilter>();
			return component2 == null || component2.sharedMesh == null || component2.sharedMesh.name.Contains("Combined Mesh");
		});
		foreach (MeshRenderer renderer3 in this.outlinedRenderers)
		{
			HighlightsFX.Instance.EnableOutline(renderer3, true);
		}
		if (this.outlinedRenderers.Count > 0)
		{
			return;
		}
		Collider component = interactable.GetComponent<Collider>();
		MeshCollider meshCollider = component as MeshCollider;
		if (meshCollider != null)
		{
			this.meshFilter.sharedMesh = meshCollider.sharedMesh;
			base.transform.position = interactable.transform.position;
			base.transform.rotation = interactable.transform.rotation;
			base.transform.localScale = Vector3.Scale(interactable.transform.lossyScale, this.initialScale);
			HighlightsFX.Instance.EnableOutline(this.meshRenderer, true);
			this.outlinedRenderers.Add(this.meshRenderer);
			return;
		}
		BoxCollider boxCollider = component as BoxCollider;
		if (boxCollider != null)
		{
			this.meshFilter.sharedMesh = this.cube;
			base.transform.position = boxCollider.transform.TransformPoint(boxCollider.center);
			base.transform.rotation = boxCollider.transform.rotation;
			base.transform.localScale = Vector3.Scale(interactable.transform.lossyScale, Vector3.Scale(boxCollider.size, this.initialScale));
			HighlightsFX.Instance.EnableOutline(this.meshRenderer, true);
			this.outlinedRenderers.Add(this.meshRenderer);
			return;
		}
		SphereCollider sphereCollider = component as SphereCollider;
		if (sphereCollider != null)
		{
			this.meshFilter.sharedMesh = this.sphere;
			base.transform.position = sphereCollider.transform.TransformPoint(sphereCollider.center);
			base.transform.localScale = Vector3.Scale(interactable.transform.lossyScale, sphereCollider.radius * this.initialScale);
			HighlightsFX.Instance.EnableOutline(this.meshRenderer, true);
			this.outlinedRenderers.Add(this.meshRenderer);
			return;
		}
		CapsuleCollider capsuleCollider = component as CapsuleCollider;
		if (capsuleCollider != null)
		{
			this.meshFilter.sharedMesh = this.capsule;
			base.transform.position = capsuleCollider.transform.TransformPoint(capsuleCollider.center);
			Quaternion rhs = Quaternion.identity;
			float num = capsuleCollider.height;
			float num2 = capsuleCollider.radius;
			if (capsuleCollider.direction == 0)
			{
				rhs = Quaternion.Euler(0f, 0f, -90f);
				num2 = Mathf.Abs(Mathf.Max(capsuleCollider.transform.lossyScale.y, capsuleCollider.transform.lossyScale.z) * capsuleCollider.radius);
				num = Mathf.Max(capsuleCollider.transform.lossyScale.x * capsuleCollider.height, num2);
			}
			else if (capsuleCollider.direction == 1)
			{
				num2 = Mathf.Abs(Mathf.Max(capsuleCollider.transform.lossyScale.x, capsuleCollider.transform.lossyScale.z) * capsuleCollider.radius);
				num = Mathf.Max(capsuleCollider.transform.lossyScale.y * capsuleCollider.height, num2);
			}
			else if (capsuleCollider.direction == 2)
			{
				rhs = Quaternion.Euler(90f, 0f, 0f);
				num2 = Mathf.Abs(Mathf.Max(capsuleCollider.transform.lossyScale.x, capsuleCollider.transform.lossyScale.y) * capsuleCollider.radius);
				num = Mathf.Max(capsuleCollider.transform.lossyScale.z * capsuleCollider.height, num2);
			}
			base.transform.rotation = capsuleCollider.transform.rotation * rhs;
			base.transform.localScale = Vector3.Scale(new Vector3(num2, num / 4f, num2), this.initialScale);
			HighlightsFX.Instance.EnableOutline(this.meshRenderer, true);
			this.outlinedRenderers.Add(this.meshRenderer);
			return;
		}
		Debug.LogWarning("Highlight Other - " + component.ToString());
	}

	// Token: 0x04004908 RID: 18696
	private MeshFilter meshFilter;

	// Token: 0x04004909 RID: 18697
	private MeshRenderer meshRenderer;

	// Token: 0x0400490A RID: 18698
	private Vector3 initialScale;

	// Token: 0x0400490B RID: 18699
	private Mesh sphere;

	// Token: 0x0400490C RID: 18700
	private Mesh capsule;

	// Token: 0x0400490D RID: 18701
	private Mesh cube;

	// Token: 0x0400490E RID: 18702
	private List<MeshRenderer> outlinedRenderers = new List<MeshRenderer>();
}
