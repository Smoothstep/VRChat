using System;
using System.Text;
using UnityEngine;

// Token: 0x0200065B RID: 1627
[AddComponentMenu("NGUI/UI/Text List")]
public class UITextList : MonoBehaviour
{
	// Token: 0x17000899 RID: 2201
	// (get) Token: 0x060036DA RID: 14042 RVA: 0x00118C36 File Offset: 0x00117036
	public bool isValid
	{
		get
		{
			return this.textLabel != null && this.textLabel.ambigiousFont != null;
		}
	}

	// Token: 0x1700089A RID: 2202
	// (get) Token: 0x060036DB RID: 14043 RVA: 0x00118C5D File Offset: 0x0011705D
	// (set) Token: 0x060036DC RID: 14044 RVA: 0x00118C68 File Offset: 0x00117068
	public float scrollValue
	{
		get
		{
			return this.mScroll;
		}
		set
		{
			value = Mathf.Clamp01(value);
			if (this.isValid && this.mScroll != value)
			{
				if (this.scrollBar != null)
				{
					this.scrollBar.value = value;
				}
				else
				{
					this.mScroll = value;
					this.UpdateVisibleText();
				}
			}
		}
	}

	// Token: 0x1700089B RID: 2203
	// (get) Token: 0x060036DD RID: 14045 RVA: 0x00118CC3 File Offset: 0x001170C3
	protected float lineHeight
	{
		get
		{
			return (!(this.textLabel != null)) ? 20f : ((float)this.textLabel.fontSize + this.textLabel.effectiveSpacingY);
		}
	}

	// Token: 0x1700089C RID: 2204
	// (get) Token: 0x060036DE RID: 14046 RVA: 0x00118CF8 File Offset: 0x001170F8
	protected int scrollHeight
	{
		get
		{
			if (!this.isValid)
			{
				return 0;
			}
			int num = Mathf.FloorToInt((float)this.textLabel.height / this.lineHeight);
			return Mathf.Max(0, this.mTotalLines - num);
		}
	}

	// Token: 0x060036DF RID: 14047 RVA: 0x00118D39 File Offset: 0x00117139
	public void Clear()
	{
		this.mParagraphs.Clear();
		this.UpdateVisibleText();
	}

	// Token: 0x060036E0 RID: 14048 RVA: 0x00118D4C File Offset: 0x0011714C
	private void Start()
	{
		if (this.textLabel == null)
		{
			this.textLabel = base.GetComponentInChildren<UILabel>();
		}
		if (this.scrollBar != null)
		{
			EventDelegate.Add(this.scrollBar.onChange, new EventDelegate.Callback(this.OnScrollBar));
		}
		this.textLabel.overflowMethod = UILabel.Overflow.ClampContent;
		if (this.style == UITextList.Style.Chat)
		{
			this.textLabel.pivot = UIWidget.Pivot.BottomLeft;
			this.scrollValue = 1f;
		}
		else
		{
			this.textLabel.pivot = UIWidget.Pivot.TopLeft;
			this.scrollValue = 0f;
		}
	}

	// Token: 0x060036E1 RID: 14049 RVA: 0x00118DF0 File Offset: 0x001171F0
	private void Update()
	{
		if (this.isValid && (this.textLabel.width != this.mLastWidth || this.textLabel.height != this.mLastHeight))
		{
			this.mLastWidth = this.textLabel.width;
			this.mLastHeight = this.textLabel.height;
			this.Rebuild();
		}
	}

	// Token: 0x060036E2 RID: 14050 RVA: 0x00118E5C File Offset: 0x0011725C
	public void OnScroll(float val)
	{
		int scrollHeight = this.scrollHeight;
		if (scrollHeight != 0)
		{
			val *= this.lineHeight;
			this.scrollValue = this.mScroll - val / (float)scrollHeight;
		}
	}

	// Token: 0x060036E3 RID: 14051 RVA: 0x00118E94 File Offset: 0x00117294
	public void OnDrag(Vector2 delta)
	{
		int scrollHeight = this.scrollHeight;
		if (scrollHeight != 0)
		{
			float num = delta.y / this.lineHeight;
			this.scrollValue = this.mScroll + num / (float)scrollHeight;
		}
	}

	// Token: 0x060036E4 RID: 14052 RVA: 0x00118ECE File Offset: 0x001172CE
	private void OnScrollBar()
	{
		this.mScroll = UIProgressBar.current.value;
		this.UpdateVisibleText();
	}

	// Token: 0x060036E5 RID: 14053 RVA: 0x00118EE6 File Offset: 0x001172E6
	public void Add(string text)
	{
		this.Add(text, true);
	}

