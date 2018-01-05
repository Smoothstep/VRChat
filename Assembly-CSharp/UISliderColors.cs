using System;
using UnityEngine;

// Token: 0x02000595 RID: 1429
[AddComponentMenu("NGUI/Examples/Slider Colors")]
public class UISliderColors : MonoBehaviour
{
	// Token: 0x06002FEC RID: 12268 RVA: 0x000EA8D6 File Offset: 0x000E8CD6
	private void Start()
	{
		this.mBar = base.GetComponent<UIProgressBar>();
		this.mSprite = base.GetComponent<UIBasicSprite>();
		this.Update();
	}

	// Token: 0x06002FED RID: 12269 RVA: 0x000EA8F8 File Offset: 0x000E8CF8
	private void Update()
	{
		if (this.sprite == null || this.colors.Length == 0)
		{
			return;
		}
		float num = (!(this.mBar != null)) ? this.mSprite.fillAmount : this.mBar.value;
		num *= (float)(this.colors.Length - 1);
		int num2 = Mathf.FloorToInt(num);
		Color color = this.colors[0];
		if (num2 >= 0)
		{
			if (num2 + 1 < this.colors.Length)
			{
				float t = num - (float)num2;
				color = Color.Lerp(this.colors[num2], this.colors[num2 + 1], t);
			}
			else if (num2 < this.colors.Length)
			{
				color = this.colors[num2];
			}
			else
			{
				color = this.colors[this.colors.Length - 1];
			}
		}
		color.a = this.sprite.color.a;
		this.sprite.color = color;
	}

	// Token: 0x04001A54 RID: 6740
	public UISprite sprite;

	// Token: 0x04001A55 RID: 6741
	public Color[] colors = new Color[]
	{
		Color.red,
		Color.yellow,
		Color.green
	};

	// Token: 0x04001A56 RID: 6742
	private UIProgressBar mBar;

	// Token: 0x04001A57 RID: 6743
	private UIBasicSprite mSprite;
}
