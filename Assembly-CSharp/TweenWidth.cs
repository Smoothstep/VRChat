using System;
using UnityEngine;

// Token: 0x02000625 RID: 1573
[RequireComponent(typeof(UIWidget))]
[AddComponentMenu("NGUI/Tween/Tween Width")]
public class TweenWidth : UITweener
{
	// Token: 0x170007FC RID: 2044
	// (get) Token: 0x060034AE RID: 13486 RVA: 0x0010A1F1 File Offset: 0x001085F1
	public UIWidget cachedWidget
	{
		get
		{
			if (this.mWidget == null)
			{
				this.mWidget = base.GetComponent<UIWidget>();
			}
			return this.mWidget;
		}
	}

	// Token: 0x170007FD RID: 2045
	// (get) Token: 0x060034AF RID: 13487 RVA: 0x0010A216 File Offset: 0x00108616
	// (set) Token: 0x060034B0 RID: 13488 RVA: 0x0010A21E File Offset: 0x0010861E
	[Obsolete("Use 'value' instead")]
	public int width
	{
		get
		{
			return this.value;
		}
		set
		{
			this.value = value;
		}
	}

	// Token: 0x170007FE RID: 2046
	// (get) Token: 0x060034B1 RID: 13489 RVA: 0x0010A227 File Offset: 0x00108627
	// (set) Token: 0x060034B2 RID: 13490 RVA: 0x0010A234 File Offset: 0x00108634
	public int value
	{
		get
		{
			return this.cachedWidget.width;
		}
		set
		{
			this.cachedWidget.width = value;
		}
	}

	// Token: 0x060034B3 RID: 13491 RVA: 0x0010A244 File Offset: 0x00108644
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = Mathf.RoundToInt((float)this.from * (1f - factor) + (float)this.to * factor);
		if (this.updateTable)
		{
			if (this.mTable == null)
			{
				this.mTable = NGUITools.FindInParents<UITable>(base.gameObject);
				if (this.mTable == null)
				{
					this.updateTable = false;
					return;
				}
			}
			this.mTable.repositionNow = true;
		}
	}

	// Token: 0x060034B4 RID: 13492 RVA: 0x0010A2C8 File Offset: 0x001086C8
	public static TweenWidth Begin(UIWidget widget, float duration, int width)
	{
		TweenWidth tweenWidth = UITweener.Begin<TweenWidth>(widget.gameObject, duration);
		tweenWidth.from = widget.width;
		tweenWidth.to = width;
		if (duration <= 0f)
		{
			tweenWidth.Sample(1f, true);
			tweenWidth.enabled = false;
		}
		return tweenWidth;
	}

	// Token: 0x060034B5 RID: 13493 RVA: 0x0010A314 File Offset: 0x00108714
	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x060034B6 RID: 13494 RVA: 0x0010A322 File Offset: 0x00108722
	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x060034B7 RID: 13495 RVA: 0x0010A330 File Offset: 0x00108730
	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = this.from;
	}

	// Token: 0x060034B8 RID: 13496 RVA: 0x0010A33E File Offset: 0x0010873E
	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = this.to;
	}

	// Token: 0x04001DFB RID: 7675
	public int from = 100;

	// Token: 0x04001DFC RID: 7676
	public int to = 100;

	// Token: 0x04001DFD RID: 7677
	public bool updateTable;

	// Token: 0x04001DFE RID: 7678
	private UIWidget mWidget;

	// Token: 0x04001DFF RID: 7679
	private UITable mTable;
}
