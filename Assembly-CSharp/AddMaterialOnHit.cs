using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200088C RID: 2188
public class AddMaterialOnHit : MonoBehaviour
{
	// Token: 0x06004356 RID: 17238 RVA: 0x00162974 File Offset: 0x00160D74
	private void Update()
	{
		if (this.EffectSettings == null)
		{
			return;
		}
		if (this.EffectSettings.IsVisible)
		{
			this.timeToDelete = 0f;
		}
		else
		{
			this.timeToDelete += Time.deltaTime;
			if (this.timeToDelete > this.RemoveAfterTime)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x06004357 RID: 17239 RVA: 0x001629E4 File Offset: 0x00160DE4
	public void UpdateMaterial(RaycastHit hit)
	{
		Transform transform = hit.transform;
		if (transform != null)
		{
			if (!this.RemoveWhenDisable)
			{
				UnityEngine.Object.Destroy(base.gameObject, this.RemoveAfterTime);
			}
			this.fadeInOutShaderColor = base.GetComponents<FadeInOutShaderColor>();
			this.fadeInOutShaderFloat = base.GetComponents<FadeInOutShaderFloat>();
			this.uvTextureAnimator = base.GetComponent<UVTextureAnimator>();
			this.renderParent = base.transform.parent.GetComponent<Renderer>();
			Material[] sharedMaterials = this.renderParent.sharedMaterials;
			int num = sharedMaterials.Length + 1;
			Material[] array = new Material[num];
			sharedMaterials.CopyTo(array, 0);
			this.renderParent.material = this.Material;
			this.instanceMat = this.renderParent.material;
			array[num - 1] = this.instanceMat;
			this.renderParent.sharedMaterials = array;
			if (this.UsePointMatrixTransform)
			{
				Matrix4x4 value = Matrix4x4.TRS(hit.transform.InverseTransformPoint(hit.point), Quaternion.Euler(180f, 180f, 0f), this.TransformScale);
				this.instanceMat.SetMatrix("_DecalMatr", value);
			}
			if (this.materialQueue != -1)
			{
				this.instanceMat.renderQueue = this.materialQueue;
			}
			if (this.fadeInOutShaderColor != null)
			{
				foreach (FadeInOutShaderColor fadeInOutShaderColor in this.fadeInOutShaderColor)
				{
					fadeInOutShaderColor.UpdateMaterial(this.instanceMat);
				}
			}
			if (this.fadeInOutShaderFloat != null)
			{
				foreach (FadeInOutShaderFloat fadeInOutShaderFloat in this.fadeInOutShaderFloat)
				{
					fadeInOutShaderFloat.UpdateMaterial(this.instanceMat);
				}
			}
			if (this.uvTextureAnimator != null)
			{
				this.uvTextureAnimator.SetInstanceMaterial(this.instanceMat, hit.textureCoord);
			}
		}
	}

	// Token: 0x06004358 RID: 17240 RVA: 0x00162BCC File Offset: 0x00160FCC
	public void UpdateMaterial(Transform transformTarget)
	{
		if (transformTarget != null)
		{
			if (!this.RemoveWhenDisable)
			{
				UnityEngine.Object.Destroy(base.gameObject, this.RemoveAfterTime);
			}
			this.fadeInOutShaderColor = base.GetComponents<FadeInOutShaderColor>();
			this.fadeInOutShaderFloat = base.GetComponents<FadeInOutShaderFloat>();
			this.uvTextureAnimator = base.GetComponent<UVTextureAnimator>();
			this.renderParent = base.transform.parent.GetComponent<Renderer>();
			Material[] sharedMaterials = this.renderParent.sharedMaterials;
			int num = sharedMaterials.Length + 1;
			Material[] array = new Material[num];
			sharedMaterials.CopyTo(array, 0);
			this.renderParent.material = this.Material;
			this.instanceMat = this.renderParent.material;
			array[num - 1] = this.instanceMat;
			this.renderParent.sharedMaterials = array;
			if (this.materialQueue != -1)
			{
				this.instanceMat.renderQueue = this.materialQueue;
			}
			if (this.fadeInOutShaderColor != null)
			{
				foreach (FadeInOutShaderColor fadeInOutShaderColor in this.fadeInOutShaderColor)
				{
					fadeInOutShaderColor.UpdateMaterial(this.instanceMat);
				}
			}
			if (this.fadeInOutShaderFloat != null)
			{
				foreach (FadeInOutShaderFloat fadeInOutShaderFloat in this.fadeInOutShaderFloat)
				{
					fadeInOutShaderFloat.UpdateMaterial(this.instanceMat);
				}
			}
			if (this.uvTextureAnimator != null)
			{
				this.uvTextureAnimator.SetInstanceMaterial(this.instanceMat, Vector2.zero);
			}
		}
	}

	// Token: 0x06004359 RID: 17241 RVA: 0x00162D57 File Offset: 0x00161157
	public void SetMaterialQueue(int matlQueue)
	{
		this.materialQueue = matlQueue;
	}

	// Token: 0x0600435A RID: 17242 RVA: 0x00162D60 File Offset: 0x00161160
	public int GetDefaultMaterialQueue()
	{
		return this.instanceMat.renderQueue;
	}

	// Token: 0x0600435B RID: 17243 RVA: 0x00162D70 File Offset: 0x00161170
	private void OnDestroy()
	{
		if (this.renderParent == null)
		{
			return;
		}
		List<Material> list = this.renderParent.sharedMaterials.ToList<Material>();
		list.Remove(this.instanceMat);
		this.renderParent.sharedMaterials = list.ToArray();
	}

	// Token: 0x04002BB6 RID: 11190
	public float RemoveAfterTime = 5f;

	// Token: 0x04002BB7 RID: 11191
	public bool RemoveWhenDisable;

	// Token: 0x04002BB8 RID: 11192
	public EffectSettings EffectSettings;

	// Token: 0x04002BB9 RID: 11193
	public Material Material;

	// Token: 0x04002BBA RID: 11194
	public bool UsePointMatrixTransform;

	// Token: 0x04002BBB RID: 11195
	public Vector3 TransformScale = Vector3.one;

	// Token: 0x04002BBC RID: 11196
	private FadeInOutShaderColor[] fadeInOutShaderColor;

	// Token: 0x04002BBD RID: 11197
	private FadeInOutShaderFloat[] fadeInOutShaderFloat;

	// Token: 0x04002BBE RID: 11198
	private UVTextureAnimator uvTextureAnimator;

	// Token: 0x04002BBF RID: 11199
	private Renderer renderParent;

	// Token: 0x04002BC0 RID: 11200
	private Material instanceMat;

	// Token: 0x04002BC1 RID: 11201
	private int materialQueue = -1;

	// Token: 0x04002BC2 RID: 11202
	private bool waitRemove;

	// Token: 0x04002BC3 RID: 11203
	private float timeToDelete;
}
