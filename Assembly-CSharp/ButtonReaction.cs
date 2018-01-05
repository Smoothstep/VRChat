using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000C1A RID: 3098
public class ButtonReaction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x06005FEE RID: 24558 RVA: 0x0021BC24 File Offset: 0x0021A024
	public void Start()
	{
		if (this.audioSource == null)
		{
			this.audioSource = base.gameObject.AddComponent<AudioSource>();
			this.audioSource.outputAudioMixerGroup = VRCAudioManager.GetUiGroup();
			this.audioSource.volume = 0.25f;
		}
		if (this.mouseIn == null && this.ui_file_sounds != null && this.ui_file_sounds.MoveOver != null)
		{
			this.mouseIn = this.ui_file_sounds.MoveOver;
		}
		this.mouseOut = this.ui_file_sounds.MoveOff;
		this.mouseClick = this.ui_file_sounds.Click;
	}

	// Token: 0x06005FEF RID: 24559 RVA: 0x0021BCDE File Offset: 0x0021A0DE
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (this.mouseIn != null)
		{
			this.audioSource.PlayOneShot(this.mouseIn);
		}
	}

	// Token: 0x06005FF0 RID: 24560 RVA: 0x0021BD02 File Offset: 0x0021A102
	public void OnPointerExit(PointerEventData eventData)
	{
		if (this.mouseOut != null)
		{
			this.audioSource.PlayOneShot(this.mouseOut);
		}
	}

	// Token: 0x06005FF1 RID: 24561 RVA: 0x0021BD26 File Offset: 0x0021A126
	public void OnPointerClick(PointerEventData eventData)
	{
		if (this.mouseClick != null)
		{
			this.audioSource.PlayOneShot(this.mouseClick);
		}
	}

	// Token: 0x0400457B RID: 17787
	public AudioClip mouseIn;

	// Token: 0x0400457C RID: 17788
	public AudioClip mouseOut;

	// Token: 0x0400457D RID: 17789
	public AudioClip mouseClick;

	// Token: 0x0400457E RID: 17790
	public UISoundCollection ui_file_sounds;

	// Token: 0x0400457F RID: 17791
	public AudioSource audioSource;
}
