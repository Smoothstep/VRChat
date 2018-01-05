using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x02000599 RID: 1433
[RequireComponent(typeof(UILabel))]
[AddComponentMenu("NGUI/Interaction/Typewriter Effect")]
public class TypewriterEffect : MonoBehaviour
{
	// Token: 0x17000744 RID: 1860
	// (get) Token: 0x06002FF9 RID: 12281 RVA: 0x000EAC9A File Offset: 0x000E909A
	public bool isActive
	{
		get
		{
			return this.mActive;
		}
	}

	// Token: 0x06002FFA RID: 12282 RVA: 0x000EACA2 File Offset: 0x000E90A2
	public void ResetToBeginning()
	{
		this.Finish();
		this.mReset = true;
		this.mActive = true;
		this.mNextChar = 0f;
		this.mCurrentOffset = 0;
		this.Update();
	}

	// Token: 0x06002FFB RID: 12283 RVA: 0x000EACD0 File Offset: 0x000E90D0
	public void Finish()
	{
		if (this.mActive)
		{
			this.mActive = false;
			if (!this.mReset)
			{
				this.mCurrentOffset = this.mFullText.Length;
				this.mFade.Clear();
				this.mLabel.text = this.mFullText;
			}
			if (this.keepFullDimensions && this.scrollView != null)
			{
				this.scrollView.UpdatePosition();
			}
			TypewriterEffect.current = this;
			EventDelegate.Execute(this.onFinished);
			TypewriterEffect.current = null;
		}
	}

	// Token: 0x06002FFC RID: 12284 RVA: 0x000EAD65 File Offset: 0x000E9165
	private void OnEnable()
	{
		this.mReset = true;
		this.mActive = true;
	}

