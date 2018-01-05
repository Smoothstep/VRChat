using System;
using UnityEngine;

// Token: 0x02000A43 RID: 2627
public class LipSyncController : MonoBehaviour
{
	// Token: 0x06004F3B RID: 20283 RVA: 0x001AC038 File Offset: 0x001AA438
	public bool InitializeBlendShapes(string[] blends, GameObject avatar, Animator animator, SkinnedMeshRenderer skinnedMesh, bool local)
	{
		if (this.voiceSource != null)
		{
			this.voiceObj = this.voiceSource.gameObject;
		}
		else
		{
			if (!(this.voiceSpeaker != null))
			{
				return false;
			}
			this.voiceObj = this.voiceSpeaker.gameObject;
		}
		this._context = this.voiceObj.GetComponent<OVRLipSyncContext>();
		this._context.enabled = true;
		this._context.Initialize();
		this._morph = this.voiceObj.GetComponent<OVRLipSyncContextMorphTarget>();
		this._morph.skinnedMeshRenderer = skinnedMesh;
		this._morph.VisemeToBlendTargets = new int[LipSyncController.NUM_VISEMES];
		for (int i = 0; i < LipSyncController.NUM_VISEMES; i++)
		{
			int blendIndex = this.GetBlendIndex(skinnedMesh, blends[i]);
			if (blendIndex < 0)
			{
				Debug.LogError("Avatar Descriptor has invalid Blend Shape [" + blends[i] + "]");
				return false;
			}
			this._morph.VisemeToBlendTargets[i] = blendIndex;
		}
		this._morph.enabled = true;
		this._morph.Initialize();
		if (local)
		{
			if (this.voiceSpeaker != null)
			{
				this.voiceSpeaker.DebugPlayback = true;
			}
			this._context.audioMute = true;
			this._context.delayCompensate = false;
			this._context.gain = 10f;
		}
		return true;
	}

	// Token: 0x06004F3C RID: 20284 RVA: 0x001AC1AC File Offset: 0x001AA5AC
	public bool InitializeTextures(int mtlIndex, Texture2D[] textures, GameObject avatar, Animator animator, bool local)
	{
		if (this.voiceSource != null)
		{
			this.voiceObj = this.voiceSource.gameObject;
		}
		else
		{
			if (!(this.voiceSpeaker != null))
			{
				return false;
			}
			this.voiceObj = this.voiceSpeaker.gameObject;
		}
		if (!local)
		{
			this._context = this.voiceObj.GetComponent<OVRLipSyncContext>();
			this._flip = this.voiceObj.GetComponent<OVRLipSyncContextTextureFlip>();
			for (int i = 0; i < LipSyncController.NUM_VISEMES; i++)
			{
				this._flip.Textures[i] = textures[i];
			}
			this._context.enabled = true;
			this._context.Initialize();
			this._flip.enabled = true;
			this._flip.Initialize();
		}
		return true;
	}

	// Token: 0x06004F3D RID: 20285 RVA: 0x001AC28C File Offset: 0x001AA68C
	private int GetBlendIndex(SkinnedMeshRenderer skin, string name)
	{
		for (int i = 0; i < skin.sharedMesh.blendShapeCount; i++)
		{
			if (skin.sharedMesh.GetBlendShapeName(i) == name)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06004F3E RID: 20286 RVA: 0x001AC2D0 File Offset: 0x001AA6D0
	public void DisableLipSyncComponents()
	{
		if (this.voiceObj != null)
		{
			this._context = this.voiceObj.GetComponent<OVRLipSyncContext>();
			this._context.enabled = false;
			this._morph = this.voiceObj.GetComponent<OVRLipSyncContextMorphTarget>();
			this._morph.enabled = false;
			this._flip = this.voiceObj.GetComponent<OVRLipSyncContextTextureFlip>();
			this._flip.enabled = false;
		}
		this.voiceObj = null;
	}

	// Token: 0x06004F3F RID: 20287 RVA: 0x001AC34C File Offset: 0x001AA74C
	private void Start()
	{
	}

	// Token: 0x06004F40 RID: 20288 RVA: 0x001AC34E File Offset: 0x001AA74E
	private void Update()
	{
	}

	// Token: 0x040037B0 RID: 14256
	public static int NUM_VISEMES = 15;

	// Token: 0x040037B1 RID: 14257
	public USpeaker voiceSpeaker;

	// Token: 0x040037B2 RID: 14258
	public AudioSource voiceSource;

	// Token: 0x040037B3 RID: 14259
	private GameObject voiceObj;

	// Token: 0x040037B4 RID: 14260
	private OVRLipSyncMicInput _mic;

	// Token: 0x040037B5 RID: 14261
	private OVRLipSyncContext _context;

	// Token: 0x040037B6 RID: 14262
	private OVRLipSyncContextMorphTarget _morph;

	// Token: 0x040037B7 RID: 14263
	private OVRLipSyncContextTextureFlip _flip;
}
