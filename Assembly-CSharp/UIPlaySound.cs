using System;
using UnityEngine;

// Token: 0x020005BF RID: 1471
[AddComponentMenu("NGUI/Interaction/Play Sound")]
public class UIPlaySound : MonoBehaviour
{
	// Token: 0x17000751 RID: 1873
	// (get) Token: 0x060030E9 RID: 12521 RVA: 0x000EFCFC File Offset: 0x000EE0FC
	private bool canPlay
	{
		get
		{
			if (!base.enabled)
			{
				return false;
			}
			UIButton component = base.GetComponent<UIButton>();
			return component == null || component.isEnabled;
		}
	}

	// Token: 0x060030EA RID: 12522 RVA: 0x000EFD32 File Offset: 0x000EE132
	private void OnEnable()
	{
		if (this.trigger == UIPlaySound.Trigger.OnEnable)
		{
			NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
		}
	}

	// Token: 0x060030EB RID: 12523 RVA: 0x000EFD58 File Offset: 0x000EE158
	private void OnDisable()
	{
		if (this.trigger == UIPlaySound.Trigger.OnDisable)
		{
			NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
		}
	}

	// Token: 0x060030EC RID: 12524 RVA: 0x000EFD80 File Offset: 0x000EE180
	private void OnHover(bool isOver)
	{
		if (this.trigger == UIPlaySound.Trigger.OnMouseOver)
		{
			if (this.mIsOver == isOver)
			{
				return;
			}
			this.mIsOver = isOver;
		}
		if (this.canPlay && ((isOver && this.trigger == UIPlaySound.Trigger.OnMouseOver) || (!isOver && this.trigger == UIPlaySound.Trigger.OnMouseOut)))
		{
			NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
		}
	}

	// Token: 0x060030ED RID: 12525 RVA: 0x000EFDF4 File Offset: 0x000EE1F4
	private void OnPress(bool isPressed)
	{
		if (this.trigger == UIPlaySound.Trigger.OnPress)
		{
			if (this.mIsOver == isPressed)
			{
				return;
			}
			this.mIsOver = isPressed;
		}
		if (this.canPlay && ((isPressed && this.trigger == UIPlaySound.Trigger.OnPress) || (!isPressed && this.trigger == UIPlaySound.Trigger.OnRelease)))
		{
			NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
		}
	}

	// Token: 0x060030EE RID: 12526 RVA: 0x000EFE68 File Offset: 0x000EE268
	private void OnClick()
	{
		if (this.canPlay && this.trigger == UIPlaySound.Trigger.OnClick)
		{
			NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
		}
	}

	// Token: 0x060030EF RID: 12527 RVA: 0x000EFE98 File Offset: 0x000EE298
	private void OnSelect(bool isSelected)
	{
		if (this.canPlay && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
		{
			this.OnHover(isSelected);
		}
	}

	// Token: 0x060030F0 RID: 12528 RVA: 0x000EFEBD File Offset: 0x000EE2BD
	public void Play()
	{
		NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
	}

	// Token: 0x04001B70 RID: 7024
	public AudioClip audioClip;

	// Token: 0x04001B71 RID: 7025
	public UIPlaySound.Trigger trigger;

	// Token: 0x04001B72 RID: 7026
	[Range(0f, 1f)]
	public float volume = 1f;

	// Token: 0x04001B73 RID: 7027
	[Range(0f, 2f)]
	public float pitch = 1f;

	// Token: 0x04001B74 RID: 7028
	private bool mIsOver;

	// Token: 0x020005C0 RID: 1472
	public enum Trigger
	{
		// Token: 0x04001B76 RID: 7030
		OnClick,
		// Token: 0x04001B77 RID: 7031
		OnMouseOver,
		// Token: 0x04001B78 RID: 7032
		OnMouseOut,
		// Token: 0x04001B79 RID: 7033
		OnPress,
		// Token: 0x04001B7A RID: 7034
		OnRelease,
		// Token: 0x04001B7B RID: 7035
		Custom,
		// Token: 0x04001B7C RID: 7036
		OnEnable,
		// Token: 0x04001B7D RID: 7037
		OnDisable
	}
}
