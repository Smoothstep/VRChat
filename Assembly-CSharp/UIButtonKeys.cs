using System;
using UnityEngine;

// Token: 0x0200059F RID: 1439
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Button Keys (Legacy)")]
public class UIButtonKeys : UIKeyNavigation
{
	// Token: 0x06003024 RID: 12324 RVA: 0x000EC2AE File Offset: 0x000EA6AE
	protected override void OnEnable()
	{
		this.Upgrade();
		base.OnEnable();
	}

	// Token: 0x06003025 RID: 12325 RVA: 0x000EC2BC File Offset: 0x000EA6BC
	public void Upgrade()
	{
		if (this.onClick == null && this.selectOnClick != null)
		{
			this.onClick = this.selectOnClick.gameObject;
			this.selectOnClick = null;
			NGUITools.SetDirty(this);
		}
		if (this.onLeft == null && this.selectOnLeft != null)
		{
			this.onLeft = this.selectOnLeft.gameObject;
			this.selectOnLeft = null;
			NGUITools.SetDirty(this);
		}
		if (this.onRight == null && this.selectOnRight != null)
		{
			this.onRight = this.selectOnRight.gameObject;
			this.selectOnRight = null;
			NGUITools.SetDirty(this);
		}
		if (this.onUp == null && this.selectOnUp != null)
		{
			this.onUp = this.selectOnUp.gameObject;
			this.selectOnUp = null;
			NGUITools.SetDirty(this);
		}
		if (this.onDown == null && this.selectOnDown != null)
		{
			this.onDown = this.selectOnDown.gameObject;
			this.selectOnDown = null;
			NGUITools.SetDirty(this);
		}
	}

	// Token: 0x04001A93 RID: 6803
	public UIButtonKeys selectOnClick;

	// Token: 0x04001A94 RID: 6804
	public UIButtonKeys selectOnUp;

	// Token: 0x04001A95 RID: 6805
	public UIButtonKeys selectOnDown;

	// Token: 0x04001A96 RID: 6806
	public UIButtonKeys selectOnLeft;

	// Token: 0x04001A97 RID: 6807
	public UIButtonKeys selectOnRight;
}
