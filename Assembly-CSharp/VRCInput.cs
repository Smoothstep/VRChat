using System;
using UnityEngine;

// Token: 0x02000B10 RID: 2832
public class VRCInput
{
	// Token: 0x060055DD RID: 21981 RVA: 0x001D945C File Offset: 0x001D785C
	public VRCInput(string inputName, float axisScale = 1f, float axisLow = 0f, float axisHigh = 1f)
	{
		this._name = inputName;
		this.fScale = axisScale;
		this.lowThreshold = axisLow;
		this.highThreshold = axisHigh;
	}

	// Token: 0x060055DE RID: 21982 RVA: 0x001D9481 File Offset: 0x001D7881
	public void SetAxisButtons(VRCInput pos, VRCInput neg)
	{
		this.positive = pos;
		this.negative = neg;
	}

	// Token: 0x17000C59 RID: 3161
	// (get) Token: 0x060055DF RID: 21983 RVA: 0x001D9491 File Offset: 0x001D7891
	public string name
	{
		get
		{
			return this._name;
		}
	}

	// Token: 0x17000C5A RID: 3162
	// (get) Token: 0x060055E0 RID: 21984 RVA: 0x001D9499 File Offset: 0x001D7899
	public bool down
	{
		get
		{
			return this.bValue && !this.bValuePrev;
		}
	}

	// Token: 0x17000C5B RID: 3163
	// (get) Token: 0x060055E1 RID: 21985 RVA: 0x001D94B2 File Offset: 0x001D78B2
	public bool up
	{
		get
		{
			return !this.bValue && this.bValuePrev;
		}
	}

	// Token: 0x17000C5C RID: 3164
	// (get) Token: 0x060055E2 RID: 21986 RVA: 0x001D94C8 File Offset: 0x001D78C8
	public bool button
	{
		get
		{
			return this.bValue;
		}
	}

	// Token: 0x17000C5D RID: 3165
	// (get) Token: 0x060055E3 RID: 21987 RVA: 0x001D94D0 File Offset: 0x001D78D0
	public float axis
	{
		get
		{
			float num = this.fValue;
			if (this.positive != null)
			{
				num += this.positive.axis;
			}
			if (this.negative != null)
			{
				num += this.negative.axis;
			}
			num *= this.fScale;
			if (num < 0f)
			{
				if (num > -this.lowThreshold)
				{
					return 0f;
				}
				if (num < -this.highThreshold)
				{
					return -1f;
				}
				return -Mathf.Clamp01((-num - this.lowThreshold) / (this.highThreshold - this.lowThreshold));
			}
			else
			{
				if (num < this.lowThreshold)
				{
					return 0f;
				}
				if (num > this.highThreshold)
				{
					return 1f;
				}
				return Mathf.Clamp01((num - this.lowThreshold) / (this.highThreshold - this.lowThreshold));
			}
		}
	}

	// Token: 0x17000C5E RID: 3166
	// (get) Token: 0x060055E4 RID: 21988 RVA: 0x001D95AD File Offset: 0x001D79AD
	public float held
	{
		get
		{
			if (this.timePressed == 0f)
			{
				return 0f;
			}
			return Time.unscaledTime - this.timePressed;
		}
	}

	// Token: 0x17000C5F RID: 3167
	// (get) Token: 0x060055E5 RID: 21989 RVA: 0x001D95D4 File Offset: 0x001D79D4
	public bool click
	{
		get
		{
			return this.lastDownTime < this.lastUpTime && this.lastUpTime - this.lastDownTime < 0.05f && Time.time - this.lastUpTime < 0.05f;
		}
	}

	// Token: 0x17000C60 RID: 3168
	// (get) Token: 0x060055E6 RID: 21990 RVA: 0x001D9622 File Offset: 0x001D7A22
	// (set) Token: 0x060055E7 RID: 21991 RVA: 0x001D962A File Offset: 0x001D7A2A
	private bool bValue
	{
		get
		{
			return this._bValue;
		}
		set
		{
			this._bValue = value;
			if (this._bValue)
			{
				this.lastDownTime = Time.time;
			}
			else
			{
				this.lastUpTime = Time.time;
			}
		}
	}

	// Token: 0x060055E8 RID: 21992 RVA: 0x001D9659 File Offset: 0x001D7A59
	public void Reset()
	{
		if (!this.bValue)
		{
			this.timePressed = 0f;
		}
		this.bValuePrev = this.bValue;
		this.bValue = false;
		this.fValue = 0f;
	}

	// Token: 0x060055E9 RID: 21993 RVA: 0x001D9690 File Offset: 0x001D7A90
	public void ApplyButton(bool button)
	{
		if (button)
		{
			if (this.timePressed == 0f)
			{
				this.timePressed = Time.unscaledTime;
			}
			VRCInput._changed = true;
			this.bValue = true;
			if (this.fValue < 1f)
			{
				this.fValue = 1f;
			}
		}
	}

	// Token: 0x060055EA RID: 21994 RVA: 0x001D96E8 File Offset: 0x001D7AE8
	public void ApplyAxis(float axis)
	{
		if (Mathf.Abs(axis) > 0.1f)
		{
			VRCInput._changed = true;
			this.fValue = axis;
			if (this.fValue > this.highThreshold)
			{
				this.bValue = true;
				if (this.timePressed == 0f)
				{
					this.timePressed = Time.unscaledTime;
				}
			}
		}
	}

	// Token: 0x060055EB RID: 21995 RVA: 0x001D9745 File Offset: 0x001D7B45
	public static void ClearChanges()
	{
		VRCInput._changed = false;
	}

	// Token: 0x060055EC RID: 21996 RVA: 0x001D974D File Offset: 0x001D7B4D
	public static bool IsChanged()
	{
		return VRCInput._changed;
	}

	// Token: 0x060055ED RID: 21997 RVA: 0x001D9754 File Offset: 0x001D7B54
	public static void MarkChanged()
	{
		VRCInput._changed = true;
	}

	// Token: 0x04003C94 RID: 15508
	private string _name;

	// Token: 0x04003C95 RID: 15509
	private float timePressed;

	// Token: 0x04003C96 RID: 15510
	private float fScale;

	// Token: 0x04003C97 RID: 15511
	private float lowThreshold;

	// Token: 0x04003C98 RID: 15512
	private float highThreshold;

	// Token: 0x04003C99 RID: 15513
	private float lastDownTime;

	// Token: 0x04003C9A RID: 15514
	private float lastUpTime;

	// Token: 0x04003C9B RID: 15515
	private float fValue;

	// Token: 0x04003C9C RID: 15516
	private bool _bValue;

	// Token: 0x04003C9D RID: 15517
	private bool bValuePrev;

	// Token: 0x04003C9E RID: 15518
	private VRCInput positive;

	// Token: 0x04003C9F RID: 15519
	private VRCInput negative;

	// Token: 0x04003CA0 RID: 15520
	private static bool _changed;
}
