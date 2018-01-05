using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A40 RID: 2624
public class JawController : MonoBehaviour
{
	// Token: 0x06004F21 RID: 20257 RVA: 0x001AABF4 File Offset: 0x001A8FF4
	public void Initialize(GameObject avatar, Animator animator, bool local)
	{
		this._jawBlendMeshes.Clear();
		this._jawBlendShapeIndex.Clear();
		if (animator != null)
		{
			this._jawTransform = animator.GetBoneTransform(HumanBodyBones.Jaw);
			if (this._jawTransform != null)
			{
				this.localRotationClosed = this._jawTransform.localRotation;
				Transform transform = animator.transform;
				Quaternion rhs = Quaternion.Inverse(transform.rotation) * this._jawTransform.rotation;
				Quaternion rhs2 = Quaternion.Euler(40f, 0f, 0f) * rhs;
				this._jawTransform.rotation = transform.rotation * rhs2;
				this.localRotationOpen = this._jawTransform.localRotation;
				this._jawTransform.localRotation = this.localRotationClosed;
			}
		}
		else
		{
			this._jawTransform = null;
		}
		if (this._jawTransform == null)
		{
			SkinnedMeshRenderer[] componentsInChildren = avatar.GetComponentsInChildren<SkinnedMeshRenderer>();
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in componentsInChildren)
			{
				for (int j = 0; j < skinnedMeshRenderer.sharedMesh.blendShapeCount; j++)
				{
					string blendShapeName = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(j);
					if (blendShapeName == this.MouthOpenBlendShapeName)
					{
						this._jawBlendMeshes.Add(skinnedMeshRenderer);
						this._jawBlendShapeIndex.Add(j);
					}
				}
			}
		}
	}

	// Token: 0x06004F22 RID: 20258 RVA: 0x001AAD70 File Offset: 0x001A9170
	private float GetRMSLevel(AudioSource src)
	{
		src.GetOutputData(this.samples, 0);
		float num = 0f;
		for (int i = 0; i < this.samples.Length; i++)
		{
			num += this.samples[i] * this.samples[i];
		}
		return Mathf.Sqrt(num / (float)this.samples.Length);
	}

	// Token: 0x06004F23 RID: 20259 RVA: 0x001AADD0 File Offset: 0x001A91D0
	private void LateUpdate()
	{
		this._lastJawTime += Time.deltaTime;
		while (this._lastJawTime > 0.0125f)
		{
			if (this.voiceSource != null)
			{
				this._jawMagnitude = this.GetRMSLevel(this.voiceSource);
			}
			if (this.voiceSpeaker != null)
			{
				this._jawMagnitude = this.voiceSpeaker.GetMagnitudeAndAdvance();
			}
			this._jawMagnitude = Mathf.Clamp01(this._jawMagnitude * 3f);
			this._lastJawTime -= 0.0125f;
		}
		if (this._jawTransform != null)
		{
			this._jawTransform.localRotation = Quaternion.Lerp(this.localRotationClosed, this.localRotationOpen, this._jawMagnitude);
		}
		else if (this._jawBlendMeshes.Count > 0)
		{
			for (int i = 0; i < this._jawBlendMeshes.Count; i++)
			{
				if (this._jawBlendMeshes[i] != null)
				{
					this._jawBlendMeshes[i].SetBlendShapeWeight(this._jawBlendShapeIndex[i], this._jawMagnitude * 100f);
				}
			}
		}
	}

	// Token: 0x06004F24 RID: 20260 RVA: 0x001AAF17 File Offset: 0x001A9317
	public void Detach()
	{
		this._jawTransform = null;
		this._jawBlendMeshes.Clear();
		this._jawBlendShapeIndex.Clear();
	}

	// Token: 0x04003784 RID: 14212
	public USpeaker voiceSpeaker;

	// Token: 0x04003785 RID: 14213
	public AudioSource voiceSource;

	// Token: 0x04003786 RID: 14214
	public Quaternion localRotationOpen;

	// Token: 0x04003787 RID: 14215
	public Quaternion localRotationClosed;

	// Token: 0x04003788 RID: 14216
	public string MouthOpenBlendShapeName;

	// Token: 0x04003789 RID: 14217
	private Transform _jawTransform;

	// Token: 0x0400378A RID: 14218
	private List<SkinnedMeshRenderer> _jawBlendMeshes = new List<SkinnedMeshRenderer>();

	// Token: 0x0400378B RID: 14219
	private List<int> _jawBlendShapeIndex = new List<int>();

	// Token: 0x0400378C RID: 14220
	private float _lastJawTime;

	// Token: 0x0400378D RID: 14221
	private float _jawMagnitude;

	// Token: 0x0400378E RID: 14222
	private float[] samples = new float[16];
}
