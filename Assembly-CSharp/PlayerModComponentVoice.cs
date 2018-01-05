using System;
using System.Collections.Generic;
using UnityEngine;
using VRC;
using VRCSDK2;

// Token: 0x02000ACE RID: 2766
public class PlayerModComponentVoice : MonoBehaviour, IPlayerModComponent
{
	// Token: 0x06005403 RID: 21507 RVA: 0x001D033D File Offset: 0x001CE73D
	private void Awake()
	{
		this.sender = base.GetComponentInChildren<USpeakPhotonSender3D>();
		this.speaker = base.GetComponentInChildren<USpeaker>();
	}

	// Token: 0x06005404 RID: 21508 RVA: 0x001D0357 File Offset: 0x001CE757
	private void Start()
	{
		this.ApplyModdedVariabes();
	}

	// Token: 0x06005405 RID: 21509 RVA: 0x001D035F File Offset: 0x001CE75F
	private void OnDestroy()
	{
		this.ResetModdedVariables();
	}

	// Token: 0x06005406 RID: 21510 RVA: 0x001D0368 File Offset: 0x001CE768
	public void SetProperties(List<VRCPlayerModProperty> properties)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		foreach (VRCPlayerModProperty vrcplayerModProperty in properties)
		{
			dictionary[vrcplayerModProperty.name] = vrcplayerModProperty.value();
		}
		this.talkDistance = (float)Tools.GetOrDefaultFromDictionary(dictionary, "talkDistance", 20f);
		this.is3DMode = (bool)Tools.GetOrDefaultFromDictionary(dictionary, "is3DMode", true);
		this.ApplyModdedVariabes();
	}

	// Token: 0x06005407 RID: 21511 RVA: 0x001D0414 File Offset: 0x001CE814
	private void ApplyModdedVariabes()
	{
		if (!this.hasSetOrigValues)
		{
			if (this.sender != null)
			{
				this.origTalkDistance = this.sender.distanceThreshold;
			}
			this.hasSetOrigValues = true;
		}
		if (this.sender != null)
		{
			this.sender.distanceThreshold = this.talkDistance;
		}
		if (this.speaker != null)
		{
			this.speaker.SetMaxDistance(this.talkDistance * 2f);
			this.speaker._3DMode = ((!this.is3DMode) ? ThreeDMode.None : ThreeDMode.Full3D);
		}
	}

	// Token: 0x06005408 RID: 21512 RVA: 0x001D04BC File Offset: 0x001CE8BC
	private void ResetModdedVariables()
	{
		if (this.sender != null)
		{
			this.sender.distanceThreshold = this.origTalkDistance;
		}
		if (this.speaker != null && !this.is3DMode)
		{
			this.speaker._3DMode = ThreeDMode.Full3D;
		}
		if (this.speaker != null)
		{
			this.speaker.SetMaxDistance(this.origTalkDistance * 2f);
			this.speaker._3DMode = ThreeDMode.Full3D;
		}
	}

	// Token: 0x04003B50 RID: 15184
	private float talkDistance;

	// Token: 0x04003B51 RID: 15185
	private float origTalkDistance;

	// Token: 0x04003B52 RID: 15186
	private bool is3DMode;

	// Token: 0x04003B53 RID: 15187
	private bool hasSetOrigValues;

	// Token: 0x04003B54 RID: 15188
	private USpeakPhotonSender3D sender;

	// Token: 0x04003B55 RID: 15189
	private USpeaker speaker;
}