	// Token: 0x060036E6 RID: 14054 RVA: 0x00118EF0 File Offset: 0x001172F0
	protected void Add(string text, bool updateVisible)
	{
		UITextList.Paragraph paragraph;
		if (this.mParagraphs.size < this.paragraphHistory)
		{
			paragraph = new UITextList.Paragraph();
		}
		else
		{
			paragraph = this.mParagraphs[0];
			this.mParagraphs.RemoveAt(0);
		}
		paragraph.text = text;
		this.mParagraphs.Add(paragraph);
		this.Rebuild();
	}

	// Token: 0x060036E7 RID: 14055 RVA: 0x00118F54 File Offset: 0x00117354
	protected void Rebuild()
	{
		if (this.isValid)
		{
			this.textLabel.UpdateNGUIText();
			NGUIText.rectHeight = 1000000;
			this.mTotalLines = 0;
			for (int i = 0; i < this.mParagraphs.size; i++)
			{
				UITextList.Paragraph paragraph = this.mParagraphs.buffer[i];
				string text;
				NGUIText.WrapText(paragraph.text, out text);
				paragraph.lines = text.Split(new char[]
				{
					'\n'
				});
				this.mTotalLines += paragraph.lines.Length;
			}
			this.mTotalLines = 0;
			int j = 0;
			int size = this.mParagraphs.size;
			while (j < size)
			{
				this.mTotalLines += this.mParagraphs.buffer[j].lines.Length;
				j++;
			}
			if (this.scrollBar != null)
			{
				UIScrollBar uiscrollBar = this.scrollBar as UIScrollBar;
				if (uiscrollBar != null)
				{
					uiscrollBar.barSize = ((this.mTotalLines != 0) ? (1f - (float)this.scrollHeight / (float)this.mTotalLines) : 1f);
				}
			}
			this.UpdateVisibleText();
		}
	}

	// Token: 0x060036E8 RID: 14056 RVA: 0x00119098 File Offset: 0x00117498
	protected void UpdateVisibleText()
	{
		if (this.isValid)
		{
			if (this.mTotalLines == 0)
			{
				this.textLabel.text = string.Empty;
				return;
			}
			int num = Mathf.FloorToInt((float)this.textLabel.height / this.lineHeight);
			int num2 = Mathf.Max(0, this.mTotalLines - num);
			int num3 = Mathf.RoundToInt(this.mScroll * (float)num2);
			if (num3 < 0)
			{
				num3 = 0;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num4 = 0;
			int size = this.mParagraphs.size;
			while (num > 0 && num4 < size)
			{
				UITextList.Paragraph paragraph = this.mParagraphs.buffer[num4];
				int num5 = 0;
				int num6 = paragraph.lines.Length;
				while (num > 0 && num5 < num6)
				{
					string value = paragraph.lines[num5];
					if (num3 > 0)
					{
						num3--;
					}
					else
					{
						if (stringBuilder.Length > 0)
						{
							stringBuilder.Append("\n");
						}
						stringBuilder.Append(value);
						num--;
					}
					num5++;
				}
				num4++;
			}
			this.textLabel.text = stringBuilder.ToString();
		}
	}

	// Token: 0x04001FCA RID: 8138
	public UILabel textLabel;

	// Token: 0x04001FCB RID: 8139
	public UIProgressBar scrollBar;

	// Token: 0x04001FCC RID: 8140
	public UITextList.Style style;

	// Token: 0x04001FCD RID: 8141
	public int paragraphHistory = 50;

	// Token: 0x04001FCE RID: 8142
	protected char[] mSeparator = new char[]
	{
		'\n'
	};

	// Token: 0x04001FCF RID: 8143
	protected BetterList<UITextList.Paragraph> mParagraphs = new BetterList<UITextList.Paragraph>();

	// Token: 0x04001FD0 RID: 8144
	protected float mScroll;

	// Token: 0x04001FD1 RID: 8145
	protected int mTotalLines;

	// Token: 0x04001FD2 RID: 8146
	protected int mLastWidth;

	// Token: 0x04001FD3 RID: 8147
	protected int mLastHeight;

	// Token: 0x0200065C RID: 1628
	public enum Style
	{
		// Token: 0x04001FD5 RID: 8149
		Text,
		// Token: 0x04001FD6 RID: 8150
		Chat
	}

	// Token: 0x0200065D RID: 1629
	protected class Paragraph
	{
		// Token: 0x04001FD7 RID: 8151
		public string text;

		// Token: 0x04001FD8 RID: 8152
		public string[] lines;
	}
}
