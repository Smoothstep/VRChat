using System;
using UnityEngine;

// Token: 0x0200064D RID: 1613
[ExecuteInEditMode]
[RequireComponent(typeof(UIWidget))]
[AddComponentMenu("NGUI/UI/Localize")]
public class UILocalize : MonoBehaviour
{
	// Token: 0x1700086A RID: 2154
	// (set) Token: 0x0600363D RID: 13885 RVA: 0x0011429C File Offset: 0x0011269C
	public string value
	{
		set
		{
			if (!string.IsNullOrEmpty(value))
			{
				UIWidget component = base.GetComponent<UIWidget>();
				UILabel uilabel = component as UILabel;
				UISprite uisprite = component as UISprite;
				if (uilabel != null)
				{
					UIInput uiinput = NGUITools.FindInParents<UIInput>(uilabel.gameObject);
					if (uiinput != null && uiinput.label == uilabel)
					{
						uiinput.defaultText = value;
					}
					else
					{
						uilabel.text = value;
					}
				}
				else if (uisprite != null)
				{
					UIButton uibutton = NGUITools.FindInParents<UIButton>(uisprite.gameObject);
					if (uibutton != null && uibutton.tweenTarget == uisprite.gameObject)
					{
						uibutton.normalSprite = value;
					}
					uisprite.spriteName = value;
					uisprite.MakePixelPerfect();
				}
			}
		}
	}

	// Token: 0x0600363E RID: 13886 RVA: 0x00114368 File Offset: 0x00112768
	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.OnLocalize();
		}
	}

	// Token: 0x0600363F RID: 13887 RVA: 0x0011437B File Offset: 0x0011277B
	private void Start()
	{
		this.mStarted = true;
		this.OnLocalize();
	}

	// Token: 0x06003640 RID: 13888 RVA: 0x0011438C File Offset: 0x0011278C
	private void OnLocalize()
	{
		if (string.IsNullOrEmpty(this.key))
		{
			UILabel component = base.GetComponent<UILabel>();
			if (component != null)
			{
				this.key = component.text;
			}
		}
		if (!string.IsNullOrEmpty(this.key))
		{
			this.value = Localization.Get(this.key);
		}
	}

	// Token: 0x04001F4E RID: 8014
	public string key;

	// Token: 0x04001F4F RID: 8015
	private bool mStarted;
}
