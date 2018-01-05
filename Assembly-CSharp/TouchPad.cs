using System;
using UnityEngine;

// Token: 0x02000A17 RID: 2583
public class TouchPad : JoystickAbstract
{
	// Token: 0x06004E2D RID: 20013 RVA: 0x001A36F4 File Offset: 0x001A1AF4
	protected override void TypeSpecificOnEnable()
	{
		if (this.gui == null)
		{
			this.moveStick = false;
		}
		else if (this.touchZone == null)
		{
			this.touchZone = this.gui;
			this.moveStick = false;
		}
		else
		{
			this.moveStick = true;
		}
		this.touchZoneRect = this.touchZone.GetScreenRect();
		Vector2 center = this.touchZoneRect.center;
		this.touchZoneRect.width = this.touchZoneRect.width * (1f - this.touchZonePadding);
		this.touchZoneRect.height = this.touchZoneRect.height * (1f - this.touchZonePadding);
		this.touchZoneRect.center = center;
		this.position = this.startPosition;
		Vector2 vector = new Vector2(this.touchZoneRect.width, this.touchZoneRect.height);
		this.swipeScale = vector.magnitude * 0.01f;
		TouchPad.SensitivityRelativeTo sensitivityRelativeTo = this.sensitivityRelativeTo;
		if (sensitivityRelativeTo != TouchPad.SensitivityRelativeTo.ZoneSize)
		{
			if (sensitivityRelativeTo == TouchPad.SensitivityRelativeTo.Resolution)
			{
				float num = (Screen.dpi <= 0f) ? 100f : Screen.dpi;
				this.sensitivityRelativeX = num;
				this.sensitivityRelativeY = num;
			}
		}
		else
		{
			this.sensitivityRelativeX = this.touchZoneRect.width;
			this.sensitivityRelativeY = this.touchZoneRect.height;
		}
	}

	// Token: 0x06004E2E RID: 20014 RVA: 0x001A3864 File Offset: 0x001A1C64
	protected override void ZeroWhenUnused()
	{
		if (this.lastFingerId != -1)
		{
			return;
		}
		if (this.autoReturnStyle == JoystickAbstract.ReturnStyleOption.Curved)
		{
			this.position.x = Mathf.Lerp(this.position.x, 0f, Time.deltaTime * this.autoReturnSpeed.x);
			this.position.y = Mathf.Lerp(this.position.y, 0f, Time.deltaTime * this.autoReturnSpeed.y);
		}
		else
		{
			this.position.x = Mathf.MoveTowards(this.position.x, 0f, Time.deltaTime * this.autoReturnSpeed.x);
			this.position.y = Mathf.MoveTowards(this.position.y, 0f, Time.deltaTime * this.autoReturnSpeed.y);
		}
	}

	// Token: 0x06004E2F RID: 20015 RVA: 0x001A3954 File Offset: 0x001A1D54
	protected override void ForEachTouch(Touch touch, Vector2 guiTouchPos)
	{
		if (this.touchZoneRect.Contains(touch.position) && (this.lastFingerId == -1 || this.lastFingerId != touch.fingerId))
		{
			this.lastFingerId = touch.fingerId;
			for (int i = 0; i < JoystickAbstract.joysticks.Length; i++)
			{
				if (JoystickAbstract.joysticks[i] != this)
				{
					JoystickAbstract.joysticks[i].LatchedFinger(touch.fingerId);
				}
			}
		}
	}

	// Token: 0x06004E30 RID: 20016 RVA: 0x001A39E0 File Offset: 0x001A1DE0
	protected override void MoveJoystickGraphic()
	{
		if (this.moveStick)
		{
			this.gui.pixelInset = new Rect(Mathf.Lerp(this.touchZoneRect.x, this.touchZoneRect.x + this.touchZoneRect.width, this.position.x * 0.5f + 0.5f) - this.defaultRect.width * 0.5f, Mathf.Lerp(this.touchZoneRect.y, this.touchZoneRect.y + this.touchZoneRect.height, this.position.y * 0.5f + 0.5f) - this.defaultRect.height * 0.5f, this.defaultRect.width, this.defaultRect.height);
		}
	}

	// Token: 0x04003650 RID: 13904
	public TouchPad.SensitivityRelativeTo sensitivityRelativeTo;

	// Token: 0x04003651 RID: 13905
	protected float sensitivityRelativeX;

	// Token: 0x04003652 RID: 13906
	protected float sensitivityRelativeY;

	// Token: 0x02000A18 RID: 2584
	public enum SensitivityRelativeTo
	{
		// Token: 0x04003654 RID: 13908
		ZoneSize,
		// Token: 0x04003655 RID: 13909
		Resolution
	}
}
