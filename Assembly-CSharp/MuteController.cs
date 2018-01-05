using System;
using UnityEngine;

// Token: 0x02000AC5 RID: 2757
[RequireComponent(typeof(USpeaker))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(OVRLipSyncContext))]
public class MuteController : MonoBehaviour
{
	// Token: 0x17000C32 RID: 3122
	// (get) Token: 0x060053BB RID: 21435 RVA: 0x001CE23C File Offset: 0x001CC63C
	// (set) Token: 0x060053BC RID: 21436 RVA: 0x001CE244 File Offset: 0x001CC644
	public bool mute
	{
		get
		{
			return this._isMuted;
		}
		set
		{
			this._isMuted = value;
			this.Reset();
		}
	}

	// Token: 0x060053BD RID: 21437 RVA: 0x001CE253 File Offset: 0x001CC653
	private void Start()
	{
		this._audiosrc = base.GetComponent<AudioSource>();
		this._ovrls = base.GetComponent<OVRLipSyncContext>();
	}

	// Token: 0x060053BE RID: 21438 RVA: 0x001CE270 File Offset: 0x001CC670
	public void LocalMute()
	{
		ONSPAudioSource component = base.GetComponent<ONSPAudioSource>();
		if (component != null)
		{
			component.EnableSpatialization = false;
		}
		if (this._ovrls != null && this._ovrls.enabled)
		{
			this._audiosrc.mute = false;
			this._ovrls.audioMute = true;
		}
		else if (this._audiosrc != null)
		{
			this._audiosrc.mute = true;
		}
	}

	// Token: 0x060053BF RID: 21439 RVA: 0x001CE2F4 File Offset: 0x001CC6F4
	public void Reset()
	{
		if (this._ovrls == null || this._audiosrc == null)
		{
			return;
		}
		if (this._ovrls.enabled)
		{
			this._ovrls.audioMute = this._isMuted;
			this._audiosrc.mute = false;
		}
		else
		{
			this._audiosrc.mute = this._isMuted;
			this._ovrls.audioMute = false;
		}
	}

	// Token: 0x04003B15 RID: 15125
	private bool _isMuted;

	// Token: 0x04003B16 RID: 15126
	private AudioSource _audiosrc;

	// Token: 0x04003B17 RID: 15127
	private OVRLipSyncContext _ovrls;
}
