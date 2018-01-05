using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000649 RID: 1609
[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/NGUI Label")]
public class UILabel : UIWidget
{
	// Token: 0x17000840 RID: 2112
	// (get) Token: 0x060035D0 RID: 13776 RVA: 0x00112001 File Offset: 0x00110401
	// (set) Token: 0x060035D1 RID: 13777 RVA: 0x00112009 File Offset: 0x00110409
	private bool shouldBeProcessed
	{
		get
		{
			return this.mShouldBeProcessed;
		}
		set
		{
			if (value)
			{
				this.mChanged = true;
				this.mShouldBeProcessed = true;
			}
			else
			{
				this.mShouldBeProcessed = false;
			}
		}
	}

	// Token: 0x17000841 RID: 2113
	// (get) Token: 0x060035D2 RID: 13778 RVA: 0x0011202B File Offset: 0x0011042B
	public override bool isAnchoredHorizontally
	{
		get
		{
			return base.isAnchoredHorizontally || this.mOverflow == UILabel.Overflow.ResizeFreely;
		}
	}

	// Token: 0x17000842 RID: 2114
	// (get) Token: 0x060035D3 RID: 13779 RVA: 0x00112044 File Offset: 0x00110444
	public override bool isAnchoredVertically
	{
		get
		{
			return base.isAnchoredVertically || this.mOverflow == UILabel.Overflow.ResizeFreely || this.mOverflow == UILabel.Overflow.ResizeHeight;
		}
	}

	// Token: 0x17000843 RID: 2115
	// (get) Token: 0x060035D4 RID: 13780 RVA: 0x0011206C File Offset: 0x0011046C
	// (set) Token: 0x060035D5 RID: 13781 RVA: 0x001120CC File Offset: 0x001104CC
	public override Material material
	{
		get
		{
			if (this.mMaterial != null)
			{
				return this.mMaterial;
			}
			if (this.mFont != null)
			{
				return this.mFont.material;
			}
			if (this.mTrueTypeFont != null)
			{
				return this.mTrueTypeFont.material;
			}
			return null;
		}
		set
		{
			if (this.mMaterial != value)
			{
				base.RemoveFromPanel();
				this.mMaterial = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x17000844 RID: 2116
	// (get) Token: 0x060035D6 RID: 13782 RVA: 0x001120F2 File Offset: 0x001104F2
	// (set) Token: 0x060035D7 RID: 13783 RVA: 0x001120FA File Offset: 0x001104FA
	[Obsolete("Use UILabel.bitmapFont instead")]
	public UIFont font
	{
		get
		{
			return this.bitmapFont;
		}
		set
		{
			this.bitmapFont = value;
		}
	}

	// Token: 0x17000845 RID: 2117
	// (get) Token: 0x060035D8 RID: 13784 RVA: 0x00112103 File Offset: 0x00110503
	// (set) Token: 0x060035D9 RID: 13785 RVA: 0x0011210B File Offset: 0x0011050B
	public UIFont bitmapFont
	{
		get
		{
			return this.mFont;
		}
		set
		{
			if (this.mFont != value)
			{
				base.RemoveFromPanel();
				this.mFont = value;
				this.mTrueTypeFont = null;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x17000846 RID: 2118
	// (get) Token: 0x060035DA RID: 13786 RVA: 0x00112138 File Offset: 0x00110538
	// (set) Token: 0x060035DB RID: 13787 RVA: 0x00112174 File Offset: 0x00110574
	public Font trueTypeFont
	{
		get
		{
			if (this.mTrueTypeFont != null)
			{
				return this.mTrueTypeFont;
			}
			return (!(this.mFont != null)) ? null : this.mFont.dynamicFont;
		}
		set
		{
			if (this.mTrueTypeFont != value)
			{
				this.SetActiveFont(null);
				base.RemoveFromPanel();
				this.mTrueTypeFont = value;
				this.shouldBeProcessed = true;
				this.mFont = null;
				this.SetActiveFont(value);
				this.ProcessAndRequest();
				if (this.mActiveTTF != null)
				{
					base.MarkAsChanged();
				}
			}
		}
	}

	// Token: 0x17000847 RID: 2119
	// (get) Token: 0x060035DC RID: 13788 RVA: 0x001121D8 File Offset: 0x001105D8
	// (set) Token: 0x060035DD RID: 13789 RVA: 0x001121FC File Offset: 0x001105FC
	public UnityEngine.Object ambigiousFont
	{
        get
        {
            if ((UnityEngine.Object)this.mFont != (UnityEngine.Object)null)
                return (UnityEngine.Object)this.mFont;
            return (UnityEngine.Object)this.mTrueTypeFont;
        }
        set
        {
            UIFont uiFont = value as UIFont;
            if ((UnityEngine.Object)uiFont != (UnityEngine.Object)null)
                this.bitmapFont = uiFont;
            else
                this.trueTypeFont = value as Font;
        }
    }

	// Token: 0x17000848 RID: 2120
	// (get) Token: 0x060035DE RID: 13790 RVA: 0x00112234 File Offset: 0x00110634
	// (set) Token: 0x060035DF RID: 13791 RVA: 0x0011223C File Offset: 0x0011063C
	public string text
	{
		get
		{
			return this.mText;
		}
		set
		{
			if (this.mText == value)
			{
				return;
			}
			if (string.IsNullOrEmpty(value))
			{
				if (!string.IsNullOrEmpty(this.mText))
				{
					this.mText = string.Empty;
					this.MarkAsChanged();
					this.ProcessAndRequest();
				}
			}
			else if (this.mText != value)
			{
				this.mText = value;
				this.MarkAsChanged();
				this.ProcessAndRequest();
			}
			if (this.autoResizeBoxCollider)
			{
				base.ResizeCollider();
			}
		}
	}

	// Token: 0x17000849 RID: 2121
	// (get) Token: 0x060035E0 RID: 13792 RVA: 0x001122C8 File Offset: 0x001106C8
	public int defaultFontSize
	{
		get
		{
			return (!(this.trueTypeFont != null)) ? ((!(this.mFont != null)) ? 16 : this.mFont.defaultSize) : this.mFontSize;
		}
	}

	// Token: 0x1700084A RID: 2122
	// (get) Token: 0x060035E1 RID: 13793 RVA: 0x00112314 File Offset: 0x00110714
	// (set) Token: 0x060035E2 RID: 13794 RVA: 0x0011231C File Offset: 0x0011071C
	public int fontSize
	{
		get
		{
			return this.mFontSize;
		}
		set
		{
			value = Mathf.Clamp(value, 0, 256);
			if (this.mFontSize != value)
			{
				this.mFontSize = value;
				this.shouldBeProcessed = true;
				this.ProcessAndRequest();
			}
		}
	}

	// Token: 0x1700084B RID: 2123
	// (get) Token: 0x060035E3 RID: 13795 RVA: 0x0011234C File Offset: 0x0011074C
	// (set) Token: 0x060035E4 RID: 13796 RVA: 0x00112354 File Offset: 0x00110754
	public FontStyle fontStyle
	{
		get
		{
			return this.mFontStyle;
		}
		set
		{
			if (this.mFontStyle != value)
			{
				this.mFontStyle = value;
				this.shouldBeProcessed = true;
				this.ProcessAndRequest();
			}
		}
	}

	// Token: 0x1700084C RID: 2124
	// (get) Token: 0x060035E5 RID: 13797 RVA: 0x00112376 File Offset: 0x00110776
	// (set) Token: 0x060035E6 RID: 13798 RVA: 0x0011237E File Offset: 0x0011077E
	public NGUIText.Alignment alignment
	{
		get
		{
			return this.mAlignment;
		}
		set
		{
			if (this.mAlignment != value)
			{
				this.mAlignment = value;
				this.shouldBeProcessed = true;
				this.ProcessAndRequest();
			}
		}
	}

	// Token: 0x1700084D RID: 2125
	// (get) Token: 0x060035E7 RID: 13799 RVA: 0x001123A0 File Offset: 0x001107A0
	// (set) Token: 0x060035E8 RID: 13800 RVA: 0x001123A8 File Offset: 0x001107A8
	public bool applyGradient
	{
		get
		{
			return this.mApplyGradient;
		}
		set
		{
			if (this.mApplyGradient != value)
			{
				this.mApplyGradient = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x1700084E RID: 2126
	// (get) Token: 0x060035E9 RID: 13801 RVA: 0x001123C3 File Offset: 0x001107C3
	// (set) Token: 0x060035EA RID: 13802 RVA: 0x001123CB File Offset: 0x001107CB
	public Color gradientTop
	{
		get
		{
			return this.mGradientTop;
		}
		set
		{
			if (this.mGradientTop != value)
			{
				this.mGradientTop = value;
				if (this.mApplyGradient)
				{
					this.MarkAsChanged();
				}
			}
		}
	}

	// Token: 0x1700084F RID: 2127
	// (get) Token: 0x060035EB RID: 13803 RVA: 0x001123F6 File Offset: 0x001107F6
	// (set) Token: 0x060035EC RID: 13804 RVA: 0x001123FE File Offset: 0x001107FE
	public Color gradientBottom
	{
		get
		{
			return this.mGradientBottom;
		}
		set
		{
			if (this.mGradientBottom != value)
			{
				this.mGradientBottom = value;
				if (this.mApplyGradient)
				{
					this.MarkAsChanged();
				}
			}
		}
	}

	// Token: 0x17000850 RID: 2128
	// (get) Token: 0x060035ED RID: 13805 RVA: 0x00112429 File Offset: 0x00110829
	// (set) Token: 0x060035EE RID: 13806 RVA: 0x00112431 File Offset: 0x00110831
	public int spacingX
	{
		get
		{
			return this.mSpacingX;
		}
		set
		{
			if (this.mSpacingX != value)
			{
				this.mSpacingX = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x17000851 RID: 2129
	// (get) Token: 0x060035EF RID: 13807 RVA: 0x0011244C File Offset: 0x0011084C
	// (set) Token: 0x060035F0 RID: 13808 RVA: 0x00112454 File Offset: 0x00110854
	public int spacingY
	{
		get
		{
			return this.mSpacingY;
		}
		set
		{
			if (this.mSpacingY != value)
			{
				this.mSpacingY = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x17000852 RID: 2130
	// (get) Token: 0x060035F1 RID: 13809 RVA: 0x0011246F File Offset: 0x0011086F
	// (set) Token: 0x060035F2 RID: 13810 RVA: 0x00112477 File Offset: 0x00110877
	public bool useFloatSpacing
	{
		get
		{
			return this.mUseFloatSpacing;
		}
		set
		{
			if (this.mUseFloatSpacing != value)
			{
				this.mUseFloatSpacing = value;
				this.shouldBeProcessed = true;
			}
		}
	}

	// Token: 0x17000853 RID: 2131
	// (get) Token: 0x060035F3 RID: 13811 RVA: 0x00112493 File Offset: 0x00110893
	// (set) Token: 0x060035F4 RID: 13812 RVA: 0x0011249B File Offset: 0x0011089B
	public float floatSpacingX
	{
		get
		{
			return this.mFloatSpacingX;
		}
		set
		{
			if (!Mathf.Approximately(this.mFloatSpacingX, value))
			{
				this.mFloatSpacingX = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x17000854 RID: 2132
	// (get) Token: 0x060035F5 RID: 13813 RVA: 0x001124BB File Offset: 0x001108BB
	// (set) Token: 0x060035F6 RID: 13814 RVA: 0x001124C3 File Offset: 0x001108C3
	public float floatSpacingY
	{
		get
		{
			return this.mFloatSpacingY;
		}
		set
		{
			if (!Mathf.Approximately(this.mFloatSpacingY, value))
			{
				this.mFloatSpacingY = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x17000855 RID: 2133
	// (get) Token: 0x060035F7 RID: 13815 RVA: 0x001124E3 File Offset: 0x001108E3
	public float effectiveSpacingY
	{
		get
		{
			return (!this.mUseFloatSpacing) ? ((float)this.mSpacingY) : this.mFloatSpacingY;
		}
	}

	// Token: 0x17000856 RID: 2134
	// (get) Token: 0x060035F8 RID: 13816 RVA: 0x00112502 File Offset: 0x00110902
	public float effectiveSpacingX
	{
		get
		{
			return (!this.mUseFloatSpacing) ? ((float)this.mSpacingX) : this.mFloatSpacingX;
		}
	}

	// Token: 0x17000857 RID: 2135
	// (get) Token: 0x060035F9 RID: 13817 RVA: 0x00112521 File Offset: 0x00110921
	private bool keepCrisp
	{
		get
		{
			return this.trueTypeFont != null && this.keepCrispWhenShrunk != UILabel.Crispness.Never;
		}
	}

	// Token: 0x17000858 RID: 2136
	// (get) Token: 0x060035FA RID: 13818 RVA: 0x00112542 File Offset: 0x00110942
	// (set) Token: 0x060035FB RID: 13819 RVA: 0x0011254A File Offset: 0x0011094A
	public bool supportEncoding
	{
		get
		{
			return this.mEncoding;
		}
		set
		{
			if (this.mEncoding != value)
			{
				this.mEncoding = value;
				this.shouldBeProcessed = true;
			}
		}
	}

	// Token: 0x17000859 RID: 2137
	// (get) Token: 0x060035FC RID: 13820 RVA: 0x00112566 File Offset: 0x00110966
	// (set) Token: 0x060035FD RID: 13821 RVA: 0x0011256E File Offset: 0x0011096E
	public NGUIText.SymbolStyle symbolStyle
	{
		get
		{
			return this.mSymbols;
		}
		set
		{
			if (this.mSymbols != value)
			{
				this.mSymbols = value;
				this.shouldBeProcessed = true;
			}
		}
	}

	// Token: 0x1700085A RID: 2138
	// (get) Token: 0x060035FE RID: 13822 RVA: 0x0011258A File Offset: 0x0011098A
	// (set) Token: 0x060035FF RID: 13823 RVA: 0x00112592 File Offset: 0x00110992
	public UILabel.Overflow overflowMethod
	{
		get
		{
			return this.mOverflow;
		}
		set
		{
			if (this.mOverflow != value)
			{
				this.mOverflow = value;
				this.shouldBeProcessed = true;
			}
		}
	}

	// Token: 0x1700085B RID: 2139
	// (get) Token: 0x06003600 RID: 13824 RVA: 0x001125AE File Offset: 0x001109AE
	// (set) Token: 0x06003601 RID: 13825 RVA: 0x001125B6 File Offset: 0x001109B6
	[Obsolete("Use 'width' instead")]
	public int lineWidth
	{
		get
		{
			return base.width;
		}
		set
		{
			base.width = value;
		}
	}

	// Token: 0x1700085C RID: 2140
	// (get) Token: 0x06003602 RID: 13826 RVA: 0x001125BF File Offset: 0x001109BF
	// (set) Token: 0x06003603 RID: 13827 RVA: 0x001125C7 File Offset: 0x001109C7
	[Obsolete("Use 'height' instead")]
	public int lineHeight
	{
		get
		{
			return base.height;
		}
		set
		{
			base.height = value;
		}
	}

	// Token: 0x1700085D RID: 2141
	// (get) Token: 0x06003604 RID: 13828 RVA: 0x001125D0 File Offset: 0x001109D0
	// (set) Token: 0x06003605 RID: 13829 RVA: 0x001125DE File Offset: 0x001109DE
	public bool multiLine
	{
		get
		{
			return this.mMaxLineCount != 1;
		}
		set
		{
			if (this.mMaxLineCount != 1 != value)
			{
				this.mMaxLineCount = ((!value) ? 1 : 0);
				this.shouldBeProcessed = true;
			}
		}
	}

	// Token: 0x1700085E RID: 2142
	// (get) Token: 0x06003606 RID: 13830 RVA: 0x0011260C File Offset: 0x00110A0C
	public override Vector3[] localCorners
	{
		get
		{
			if (this.shouldBeProcessed)
			{
				this.ProcessText();
			}
			return base.localCorners;
		}
	}

	// Token: 0x1700085F RID: 2143
	// (get) Token: 0x06003607 RID: 13831 RVA: 0x00112625 File Offset: 0x00110A25
	public override Vector3[] worldCorners
	{
		get
		{
			if (this.shouldBeProcessed)
			{
				this.ProcessText();
			}
			return base.worldCorners;
		}
	}

	// Token: 0x17000860 RID: 2144
	// (get) Token: 0x06003608 RID: 13832 RVA: 0x0011263E File Offset: 0x00110A3E
	public override Vector4 drawingDimensions
	{
		get
		{
			if (this.shouldBeProcessed)
			{
				this.ProcessText();
			}
			return base.drawingDimensions;
		}
	}

	// Token: 0x17000861 RID: 2145
	// (get) Token: 0x06003609 RID: 13833 RVA: 0x00112657 File Offset: 0x00110A57
	// (set) Token: 0x0600360A RID: 13834 RVA: 0x0011265F File Offset: 0x00110A5F
	public int maxLineCount
	{
		get
		{
			return this.mMaxLineCount;
		}
		set
		{
			if (this.mMaxLineCount != value)
			{
				this.mMaxLineCount = Mathf.Max(value, 0);
				this.shouldBeProcessed = true;
				if (this.overflowMethod == UILabel.Overflow.ShrinkContent)
				{
					this.MakePixelPerfect();
				}
			}
		}
	}

	// Token: 0x17000862 RID: 2146
	// (get) Token: 0x0600360B RID: 13835 RVA: 0x00112692 File Offset: 0x00110A92
	// (set) Token: 0x0600360C RID: 13836 RVA: 0x0011269A File Offset: 0x00110A9A
	public UILabel.Effect effectStyle
	{
		get
		{
			return this.mEffectStyle;
		}
		set
		{
			if (this.mEffectStyle != value)
			{
				this.mEffectStyle = value;
				this.shouldBeProcessed = true;
			}
		}
	}

	// Token: 0x17000863 RID: 2147
	// (get) Token: 0x0600360D RID: 13837 RVA: 0x001126B6 File Offset: 0x00110AB6
	// (set) Token: 0x0600360E RID: 13838 RVA: 0x001126BE File Offset: 0x00110ABE
	public Color effectColor
	{
		get
		{
			return this.mEffectColor;
		}
		set
		{
			if (this.mEffectColor != value)
			{
				this.mEffectColor = value;
				if (this.mEffectStyle != UILabel.Effect.None)
				{
					this.shouldBeProcessed = true;
				}
			}
		}
	}

	// Token: 0x17000864 RID: 2148
	// (get) Token: 0x0600360F RID: 13839 RVA: 0x001126EA File Offset: 0x00110AEA
	// (set) Token: 0x06003610 RID: 13840 RVA: 0x001126F2 File Offset: 0x00110AF2
	public Vector2 effectDistance
	{
		get
		{
			return this.mEffectDistance;
		}
		set
		{
			if (this.mEffectDistance != value)
			{
				this.mEffectDistance = value;
				this.shouldBeProcessed = true;
			}
		}
	}

	// Token: 0x17000865 RID: 2149
	// (get) Token: 0x06003611 RID: 13841 RVA: 0x00112713 File Offset: 0x00110B13
	// (set) Token: 0x06003612 RID: 13842 RVA: 0x0011271E File Offset: 0x00110B1E
	[Obsolete("Use 'overflowMethod == UILabel.Overflow.ShrinkContent' instead")]
	public bool shrinkToFit
	{
		get
		{
			return this.mOverflow == UILabel.Overflow.ShrinkContent;
		}
		set
		{
			if (value)
			{
				this.overflowMethod = UILabel.Overflow.ShrinkContent;
			}
		}
	}

	// Token: 0x17000866 RID: 2150
	// (get) Token: 0x06003613 RID: 13843 RVA: 0x00112730 File Offset: 0x00110B30
	public string processedText
	{
		get
		{
			if (this.mLastWidth != this.mWidth || this.mLastHeight != this.mHeight)
			{
				this.mLastWidth = this.mWidth;
				this.mLastHeight = this.mHeight;
				this.mShouldBeProcessed = true;
			}
			if (this.shouldBeProcessed)
			{
				this.ProcessText();
			}
			return this.mProcessedText;
		}
	}

	// Token: 0x17000867 RID: 2151
	// (get) Token: 0x06003614 RID: 13844 RVA: 0x00112795 File Offset: 0x00110B95
	public Vector2 printedSize
	{
		get
		{
			if (this.shouldBeProcessed)
			{
				this.ProcessText();
			}
			return this.mCalculatedSize;
		}
	}

	// Token: 0x17000868 RID: 2152
	// (get) Token: 0x06003615 RID: 13845 RVA: 0x001127AE File Offset: 0x00110BAE
	public override Vector2 localSize
	{
		get
		{
			if (this.shouldBeProcessed)
			{
				this.ProcessText();
			}
			return base.localSize;
		}
	}

	// Token: 0x17000869 RID: 2153
	// (get) Token: 0x06003616 RID: 13846 RVA: 0x001127C7 File Offset: 0x00110BC7
	private bool isValid
	{
		get
		{
			return this.mFont != null || this.mTrueTypeFont != null;
		}
	}

	// Token: 0x06003617 RID: 13847 RVA: 0x001127E9 File Offset: 0x00110BE9
	protected override void OnInit()
	{
		base.OnInit();
		UILabel.mList.Add(this);
		this.SetActiveFont(this.trueTypeFont);
	}

	// Token: 0x06003618 RID: 13848 RVA: 0x00112808 File Offset: 0x00110C08
	protected override void OnDisable()
	{
		this.SetActiveFont(null);
		UILabel.mList.Remove(this);
		base.OnDisable();
	}

	// Token: 0x06003619 RID: 13849 RVA: 0x00112824 File Offset: 0x00110C24
	protected void SetActiveFont(Font fnt)
	{
		if (this.mActiveTTF != fnt)
		{
			if (this.mActiveTTF != null)
			{
				int num;
				if (UILabel.mFontUsage.TryGetValue(this.mActiveTTF, out num))
				{
					num = Mathf.Max(0, --num);
					if (num == 0)
					{
						this.mActiveTTF.textureRebuildCallback = null;
						UILabel.mFontUsage.Remove(this.mActiveTTF);
					}
					else
					{
						UILabel.mFontUsage[this.mActiveTTF] = num;
					}
				}
				else
				{
					this.mActiveTTF.textureRebuildCallback = null;
				}
			}
			this.mActiveTTF = fnt;
			if (this.mActiveTTF != null)
			{
				int num2 = 0;
				if (!UILabel.mFontUsage.TryGetValue(this.mActiveTTF, out num2))
				{
					Font font = this.mActiveTTF;
					if (UILabel.f__mg0 == null)
					{
						UILabel.f__mg0 = new Font.FontTextureRebuildCallback(UILabel.OnFontTextureChanged);
					}
					font.textureRebuildCallback = UILabel.f__mg0;
				}
				num2 = (UILabel.mFontUsage[this.mActiveTTF] = num2 + 1);
			}
		}
	}

	// Token: 0x0600361A RID: 13850 RVA: 0x00112930 File Offset: 0x00110D30
	private static void OnFontTextureChanged()
	{
		for (int i = 0; i < UILabel.mList.size; i++)
		{
			UILabel uilabel = UILabel.mList[i];
			if (uilabel != null)
			{
				Font trueTypeFont = uilabel.trueTypeFont;
				if (trueTypeFont != null)
				{
					trueTypeFont.RequestCharactersInTexture(uilabel.mText, uilabel.mPrintedSize, uilabel.mFontStyle);
				}
			}
		}
		for (int j = 0; j < UILabel.mList.size; j++)
		{
			UILabel uilabel2 = UILabel.mList[j];
			if (uilabel2 != null)
			{
				Font trueTypeFont2 = uilabel2.trueTypeFont;
				if (trueTypeFont2 != null)
				{
					uilabel2.RemoveFromPanel();
					uilabel2.CreatePanel();
				}
			}
		}
	}

	// Token: 0x0600361B RID: 13851 RVA: 0x001129F5 File Offset: 0x00110DF5
	public override Vector3[] GetSides(Transform relativeTo)
	{
		if (this.shouldBeProcessed)
		{
			this.ProcessText();
		}
		return base.GetSides(relativeTo);
	}

	// Token: 0x0600361C RID: 13852 RVA: 0x00112A10 File Offset: 0x00110E10
	protected override void UpgradeFrom265()
	{
		this.ProcessText(true, true);
		if (this.mShrinkToFit)
		{
			this.overflowMethod = UILabel.Overflow.ShrinkContent;
			this.mMaxLineCount = 0;
		}
		if (this.mMaxLineWidth != 0)
		{
			base.width = this.mMaxLineWidth;
			this.overflowMethod = ((this.mMaxLineCount <= 0) ? UILabel.Overflow.ShrinkContent : UILabel.Overflow.ResizeHeight);
		}
		else
		{
			this.overflowMethod = UILabel.Overflow.ResizeFreely;
		}
		if (this.mMaxLineHeight != 0)
		{
			base.height = this.mMaxLineHeight;
		}
		if (this.mFont != null)
		{
			int defaultSize = this.mFont.defaultSize;
			if (base.height < defaultSize)
			{
				base.height = defaultSize;
			}
			this.fontSize = defaultSize;
		}
		this.mMaxLineWidth = 0;
		this.mMaxLineHeight = 0;
		this.mShrinkToFit = false;
		NGUITools.UpdateWidgetCollider(base.gameObject, true);
	}

	// Token: 0x0600361D RID: 13853 RVA: 0x00112AEC File Offset: 0x00110EEC
	protected override void OnAnchor()
	{
		if (this.mOverflow == UILabel.Overflow.ResizeFreely)
		{
			if (base.isFullyAnchored)
			{
				this.mOverflow = UILabel.Overflow.ShrinkContent;
			}
		}
		else if (this.mOverflow == UILabel.Overflow.ResizeHeight && this.topAnchor.target != null && this.bottomAnchor.target != null)
		{
			this.mOverflow = UILabel.Overflow.ShrinkContent;
		}
		base.OnAnchor();
	}

	// Token: 0x0600361E RID: 13854 RVA: 0x00112B61 File Offset: 0x00110F61
	private void ProcessAndRequest()
	{
		if (this.ambigiousFont != null)
		{
			this.ProcessText();
		}
	}

	// Token: 0x0600361F RID: 13855 RVA: 0x00112B7C File Offset: 0x00110F7C
	protected override void OnStart()
	{
		base.OnStart();
		if (this.mLineWidth > 0f)
		{
			this.mMaxLineWidth = Mathf.RoundToInt(this.mLineWidth);
			this.mLineWidth = 0f;
		}
		if (!this.mMultiline)
		{
			this.mMaxLineCount = 1;
			this.mMultiline = true;
		}
		this.mPremultiply = (this.material != null && this.material.shader != null && this.material.shader.name.Contains("Premultiplied"));
		this.ProcessAndRequest();
	}

	// Token: 0x06003620 RID: 13856 RVA: 0x00112C24 File Offset: 0x00111024
	public override void MarkAsChanged()
	{
		this.shouldBeProcessed = true;
		base.MarkAsChanged();
	}

	// Token: 0x06003621 RID: 13857 RVA: 0x00112C33 File Offset: 0x00111033
	public void ProcessText()
	{
		this.ProcessText(false, true);
	}

	// Token: 0x06003622 RID: 13858 RVA: 0x00112C40 File Offset: 0x00111040
	private void ProcessText(bool legacyMode, bool full)
	{
		if (!this.isValid)
		{
			return;
		}
		this.mChanged = true;
		this.shouldBeProcessed = false;
		float num = this.mDrawRegion.z - this.mDrawRegion.x;
		float num2 = this.mDrawRegion.w - this.mDrawRegion.y;
		NGUIText.rectWidth = ((!legacyMode) ? base.width : ((this.mMaxLineWidth == 0) ? 1000000 : this.mMaxLineWidth));
		NGUIText.rectHeight = ((!legacyMode) ? base.height : ((this.mMaxLineHeight == 0) ? 1000000 : this.mMaxLineHeight));
		NGUIText.regionWidth = ((num == 1f) ? NGUIText.rectWidth : Mathf.RoundToInt((float)NGUIText.rectWidth * num));
		NGUIText.regionHeight = ((num2 == 1f) ? NGUIText.rectHeight : Mathf.RoundToInt((float)NGUIText.rectHeight * num2));
		this.mPrintedSize = Mathf.Abs((!legacyMode) ? this.defaultFontSize : Mathf.RoundToInt(base.cachedTransform.localScale.x));
		this.mScale = 1f;
		if (NGUIText.regionWidth < 1 || NGUIText.regionHeight < 0)
		{
			this.mProcessedText = string.Empty;
			return;
		}
		bool flag = this.trueTypeFont != null;
		if (flag && this.keepCrisp)
		{
			UIRoot root = base.root;
			if (root != null)
			{
				this.mDensity = ((!(root != null)) ? 1f : root.pixelSizeAdjustment);
			}
		}
		else
		{
			this.mDensity = 1f;
		}
		if (full)
		{
			this.UpdateNGUIText();
		}
		if (this.mOverflow == UILabel.Overflow.ResizeFreely)
		{
			NGUIText.rectWidth = 1000000;
			NGUIText.regionWidth = 1000000;
		}
		if (this.mOverflow == UILabel.Overflow.ResizeFreely || this.mOverflow == UILabel.Overflow.ResizeHeight)
		{
			NGUIText.rectHeight = 1000000;
			NGUIText.regionHeight = 1000000;
		}
		if (this.mPrintedSize > 0)
		{
			bool keepCrisp = this.keepCrisp;
			for (int i = this.mPrintedSize; i > 0; i--)
			{
				if (keepCrisp)
				{
					this.mPrintedSize = i;
					NGUIText.fontSize = this.mPrintedSize;
				}
				else
				{
					this.mScale = (float)i / (float)this.mPrintedSize;
					NGUIText.fontScale = ((!flag) ? ((float)this.mFontSize / (float)this.mFont.defaultSize * this.mScale) : this.mScale);
				}
				NGUIText.Update(false);
				bool flag2 = NGUIText.WrapText(this.mText, out this.mProcessedText, true);
				if (this.mOverflow != UILabel.Overflow.ShrinkContent || flag2)
				{
					if (this.mOverflow == UILabel.Overflow.ResizeFreely)
					{
						this.mCalculatedSize = NGUIText.CalculatePrintedSize(this.mProcessedText);
						this.mWidth = Mathf.Max(this.minWidth, Mathf.RoundToInt(this.mCalculatedSize.x));
						if (num != 1f)
						{
							this.mWidth = Mathf.RoundToInt((float)this.mWidth / num);
						}
						this.mHeight = Mathf.Max(this.minHeight, Mathf.RoundToInt(this.mCalculatedSize.y));
						if (num2 != 1f)
						{
							this.mHeight = Mathf.RoundToInt((float)this.mHeight / num2);
						}
						if ((this.mWidth & 1) == 1)
						{
							this.mWidth++;
						}
						if ((this.mHeight & 1) == 1)
						{
							this.mHeight++;
						}
					}
					else if (this.mOverflow == UILabel.Overflow.ResizeHeight)
					{
						this.mCalculatedSize = NGUIText.CalculatePrintedSize(this.mProcessedText);
						this.mHeight = Mathf.Max(this.minHeight, Mathf.RoundToInt(this.mCalculatedSize.y));
						if (num2 != 1f)
						{
							this.mHeight = Mathf.RoundToInt((float)this.mHeight / num2);
						}
						if ((this.mHeight & 1) == 1)
						{
							this.mHeight++;
						}
					}
					else
					{
						this.mCalculatedSize = NGUIText.CalculatePrintedSize(this.mProcessedText);
					}
					if (legacyMode)
					{
						base.width = Mathf.RoundToInt(this.mCalculatedSize.x);
						base.height = Mathf.RoundToInt(this.mCalculatedSize.y);
						base.cachedTransform.localScale = Vector3.one;
					}
					break;
				}
				if (--i <= 1)
				{
					break;
				}
			}
		}
		else
		{
			base.cachedTransform.localScale = Vector3.one;
			this.mProcessedText = string.Empty;
			this.mScale = 1f;
		}
		if (full)
		{
			NGUIText.bitmapFont = null;
			NGUIText.dynamicFont = null;
		}
	}

	// Token: 0x06003623 RID: 13859 RVA: 0x00113128 File Offset: 0x00111528
	public override void MakePixelPerfect()
	{
		if (this.ambigiousFont != null)
		{
			Vector3 localPosition = base.cachedTransform.localPosition;
			localPosition.x = (float)Mathf.RoundToInt(localPosition.x);
			localPosition.y = (float)Mathf.RoundToInt(localPosition.y);
			localPosition.z = (float)Mathf.RoundToInt(localPosition.z);
			base.cachedTransform.localPosition = localPosition;
			base.cachedTransform.localScale = Vector3.one;
			if (this.mOverflow == UILabel.Overflow.ResizeFreely)
			{
				this.AssumeNaturalSize();
			}
			else
			{
				int width = base.width;
				int height = base.height;
				UILabel.Overflow overflow = this.mOverflow;
				if (overflow != UILabel.Overflow.ResizeHeight)
				{
					this.mWidth = 100000;
				}
				this.mHeight = 100000;
				this.mOverflow = UILabel.Overflow.ShrinkContent;
				this.ProcessText(false, true);
				this.mOverflow = overflow;
				int num = Mathf.RoundToInt(this.mCalculatedSize.x);
				int num2 = Mathf.RoundToInt(this.mCalculatedSize.y);
				num = Mathf.Max(num, base.minWidth);
				num2 = Mathf.Max(num2, base.minHeight);
				this.mWidth = Mathf.Max(width, num);
				this.mHeight = Mathf.Max(height, num2);
				this.MarkAsChanged();
			}
		}
		else
		{
			base.MakePixelPerfect();
		}
	}

	// Token: 0x06003624 RID: 13860 RVA: 0x00113278 File Offset: 0x00111678
	public void AssumeNaturalSize()
	{
		if (this.ambigiousFont != null)
		{
			this.mWidth = 100000;
			this.mHeight = 100000;
			this.ProcessText(false, true);
			this.mWidth = Mathf.RoundToInt(this.mCalculatedSize.x);
			this.mHeight = Mathf.RoundToInt(this.mCalculatedSize.y);
			if ((this.mWidth & 1) == 1)
			{
				this.mWidth++;
			}
			if ((this.mHeight & 1) == 1)
			{
				this.mHeight++;
			}
			this.MarkAsChanged();
		}
	}

	// Token: 0x06003625 RID: 13861 RVA: 0x0011331E File Offset: 0x0011171E
	[Obsolete("Use UILabel.GetCharacterAtPosition instead")]
	public int GetCharacterIndex(Vector3 worldPos)
	{
		return this.GetCharacterIndexAtPosition(worldPos, false);
	}

	// Token: 0x06003626 RID: 13862 RVA: 0x00113328 File Offset: 0x00111728
	[Obsolete("Use UILabel.GetCharacterAtPosition instead")]
	public int GetCharacterIndex(Vector2 localPos)
	{
		return this.GetCharacterIndexAtPosition(localPos, false);
	}

	// Token: 0x06003627 RID: 13863 RVA: 0x00113334 File Offset: 0x00111734
	public int GetCharacterIndexAtPosition(Vector3 worldPos, bool precise)
	{
		Vector2 localPos = base.cachedTransform.InverseTransformPoint(worldPos);
		return this.GetCharacterIndexAtPosition(localPos, precise);
	}

	// Token: 0x06003628 RID: 13864 RVA: 0x0011335C File Offset: 0x0011175C
	public int GetCharacterIndexAtPosition(Vector2 localPos, bool precise)
	{
		if (this.isValid)
		{
			string processedText = this.processedText;
			if (string.IsNullOrEmpty(processedText))
			{
				return 0;
			}
			this.UpdateNGUIText();
			if (precise)
			{
				NGUIText.PrintExactCharacterPositions(processedText, UILabel.mTempVerts, UILabel.mTempIndices);
			}
			else
			{
				NGUIText.PrintApproximateCharacterPositions(processedText, UILabel.mTempVerts, UILabel.mTempIndices);
			}
			if (UILabel.mTempVerts.size > 0)
			{
				this.ApplyOffset(UILabel.mTempVerts, 0);
				int result = (!precise) ? NGUIText.GetApproximateCharacterIndex(UILabel.mTempVerts, UILabel.mTempIndices, localPos) : NGUIText.GetExactCharacterIndex(UILabel.mTempVerts, UILabel.mTempIndices, localPos);
				UILabel.mTempVerts.Clear();
				UILabel.mTempIndices.Clear();
				NGUIText.bitmapFont = null;
				NGUIText.dynamicFont = null;
				return result;
			}
			NGUIText.bitmapFont = null;
			NGUIText.dynamicFont = null;
		}
		return 0;
	}

	// Token: 0x06003629 RID: 13865 RVA: 0x00113434 File Offset: 0x00111834
	public string GetWordAtPosition(Vector3 worldPos)
	{
		int characterIndexAtPosition = this.GetCharacterIndexAtPosition(worldPos, true);
		return this.GetWordAtCharacterIndex(characterIndexAtPosition);
	}

	// Token: 0x0600362A RID: 13866 RVA: 0x00113454 File Offset: 0x00111854
	public string GetWordAtPosition(Vector2 localPos)
	{
		int characterIndexAtPosition = this.GetCharacterIndexAtPosition(localPos, true);
		return this.GetWordAtCharacterIndex(characterIndexAtPosition);
	}

	// Token: 0x0600362B RID: 13867 RVA: 0x00113474 File Offset: 0x00111874
	public string GetWordAtCharacterIndex(int characterIndex)
	{
		if (characterIndex != -1 && characterIndex < this.mText.Length)
		{
			int num = this.mText.LastIndexOfAny(new char[]
			{
				' ',
				'\n'
			}, characterIndex) + 1;
			int num2 = this.mText.IndexOfAny(new char[]
			{
				' ',
				'\n',
				',',
				'.'
			}, characterIndex);
			if (num2 == -1)
			{
				num2 = this.mText.Length;
			}
			if (num != num2)
			{
				int num3 = num2 - num;
				if (num3 > 0)
				{
					string text = this.mText.Substring(num, num3);
					return NGUIText.StripSymbols(text);
				}
			}
		}
		return null;
	}

	// Token: 0x0600362C RID: 13868 RVA: 0x00113511 File Offset: 0x00111911
	public string GetUrlAtPosition(Vector3 worldPos)
	{
		return this.GetUrlAtCharacterIndex(this.GetCharacterIndexAtPosition(worldPos, true));
	}

	// Token: 0x0600362D RID: 13869 RVA: 0x00113521 File Offset: 0x00111921
	public string GetUrlAtPosition(Vector2 localPos)
	{
		return this.GetUrlAtCharacterIndex(this.GetCharacterIndexAtPosition(localPos, true));
	}

	// Token: 0x0600362E RID: 13870 RVA: 0x00113534 File Offset: 0x00111934
	public string GetUrlAtCharacterIndex(int characterIndex)
	{
		if (characterIndex != -1 && characterIndex < this.mText.Length - 6)
		{
			int num;
			if (this.mText[characterIndex] == '[' && this.mText[characterIndex + 1] == 'u' && this.mText[characterIndex + 2] == 'r' && this.mText[characterIndex + 3] == 'l' && this.mText[characterIndex + 4] == '=')
			{
				num = characterIndex;
			}
			else
			{
				num = this.mText.LastIndexOf("[url=", characterIndex);
			}
			if (num == -1)
			{
				return null;
			}
			num += 5;
			int num2 = this.mText.IndexOf("]", num);
			if (num2 == -1)
			{
				return null;
			}
			int num3 = this.mText.IndexOf("[/url]", num2);
			if (num3 == -1 || characterIndex <= num3)
			{
				return this.mText.Substring(num, num2 - num);
			}
		}
		return null;
	}

	// Token: 0x0600362F RID: 13871 RVA: 0x00113634 File Offset: 0x00111A34
	public int GetCharacterIndex(int currentIndex, KeyCode key)
	{
		if (this.isValid)
		{
			string processedText = this.processedText;
			if (string.IsNullOrEmpty(processedText))
			{
				return 0;
			}
			int defaultFontSize = this.defaultFontSize;
			this.UpdateNGUIText();
			NGUIText.PrintApproximateCharacterPositions(processedText, UILabel.mTempVerts, UILabel.mTempIndices);
			if (UILabel.mTempVerts.size > 0)
			{
				this.ApplyOffset(UILabel.mTempVerts, 0);
				int i = 0;
				while (i < UILabel.mTempIndices.size)
				{
					if (UILabel.mTempIndices[i] == currentIndex)
					{
						Vector2 pos = UILabel.mTempVerts[i];
						if (key == KeyCode.UpArrow)
						{
							pos.y += (float)defaultFontSize + this.effectiveSpacingY;
						}
						else if (key == KeyCode.DownArrow)
						{
							pos.y -= (float)defaultFontSize + this.effectiveSpacingY;
						}
						else if (key == KeyCode.Home)
						{
							pos.x -= 1000f;
						}
						else if (key == KeyCode.End)
						{
							pos.x += 1000f;
						}
						int approximateCharacterIndex = NGUIText.GetApproximateCharacterIndex(UILabel.mTempVerts, UILabel.mTempIndices, pos);
						if (approximateCharacterIndex == currentIndex)
						{
							break;
						}
						UILabel.mTempVerts.Clear();
						UILabel.mTempIndices.Clear();
						return approximateCharacterIndex;
					}
					else
					{
						i++;
					}
				}
				UILabel.mTempVerts.Clear();
				UILabel.mTempIndices.Clear();
			}
			NGUIText.bitmapFont = null;
			NGUIText.dynamicFont = null;
			if (key == KeyCode.UpArrow || key == KeyCode.Home)
			{
				return 0;
			}
			if (key == KeyCode.DownArrow || key == KeyCode.End)
			{
				return processedText.Length;
			}
		}
		return currentIndex;
	}

	// Token: 0x06003630 RID: 13872 RVA: 0x001137F4 File Offset: 0x00111BF4
	public void PrintOverlay(int start, int end, UIGeometry caret, UIGeometry highlight, Color caretColor, Color highlightColor)
	{
		if (caret != null)
		{
			caret.Clear();
		}
		if (highlight != null)
		{
			highlight.Clear();
		}
		if (!this.isValid)
		{
			return;
		}
		string processedText = this.processedText;
		this.UpdateNGUIText();
		int size = caret.verts.size;
		Vector2 item = new Vector2(0.5f, 0.5f);
		float finalAlpha = this.finalAlpha;
		if (highlight != null && start != end)
		{
			int size2 = highlight.verts.size;
			NGUIText.PrintCaretAndSelection(processedText, start, end, caret.verts, highlight.verts);
			if (highlight.verts.size > size2)
			{
				this.ApplyOffset(highlight.verts, size2);
				Color32 item2 = new Color(highlightColor.r, highlightColor.g, highlightColor.b, highlightColor.a * finalAlpha);
				for (int i = size2; i < highlight.verts.size; i++)
				{
					highlight.uvs.Add(item);
					highlight.cols.Add(item2);
				}
			}
		}
		else
		{
			NGUIText.PrintCaretAndSelection(processedText, start, end, caret.verts, null);
		}
		this.ApplyOffset(caret.verts, size);
		Color32 item3 = new Color(caretColor.r, caretColor.g, caretColor.b, caretColor.a * finalAlpha);
		for (int j = size; j < caret.verts.size; j++)
		{
			caret.uvs.Add(item);
			caret.cols.Add(item3);
		}
		NGUIText.bitmapFont = null;
		NGUIText.dynamicFont = null;
	}

	// Token: 0x06003631 RID: 13873 RVA: 0x001139A4 File Offset: 0x00111DA4
	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		if (!this.isValid)
		{
			return;
		}
		int num = verts.size;
		Color color = base.color;
		color.a = this.finalAlpha;
		if (this.mFont != null && this.mFont.premultipliedAlphaShader)
		{
			color = NGUITools.ApplyPMA(color);
		}
		if (QualitySettings.activeColorSpace == ColorSpace.Linear)
		{
			color.r = Mathf.Pow(color.r, 2.2f);
			color.g = Mathf.Pow(color.g, 2.2f);
			color.b = Mathf.Pow(color.b, 2.2f);
		}
		string processedText = this.processedText;
		int size = verts.size;
		this.UpdateNGUIText();
		NGUIText.tint = color;
		NGUIText.Print(processedText, verts, uvs, cols);
		NGUIText.bitmapFont = null;
		NGUIText.dynamicFont = null;
		Vector2 vector = this.ApplyOffset(verts, size);
		if (this.mFont != null && this.mFont.packedFontShader)
		{
			return;
		}
		if (this.effectStyle != UILabel.Effect.None)
		{
			int size2 = verts.size;
			vector.x = this.mEffectDistance.x;
			vector.y = this.mEffectDistance.y;
			this.ApplyShadow(verts, uvs, cols, num, size2, vector.x, -vector.y);
			if (this.effectStyle == UILabel.Effect.Outline)
			{
				num = size2;
				size2 = verts.size;
				this.ApplyShadow(verts, uvs, cols, num, size2, -vector.x, vector.y);
				num = size2;
				size2 = verts.size;
				this.ApplyShadow(verts, uvs, cols, num, size2, vector.x, vector.y);
				num = size2;
				size2 = verts.size;
				this.ApplyShadow(verts, uvs, cols, num, size2, -vector.x, -vector.y);
			}
		}
		if (this.onPostFill != null)
		{
			this.onPostFill(this, num, verts, uvs, cols);
		}
	}

	// Token: 0x06003632 RID: 13874 RVA: 0x00113B9C File Offset: 0x00111F9C
	public Vector2 ApplyOffset(BetterList<Vector3> verts, int start)
	{
		Vector2 pivotOffset = base.pivotOffset;
		float num = Mathf.Lerp(0f, (float)(-(float)this.mWidth), pivotOffset.x);
		float num2 = Mathf.Lerp((float)this.mHeight, 0f, pivotOffset.y) + Mathf.Lerp(this.mCalculatedSize.y - (float)this.mHeight, 0f, pivotOffset.y);
		num = Mathf.Round(num);
		num2 = Mathf.Round(num2);
		for (int i = start; i < verts.size; i++)
		{
			Vector3[] buffer = verts.buffer;
			int num3 = i;
			buffer[num3].x = buffer[num3].x + num;
			Vector3[] buffer2 = verts.buffer;
			int num4 = i;
			buffer2[num4].y = buffer2[num4].y + num2;
		}
		return new Vector2(num, num2);
	}

	// Token: 0x06003633 RID: 13875 RVA: 0x00113C68 File Offset: 0x00112068
	public void ApplyShadow(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols, int start, int end, float x, float y)
	{
		Color color = this.mEffectColor;
		color.a *= this.finalAlpha;
		Color32 color2 = (!(this.bitmapFont != null) || !this.bitmapFont.premultipliedAlphaShader) ? color : NGUITools.ApplyPMA(color);
		for (int i = start; i < end; i++)
		{
			verts.Add(verts.buffer[i]);
			uvs.Add(uvs.buffer[i]);
			cols.Add(cols.buffer[i]);
			Vector3 vector = verts.buffer[i];
			vector.x += x;
			vector.y += y;
			verts.buffer[i] = vector;
			Color32 color3 = cols.buffer[i];
			if (color3.a == 255)
			{
				cols.buffer[i] = color2;
			}
			else
			{
				Color color4 = color;
				color4.a = (float)color3.a / 255f * color.a;
				cols.buffer[i] = ((!(this.bitmapFont != null) || !this.bitmapFont.premultipliedAlphaShader) ? color4 : NGUITools.ApplyPMA(color4));
			}
		}
	}

	// Token: 0x06003634 RID: 13876 RVA: 0x00113E00 File Offset: 0x00112200
	public int CalculateOffsetToFit(string text)
	{
		this.UpdateNGUIText();
		NGUIText.encoding = false;
		NGUIText.symbolStyle = NGUIText.SymbolStyle.None;
		int result = NGUIText.CalculateOffsetToFit(text);
		NGUIText.bitmapFont = null;
		NGUIText.dynamicFont = null;
		return result;
	}

	// Token: 0x06003635 RID: 13877 RVA: 0x00113E34 File Offset: 0x00112234
	public void SetCurrentProgress()
	{
		if (UIProgressBar.current != null)
		{
			this.text = UIProgressBar.current.value.ToString("F");
		}
	}

	// Token: 0x06003636 RID: 13878 RVA: 0x00113E6E File Offset: 0x0011226E
	public void SetCurrentPercent()
	{
		if (UIProgressBar.current != null)
		{
			this.text = Mathf.RoundToInt(UIProgressBar.current.value * 100f) + "%";
		}
	}

	// Token: 0x06003637 RID: 13879 RVA: 0x00113EAC File Offset: 0x001122AC
	public void SetCurrentSelection()
	{
		if (UIPopupList.current != null)
		{
			this.text = ((!UIPopupList.current.isLocalized) ? UIPopupList.current.value : Localization.Get(UIPopupList.current.value));
		}
	}

	// Token: 0x06003638 RID: 13880 RVA: 0x00113EFC File Offset: 0x001122FC
	public bool Wrap(string text, out string final)
	{
		return this.Wrap(text, out final, 1000000);
	}

	// Token: 0x06003639 RID: 13881 RVA: 0x00113F0C File Offset: 0x0011230C
	public bool Wrap(string text, out string final, int height)
	{
		this.UpdateNGUIText();
		NGUIText.rectHeight = height;
		NGUIText.regionHeight = height;
		bool result = NGUIText.WrapText(text, out final);
		NGUIText.bitmapFont = null;
		NGUIText.dynamicFont = null;
		return result;
	}

	// Token: 0x0600363A RID: 13882 RVA: 0x00113F40 File Offset: 0x00112340
	public void UpdateNGUIText()
	{
		Font trueTypeFont = this.trueTypeFont;
		bool flag = trueTypeFont != null;
		NGUIText.fontSize = this.mPrintedSize;
		NGUIText.fontStyle = this.mFontStyle;
		NGUIText.rectWidth = this.mWidth;
		NGUIText.rectHeight = this.mHeight;
		NGUIText.regionWidth = Mathf.RoundToInt((float)this.mWidth * (this.mDrawRegion.z - this.mDrawRegion.x));
		NGUIText.regionHeight = Mathf.RoundToInt((float)this.mHeight * (this.mDrawRegion.w - this.mDrawRegion.y));
		NGUIText.gradient = (this.mApplyGradient && (this.mFont == null || !this.mFont.packedFontShader));
		NGUIText.gradientTop = this.mGradientTop;
		NGUIText.gradientBottom = this.mGradientBottom;
		NGUIText.encoding = this.mEncoding;
		NGUIText.premultiply = this.mPremultiply;
		NGUIText.symbolStyle = this.mSymbols;
		NGUIText.maxLines = this.mMaxLineCount;
		NGUIText.spacingX = this.effectiveSpacingX;
		NGUIText.spacingY = this.effectiveSpacingY;
		NGUIText.fontScale = ((!flag) ? ((float)this.mFontSize / (float)this.mFont.defaultSize * this.mScale) : this.mScale);
		if (this.mFont != null)
		{
			NGUIText.bitmapFont = this.mFont;
			for (;;)
			{
				UIFont replacement = NGUIText.bitmapFont.replacement;
				if (replacement == null)
				{
					break;
				}
				NGUIText.bitmapFont = replacement;
			}
			if (NGUIText.bitmapFont.isDynamic)
			{
				NGUIText.dynamicFont = NGUIText.bitmapFont.dynamicFont;
				NGUIText.bitmapFont = null;
			}
			else
			{
				NGUIText.dynamicFont = null;
			}
		}
		else
		{
			NGUIText.dynamicFont = trueTypeFont;
			NGUIText.bitmapFont = null;
		}
		if (flag && this.keepCrisp)
		{
			UIRoot root = base.root;
			if (root != null)
			{
				NGUIText.pixelDensity = ((!(root != null)) ? 1f : root.pixelSizeAdjustment);
			}
		}
		else
		{
			NGUIText.pixelDensity = 1f;
		}
		if (this.mDensity != NGUIText.pixelDensity)
		{
			this.ProcessText(false, false);
			NGUIText.rectWidth = this.mWidth;
			NGUIText.rectHeight = this.mHeight;
			NGUIText.regionWidth = Mathf.RoundToInt((float)this.mWidth * (this.mDrawRegion.z - this.mDrawRegion.x));
			NGUIText.regionHeight = Mathf.RoundToInt((float)this.mHeight * (this.mDrawRegion.w - this.mDrawRegion.y));
		}
		if (this.alignment == NGUIText.Alignment.Automatic)
		{
			UIWidget.Pivot pivot = base.pivot;
			if (pivot == UIWidget.Pivot.Left || pivot == UIWidget.Pivot.TopLeft || pivot == UIWidget.Pivot.BottomLeft)
			{
				NGUIText.alignment = NGUIText.Alignment.Left;
			}
			else if (pivot == UIWidget.Pivot.Right || pivot == UIWidget.Pivot.TopRight || pivot == UIWidget.Pivot.BottomRight)
			{
				NGUIText.alignment = NGUIText.Alignment.Right;
			}
			else
			{
				NGUIText.alignment = NGUIText.Alignment.Center;
			}
		}
		else
		{
			NGUIText.alignment = this.alignment;
		}
		NGUIText.Update();
	}

	// Token: 0x04001F16 RID: 7958
	public UILabel.Crispness keepCrispWhenShrunk = UILabel.Crispness.OnDesktop;

	// Token: 0x04001F17 RID: 7959
	[HideInInspector]
	[SerializeField]
	private Font mTrueTypeFont;

	// Token: 0x04001F18 RID: 7960
	[HideInInspector]
	[SerializeField]
	private UIFont mFont;

	// Token: 0x04001F19 RID: 7961
	[Multiline(6)]
	[HideInInspector]
	[SerializeField]
	private string mText = string.Empty;

	// Token: 0x04001F1A RID: 7962
	[HideInInspector]
	[SerializeField]
	private int mFontSize = 16;

	// Token: 0x04001F1B RID: 7963
	[HideInInspector]
	[SerializeField]
	private FontStyle mFontStyle;

	// Token: 0x04001F1C RID: 7964
	[HideInInspector]
	[SerializeField]
	private NGUIText.Alignment mAlignment;

	// Token: 0x04001F1D RID: 7965
	[HideInInspector]
	[SerializeField]
	private bool mEncoding = true;

	// Token: 0x04001F1E RID: 7966
	[HideInInspector]
	[SerializeField]
	private int mMaxLineCount;

	// Token: 0x04001F1F RID: 7967
	[HideInInspector]
	[SerializeField]
	private UILabel.Effect mEffectStyle;

	// Token: 0x04001F20 RID: 7968
	[HideInInspector]
	[SerializeField]
	private Color mEffectColor = Color.black;

	// Token: 0x04001F21 RID: 7969
	[HideInInspector]
	[SerializeField]
	private NGUIText.SymbolStyle mSymbols = NGUIText.SymbolStyle.Normal;

	// Token: 0x04001F22 RID: 7970
	[HideInInspector]
	[SerializeField]
	private Vector2 mEffectDistance = Vector2.one;

	// Token: 0x04001F23 RID: 7971
	[HideInInspector]
	[SerializeField]
	private UILabel.Overflow mOverflow;

	// Token: 0x04001F24 RID: 7972
	[HideInInspector]
	[SerializeField]
	private Material mMaterial;

	// Token: 0x04001F25 RID: 7973
	[HideInInspector]
	[SerializeField]
	private bool mApplyGradient;

	// Token: 0x04001F26 RID: 7974
	[HideInInspector]
	[SerializeField]
	private Color mGradientTop = Color.white;

	// Token: 0x04001F27 RID: 7975
	[HideInInspector]
	[SerializeField]
	private Color mGradientBottom = new Color(0.7f, 0.7f, 0.7f);

	// Token: 0x04001F28 RID: 7976
	[HideInInspector]
	[SerializeField]
	private int mSpacingX;

	// Token: 0x04001F29 RID: 7977
	[HideInInspector]
	[SerializeField]
	private int mSpacingY;

	// Token: 0x04001F2A RID: 7978
	[HideInInspector]
	[SerializeField]
	private bool mUseFloatSpacing;

	// Token: 0x04001F2B RID: 7979
	[HideInInspector]
	[SerializeField]
	private float mFloatSpacingX;

	// Token: 0x04001F2C RID: 7980
	[HideInInspector]
	[SerializeField]
	private float mFloatSpacingY;

	// Token: 0x04001F2D RID: 7981
	[HideInInspector]
	[SerializeField]
	private bool mShrinkToFit;

	// Token: 0x04001F2E RID: 7982
	[HideInInspector]
	[SerializeField]
	private int mMaxLineWidth;

	// Token: 0x04001F2F RID: 7983
	[HideInInspector]
	[SerializeField]
	private int mMaxLineHeight;

	// Token: 0x04001F30 RID: 7984
	[HideInInspector]
	[SerializeField]
	private float mLineWidth;

	// Token: 0x04001F31 RID: 7985
	[HideInInspector]
	[SerializeField]
	private bool mMultiline = true;

	// Token: 0x04001F32 RID: 7986
	[NonSerialized]
	private Font mActiveTTF;

	// Token: 0x04001F33 RID: 7987
	private float mDensity = 1f;

	// Token: 0x04001F34 RID: 7988
	private bool mShouldBeProcessed = true;

	// Token: 0x04001F35 RID: 7989
	private string mProcessedText;

	// Token: 0x04001F36 RID: 7990
	private bool mPremultiply;

	// Token: 0x04001F37 RID: 7991
	private Vector2 mCalculatedSize = Vector2.zero;

	// Token: 0x04001F38 RID: 7992
	private float mScale = 1f;

	// Token: 0x04001F39 RID: 7993
	private int mPrintedSize;

	// Token: 0x04001F3A RID: 7994
	private int mLastWidth;

	// Token: 0x04001F3B RID: 7995
	private int mLastHeight;

	// Token: 0x04001F3C RID: 7996
	private static BetterList<UILabel> mList = new BetterList<UILabel>();

	// Token: 0x04001F3D RID: 7997
	private static Dictionary<Font, int> mFontUsage = new Dictionary<Font, int>();

	// Token: 0x04001F3E RID: 7998
	private static BetterList<Vector3> mTempVerts = new BetterList<Vector3>();

	// Token: 0x04001F3F RID: 7999
	private static BetterList<int> mTempIndices = new BetterList<int>();

	// Token: 0x04001F40 RID: 8000
	[CompilerGenerated]
	private static Font.FontTextureRebuildCallback f__mg0;

	// Token: 0x0200064A RID: 1610
	public enum Effect
	{
		// Token: 0x04001F42 RID: 8002
		None,
		// Token: 0x04001F43 RID: 8003
		Shadow,
		// Token: 0x04001F44 RID: 8004
		Outline
	}

	// Token: 0x0200064B RID: 1611
	public enum Overflow
	{
		// Token: 0x04001F46 RID: 8006
		ShrinkContent,
		// Token: 0x04001F47 RID: 8007
		ClampContent,
		// Token: 0x04001F48 RID: 8008
		ResizeFreely,
		// Token: 0x04001F49 RID: 8009
		ResizeHeight
	}

	// Token: 0x0200064C RID: 1612
	public enum Crispness
	{
		// Token: 0x04001F4B RID: 8011
		Never,
		// Token: 0x04001F4C RID: 8012
		OnDesktop,
		// Token: 0x04001F4D RID: 8013
		Always
	}
}
