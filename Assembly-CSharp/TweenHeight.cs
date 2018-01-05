using System;
using UnityEngine;

// Token: 0x0200061E RID: 1566
[RequireComponent(typeof(UIWidget))]
[AddComponentMenu("NGUI/Tween/Tween Height")]
public class TweenHeight : UITweener
{
	// Token: 0x170007EA RID: 2026
	// (get) Token: 0x06003464 RID: 13412 RVA: 0x001096FF File Offset: 0x00107AFF
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

	// Token: 0x170007EB RID: 2027
	// (get) Token: 0x06003465 RID: 13413 RVA: 0x00109724 File Offset: 0x00107B24
	// (set) Token: 0x06003466 RID: 13414 RVA: 0x0010972C File Offset: 0x00107B2C
	[Obsolete("Use 'value' instead")]
	public int height
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

	// Token: 0x170007EC RID: 2028
	// (get) Token: 0x06003467 RID: 13415 RVA: 0x00109735 File Offset: 0x00107B35
	// (set) Token: 0x06003468 RID: 13416 RVA: 0x00109742 File Offset: 0x00107B42
	public int value
	{
		get
		{
			return this.cachedWidget.height;
		}
		set
		{
			this.cachedWidget.height = value;
		}
	}

	// Token: 0x06003469 RID: 13417 RVA: 0x00109750 File Offset: 0x00107B50
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

	// Token: 0x0600346A RID: 13418 RVA: 0x001097D4 File Offset: 0x00107BD4
	public static TweenHeight Begin(UIWidget widget, float duration, int height)
	{
		TweenHeight tweenHeight = UITweener.Begin<TweenHeight>(widget.gameObject, duration);
		tweenHeight.from = widget.height;
		tweenHeight.to = height;
		if (duration <= 0f)
		{
			tweenHeight.Sample(1f, true);
			tweenHeight.enabled = false;
		}
		return tweenHeight;
	}

	// Token: 0x0600346B RID: 13419 RVA: 0x00109820 File Offset: 0x00107C20
	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x0600346C RID: 13420 RVA: 0x0010982E File Offset: 0x00107C2E
	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x0600346D RID: 13421 RVA: 0x0010983C File Offset: 0x00107C3C
	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = this.from;
	}

	// Token: 0x0600346E RID: 13422 RVA: 0x0010984A File Offset: 0x00107C4A
	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = this.to;
	}

	// Token: 0x04001DDC RID: 7644
	public int from = 100;

	// Token: 0x04001DDD RID: 7645
	public int to = 100;

	// Token: 0x04001DDE RID: 7646
	public bool updateTable;

	// Token: 0x04001DDF RID: 7647
	private UIWidget mWidget;

	// Token: 0x04001DE0 RID: 7648
	private UITable mTable;
}
