using System;
using UnityEngine;

// Token: 0x02000A1A RID: 2586
public class TouchPadRelativePositional : TouchPad
{
	// Token: 0x06004E34 RID: 20020 RVA: 0x001A3BFC File Offset: 0x001A1FFC
	protected override void ForEachTouch(Touch touch, Vector2 guiTouchPos)
	{
		base.ForEachTouch(touch, guiTouchPos);
		if (this.lastFingerId != touch.fingerId)
		{
			return;
		}
		if (touch.phase == TouchPhase.Began)
		{
			this.touchStart = touch.position;
		}
		Vector2 a = new Vector2((touch.position.x - this.touchStart.x) / this.sensitivityRelativeX, (touch.position.y - this.touchStart.y) / this.sensitivityRelativeY);
		Vector2 vector = Vector2.Lerp(this.position, a * this.sensitivity * 2f, Time.deltaTime * this.interpolateTime);
		if (this.useX)
		{
			this.position.x = Mathf.Clamp(vector.x, -1f, 1f);
		}
		if (this.useY)
		{
			this.position.y = Mathf.Clamp(vector.y, -1f, 1f);
		}
		if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
		{
			base.ResetJoystick();
		}
	}
}
