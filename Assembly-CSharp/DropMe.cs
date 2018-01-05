using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000569 RID: 1385
public class DropMe : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
{
	// Token: 0x06002F56 RID: 12118 RVA: 0x000E6047 File Offset: 0x000E4447
	public void OnEnable()
	{
		if (this.containerImage != null)
		{
			this.normalColor = this.containerImage.color;
		}
	}

	// Token: 0x06002F57 RID: 12119 RVA: 0x000E606C File Offset: 0x000E446C
	public void OnDrop(PointerEventData data)
	{
		this.containerImage.color = this.normalColor;
		if (this.receivingImage == null)
		{
			return;
		}
		Sprite dropSprite = this.GetDropSprite(data);
		if (dropSprite != null)
		{
			this.receivingImage.overrideSprite = dropSprite;
		}
	}

	// Token: 0x06002F58 RID: 12120 RVA: 0x000E60BC File Offset: 0x000E44BC
	public void OnPointerEnter(PointerEventData data)
	{
		if (this.containerImage == null)
		{
			return;
		}
		Sprite dropSprite = this.GetDropSprite(data);
		if (dropSprite != null)
		{
			this.containerImage.color = this.highlightColor;
		}
	}

	// Token: 0x06002F59 RID: 12121 RVA: 0x000E6100 File Offset: 0x000E4500
	public void OnPointerExit(PointerEventData data)
	{
		if (this.containerImage == null)
		{
			return;
		}
		this.containerImage.color = this.normalColor;
	}

	// Token: 0x06002F5A RID: 12122 RVA: 0x000E6128 File Offset: 0x000E4528
	private Sprite GetDropSprite(PointerEventData data)
	{
		GameObject pointerDrag = data.pointerDrag;
		if (pointerDrag == null)
		{
			return null;
		}
		Image component = pointerDrag.GetComponent<Image>();
		if (component == null)
		{
			return null;
		}
		return component.sprite;
	}

	// Token: 0x04001994 RID: 6548
	public Image containerImage;

	// Token: 0x04001995 RID: 6549
	public Image receivingImage;

	// Token: 0x04001996 RID: 6550
	private Color normalColor;

	// Token: 0x04001997 RID: 6551
	public Color highlightColor = Color.yellow;
}