	// Token: 0x06002FFD RID: 12285 RVA: 0x000EAD78 File Offset: 0x000E9178
	private void Update()
	{
		if (!this.mActive)
		{
			return;
		}
		if (this.mReset)
		{
			this.mCurrentOffset = 0;
			this.mReset = false;
			this.mLabel = base.GetComponent<UILabel>();
			this.mFullText = this.mLabel.processedText;
			this.mFade.Clear();
			if (this.keepFullDimensions && this.scrollView != null)
			{
				this.scrollView.UpdatePosition();
			}
		}
		while (this.mCurrentOffset < this.mFullText.Length && this.mNextChar <= RealTime.time)
		{
			int num = this.mCurrentOffset;
			this.charsPerSecond = Mathf.Max(1, this.charsPerSecond);
			while (NGUIText.ParseSymbol(this.mFullText, ref this.mCurrentOffset))
			{
			}
			this.mCurrentOffset++;
			if (this.mCurrentOffset > this.mFullText.Length)
			{
				break;
			}
			float num2 = 1f / (float)this.charsPerSecond;
			char c = (num >= this.mFullText.Length) ? '\n' : this.mFullText[num];
			if (c == '\n')
			{
				num2 += this.delayOnNewLine;
			}
			else if (num + 1 == this.mFullText.Length || this.mFullText[num + 1] <= ' ')
			{
				if (c == '.')
				{
					if (num + 2 < this.mFullText.Length && this.mFullText[num + 1] == '.' && this.mFullText[num + 2] == '.')
					{
						num2 += this.delayOnPeriod * 3f;
						num += 2;
					}
					else
					{
						num2 += this.delayOnPeriod;
					}
				}
				else if (c == '!' || c == '?')
				{
					num2 += this.delayOnPeriod;
				}
			}
			if (this.mNextChar == 0f)
			{
				this.mNextChar = RealTime.time + num2;
			}
			else
			{
				this.mNextChar += num2;
			}
			if (this.fadeInTime != 0f)
			{
				TypewriterEffect.FadeEntry item = default(TypewriterEffect.FadeEntry);
				item.index = num;
				item.alpha = 0f;
				item.text = this.mFullText.Substring(num, this.mCurrentOffset - num);
				this.mFade.Add(item);
			}
			else
			{
				this.mLabel.text = ((!this.keepFullDimensions) ? this.mFullText.Substring(0, this.mCurrentOffset) : (this.mFullText.Substring(0, this.mCurrentOffset) + "[00]" + this.mFullText.Substring(this.mCurrentOffset)));
				if (!this.keepFullDimensions && this.scrollView != null)
				{
					this.scrollView.UpdatePosition();
				}
			}
		}
		if (this.mFade.size != 0)
		{
			int i = 0;
			while (i < this.mFade.size)
			{
				TypewriterEffect.FadeEntry value = this.mFade[i];
				value.alpha += RealTime.deltaTime / this.fadeInTime;
				if (value.alpha < 1f)
				{
					this.mFade[i] = value;
					i++;
				}
				else
				{
					this.mFade.RemoveAt(i);
				}
			}
			if (this.mFade.size == 0)
			{
				if (this.keepFullDimensions)
				{
					this.mLabel.text = this.mFullText.Substring(0, this.mCurrentOffset) + "[00]" + this.mFullText.Substring(this.mCurrentOffset);
				}
				else
				{
					this.mLabel.text = this.mFullText.Substring(0, this.mCurrentOffset);
				}
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int j = 0; j < this.mFade.size; j++)
				{
					TypewriterEffect.FadeEntry fadeEntry = this.mFade[j];
					if (j == 0)
					{
						stringBuilder.Append(this.mFullText.Substring(0, fadeEntry.index));
					}
					stringBuilder.Append('[');
					stringBuilder.Append(NGUIText.EncodeAlpha(fadeEntry.alpha));
					stringBuilder.Append(']');
					stringBuilder.Append(fadeEntry.text);
				}
				if (this.keepFullDimensions)
				{
					stringBuilder.Append("[00]");
					stringBuilder.Append(this.mFullText.Substring(this.mCurrentOffset));
				}
				this.mLabel.text = stringBuilder.ToString();
			}
		}
		else if (this.mCurrentOffset == this.mFullText.Length)
		{
			TypewriterEffect.current = this;
			EventDelegate.Execute(this.onFinished);
			TypewriterEffect.current = null;
			this.mActive = false;
		}
	}

	// Token: 0x04001A62 RID: 6754
	public static TypewriterEffect current;

	// Token: 0x04001A63 RID: 6755
	public int charsPerSecond = 20;

	// Token: 0x04001A64 RID: 6756
	public float fadeInTime;

	// Token: 0x04001A65 RID: 6757
	public float delayOnPeriod;

	// Token: 0x04001A66 RID: 6758
	public float delayOnNewLine;

	// Token: 0x04001A67 RID: 6759
	public UIScrollView scrollView;

	// Token: 0x04001A68 RID: 6760
	public bool keepFullDimensions;

	// Token: 0x04001A69 RID: 6761
	public List<EventDelegate> onFinished = new List<EventDelegate>();

	// Token: 0x04001A6A RID: 6762
	private UILabel mLabel;

	// Token: 0x04001A6B RID: 6763
	private string mFullText = string.Empty;

	// Token: 0x04001A6C RID: 6764
	private int mCurrentOffset;

	// Token: 0x04001A6D RID: 6765
	private float mNextChar;

	// Token: 0x04001A6E RID: 6766
	private bool mReset = true;

	// Token: 0x04001A6F RID: 6767
	private bool mActive;

	// Token: 0x04001A70 RID: 6768
	private BetterList<TypewriterEffect.FadeEntry> mFade = new BetterList<TypewriterEffect.FadeEntry>();

	// Token: 0x0200059A RID: 1434
	private struct FadeEntry
	{
		// Token: 0x04001A71 RID: 6769
		public int index;

		// Token: 0x04001A72 RID: 6770
		public string text;

		// Token: 0x04001A73 RID: 6771
		public float alpha;
	}
}
