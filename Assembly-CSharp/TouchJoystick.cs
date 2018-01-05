using System;
using UnityEngine;

// Token: 0x02000A11 RID: 2577
public class TouchJoystick : MonoBehaviour
{
	// Token: 0x06004E26 RID: 20006 RVA: 0x001A28C8 File Offset: 0x001A0CC8
	private void OnEnable()
	{
		this.useX = (this.axesToUse == TouchJoystick.AxisOption.Both || this.axesToUse == TouchJoystick.AxisOption.OnlyHorizontal);
		this.useY = (this.axesToUse == TouchJoystick.AxisOption.Both || this.axesToUse == TouchJoystick.AxisOption.OnlyVertical);
		if (this.useX)
		{
			this.horizontalVirtualAxis = new CrossPlatformInput.VirtualAxis(this.horizontalAxisName);
		}
		if (this.useY)
		{
			this.verticalVirtualAxis = new CrossPlatformInput.VirtualAxis(this.verticalAxisName);
		}
		this.gui = base.GetComponent<GUITexture>();
		if (this.gui != null)
		{
			this.defaultRect = this.gui.GetScreenRect();
		}
		base.transform.position = new Vector3(0f, 0f, base.transform.position.z);
		this.moveStick = true;
		if (this.inputMode == TouchJoystick.InputMode.TouchPadPositional || this.inputMode == TouchJoystick.InputMode.TouchPadRelativePositional || this.inputMode == TouchJoystick.InputMode.TouchPadSwipe)
		{
			this.touchPad = true;
			this.getTouchZoneRect = true;
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
		}
		else
		{
			this.touchPad = false;
			this.guiTouchOffset.x = this.defaultRect.width * 0.5f;
			this.guiTouchOffset.y = this.defaultRect.height * 0.5f;
			this.guiCenter.x = this.defaultRect.x + this.guiTouchOffset.x;
			this.guiCenter.y = this.defaultRect.y + this.guiTouchOffset.y;
			this.guiBoundary.min.x = this.defaultRect.x - this.guiTouchOffset.x;
			this.guiBoundary.max.x = this.defaultRect.x + this.guiTouchOffset.x;
			this.guiBoundary.min.y = this.defaultRect.y - this.guiTouchOffset.y;
			this.guiBoundary.max.y = this.defaultRect.y + this.guiTouchOffset.y;
			this.moveStick = true;
		}
		if (this.gui != null)
		{
			this.gui.pixelInset = this.defaultRect;
			base.transform.localScale = Vector3.zero;
		}
	}

	// Token: 0x06004E27 RID: 20007 RVA: 0x001A2B85 File Offset: 0x001A0F85
	private void OnDisable()
	{
		TouchJoystick.enumeratedJoysticks = false;
		if (this.useX)
		{
			this.horizontalVirtualAxis.Remove();
		}
		if (this.useY)
		{
			this.verticalVirtualAxis.Remove();
		}
	}

	// Token: 0x06004E28 RID: 20008 RVA: 0x001A2BB9 File Offset: 0x001A0FB9
	private void ResetJoystick()
	{
		this.lastFingerId = -1;
	}

	// Token: 0x06004E29 RID: 20009 RVA: 0x001A2BC2 File Offset: 0x001A0FC2
	private void LatchedFinger(int fingerId)
	{
		if (this.lastFingerId == fingerId)
		{
			this.ResetJoystick();
		}
	}

	// Token: 0x06004E2A RID: 20010 RVA: 0x001A2BD8 File Offset: 0x001A0FD8
	public void Update()
	{
		if (this.touchPad && this.getTouchZoneRect)
		{
			this.getTouchZoneRect = false;
			this.touchZoneRect = this.touchZone.GetScreenRect();
			Vector2 center = this.touchZoneRect.center;
			this.touchZoneRect.width = this.touchZoneRect.width * (1f - this.touchZonePadding);
			this.touchZoneRect.height = this.touchZoneRect.height * (1f - this.touchZonePadding);
			this.touchZoneRect.center = center;
			this.position = this.startPosition;
			Vector2 vector = new Vector2(this.touchZoneRect.width, this.touchZoneRect.height);
			this.swipeScale = vector.magnitude * 0.01f;
			TouchJoystick.SensitivityRelativeTo sensitivityRelativeTo = this.sensitivityRelativeTo;
			if (sensitivityRelativeTo != TouchJoystick.SensitivityRelativeTo.ZoneSize)
			{
				if (sensitivityRelativeTo == TouchJoystick.SensitivityRelativeTo.Resolution)
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
		if (this.lastFingerId == -1 || this.inputMode == TouchJoystick.InputMode.TouchPadSwipe)
		{
			if (this.touchPad)
			{
				if (this.autoReturnStyle == TouchJoystick.ReturnStyleOption.Curved)
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
			else
			{
				Rect pixelInset = this.gui.pixelInset;
				if (this.autoReturnStyle == TouchJoystick.ReturnStyleOption.Curved)
				{
					pixelInset.x = Mathf.Lerp(pixelInset.x, this.defaultRect.x, Time.deltaTime * this.autoReturnSpeed.x * this.guiTouchOffset.x);
					pixelInset.y = Mathf.Lerp(pixelInset.y, this.defaultRect.y, Time.deltaTime * this.autoReturnSpeed.y * this.guiTouchOffset.y);
				}
				else
				{
					pixelInset.x = Mathf.MoveTowards(pixelInset.x, this.defaultRect.x, Time.deltaTime * this.autoReturnSpeed.x * this.guiTouchOffset.x);
					pixelInset.y = Mathf.MoveTowards(pixelInset.y, this.defaultRect.y, Time.deltaTime * this.autoReturnSpeed.y * this.guiTouchOffset.y);
				}
				this.gui.pixelInset = pixelInset;
			}
		}
		if (!TouchJoystick.enumeratedJoysticks)
		{
			TouchJoystick.joysticks = UnityEngine.Object.FindObjectsOfType<TouchJoystick>();
			TouchJoystick.enumeratedJoysticks = true;
		}
		int touchCount = Input.touchCount;
		if (touchCount == 0)
		{
			this.ResetJoystick();
		}
		else
		{
			for (int i = 0; i < touchCount; i++)
			{
				Touch touch = Input.GetTouch(i);
				Vector2 vector2 = touch.position - this.guiTouchOffset;
				bool flag = false;
				if (this.touchPad)
				{
					if (this.touchZoneRect.Contains(touch.position))
					{
						flag = true;
					}
				}
				else if (this.gui.HitTest(touch.position))
				{
					flag = true;
				}
				if (flag && (this.lastFingerId == -1 || this.lastFingerId != touch.fingerId))
				{
					if (this.touchPad)
					{
						this.lastFingerId = touch.fingerId;
					}
					this.lastFingerId = touch.fingerId;
					for (int j = 0; j < TouchJoystick.joysticks.Length; j++)
					{
						if (TouchJoystick.joysticks[j] != this)
						{
							TouchJoystick.joysticks[j].LatchedFinger(touch.fingerId);
						}
					}
				}
				if (this.lastFingerId == touch.fingerId)
				{
					if (this.touchPad)
					{
						TouchJoystick.InputMode inputMode = this.inputMode;
						if (inputMode != TouchJoystick.InputMode.TouchPadPositional)
						{
							if (inputMode != TouchJoystick.InputMode.TouchPadRelativePositional)
							{
								if (inputMode == TouchJoystick.InputMode.TouchPadSwipe)
								{
									if (touch.phase == TouchPhase.Began)
									{
										this.lastTouchPos = touch.position;
										this.touchDelta = Vector2.zero;
									}
									this.touchDelta = Vector2.Lerp(this.touchDelta, (this.lastTouchPos - touch.position) / this.swipeScale, Time.deltaTime * this.interpolateTime);
									if (touch.deltaTime > 0f)
									{
										if (this.useX)
										{
											float x = this.touchDelta.x * this.sensitivity;
											this.position.x = x;
										}
										if (this.useY)
										{
											float y = this.touchDelta.y * this.sensitivity;
											this.position.y = y;
										}
									}
									this.lastTouchPos = touch.position;
								}
							}
							else
							{
								if (touch.phase == TouchPhase.Began)
								{
									this.touchStart = touch.position;
								}
								Vector2 a = new Vector2((touch.position.x - this.touchStart.x) / this.sensitivityRelativeX, (touch.position.y - this.touchStart.y) / this.sensitivityRelativeY);
								Vector2 vector3 = Vector2.Lerp(this.position, a * this.sensitivity * 2f, Time.deltaTime * this.interpolateTime);
								if (this.useX)
								{
									this.position.x = Mathf.Clamp(vector3.x, -1f, 1f);
								}
								if (this.useY)
								{
									this.position.y = Mathf.Clamp(vector3.y, -1f, 1f);
								}
							}
						}
						else
						{
							Vector2 a2 = new Vector2((touch.position.x - this.touchZoneRect.center.x) / this.sensitivityRelativeX, (touch.position.y - this.touchZoneRect.center.y) / this.sensitivityRelativeY) * 2f;
							Vector2 vector4 = Vector2.Lerp(this.position, a2 * this.sensitivity, Time.deltaTime * this.interpolateTime);
							if (this.useX)
							{
								this.position.x = Mathf.Clamp(vector4.x, -1f, 1f);
							}
							if (this.useY)
							{
								this.position.y = Mathf.Clamp(vector4.y, -1f, 1f);
							}
						}
					}
					else
					{
						this.gui.pixelInset = new Rect(Mathf.Clamp(vector2.x, this.guiBoundary.min.x, this.guiBoundary.max.x), Mathf.Clamp(vector2.y, this.guiBoundary.min.y, this.guiBoundary.max.y), this.gui.pixelInset.width, this.gui.pixelInset.height);
					}
					if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
					{
						this.ResetJoystick();
					}
				}
			}
		}
		if (this.touchPad && this.moveStick)
		{
			this.gui.pixelInset = new Rect(Mathf.Lerp(this.touchZoneRect.x, this.touchZoneRect.x + this.touchZoneRect.width, this.position.x * 0.5f + 0.5f) - this.defaultRect.width * 0.5f, Mathf.Lerp(this.touchZoneRect.y, this.touchZoneRect.y + this.touchZoneRect.height, this.position.y * 0.5f + 0.5f) - this.defaultRect.height * 0.5f, this.defaultRect.width, this.defaultRect.height);
		}
		if (!this.touchPad)
		{
			if (this.useX)
			{
				this.position.x = (this.gui.pixelInset.x + this.guiTouchOffset.x - this.guiCenter.x) / this.guiTouchOffset.x;
			}
			if (this.useY)
			{
				this.position.y = (this.gui.pixelInset.y + this.guiTouchOffset.y - this.guiCenter.y) / this.guiTouchOffset.y;
			}
		}
		float num2 = this.position.x;
		float num3 = this.position.y;
		float num4 = Mathf.Abs(num2);
		float num5 = Mathf.Abs(num3);
		if (num4 < this.deadZone.x)
		{
			num2 = 0f;
		}
		else if (this.normalize)
		{
			num2 = Mathf.Sign(num2) * (num4 - this.deadZone.x) / (1f - this.deadZone.x);
		}
		if (num5 < this.deadZone.y)
		{
			num3 = 0f;
		}
		else if (this.normalize)
		{
			num3 = Mathf.Sign(num3) * (num5 - this.deadZone.y) / (1f - this.deadZone.y);
		}
		num2 *= (float)((!this.invertX) ? 1 : -1);
		num3 *= (float)((!this.invertY) ? 1 : -1);
		if (this.useX)
		{
			this.horizontalVirtualAxis.Update(num2);
		}
		if (this.useY)
		{
			this.verticalVirtualAxis.Update(num3);
		}
	}

	// Token: 0x04003617 RID: 13847
	public Vector2 deadZone = Vector2.zero;

	// Token: 0x04003618 RID: 13848
	public bool normalize;

	// Token: 0x04003619 RID: 13849
	public Vector2 autoReturnSpeed = new Vector2(4f, 4f);

	// Token: 0x0400361A RID: 13850
	public string horizontalAxisName = "Horizontal";

	// Token: 0x0400361B RID: 13851
	public string verticalAxisName = "Vertical";

	// Token: 0x0400361C RID: 13852
	public TouchJoystick.AxisOption axesToUse;

	// Token: 0x0400361D RID: 13853
	public bool invertX;

	// Token: 0x0400361E RID: 13854
	public bool invertY;

	// Token: 0x0400361F RID: 13855
	public TouchJoystick.InputMode inputMode;

	// Token: 0x04003620 RID: 13856
	public GUITexture touchZone;

	// Token: 0x04003621 RID: 13857
	public float touchZonePadding;

	// Token: 0x04003622 RID: 13858
	public TouchJoystick.ReturnStyleOption autoReturnStyle = TouchJoystick.ReturnStyleOption.Curved;

	// Token: 0x04003623 RID: 13859
	public float sensitivity = 1f;

	// Token: 0x04003624 RID: 13860
	public float interpolateTime = 2f;

	// Token: 0x04003625 RID: 13861
	public Vector2 startPosition = Vector2.zero;

	// Token: 0x04003626 RID: 13862
	public bool relativeSensitivity;

	// Token: 0x04003627 RID: 13863
	public TouchJoystick.SensitivityRelativeTo sensitivityRelativeTo;

	// Token: 0x04003628 RID: 13864
	private static TouchJoystick[] joysticks;

	// Token: 0x04003629 RID: 13865
	private static bool enumeratedJoysticks;

	// Token: 0x0400362A RID: 13866
	private Rect touchZoneRect;

	// Token: 0x0400362B RID: 13867
	private Vector2 position;

	// Token: 0x0400362C RID: 13868
	private int lastFingerId = -1;

	// Token: 0x0400362D RID: 13869
	private GUITexture gui;

	// Token: 0x0400362E RID: 13870
	private Rect defaultRect;

	// Token: 0x0400362F RID: 13871
	private TouchJoystick.Boundary guiBoundary = new TouchJoystick.Boundary();

	// Token: 0x04003630 RID: 13872
	private Vector2 guiTouchOffset;

	// Token: 0x04003631 RID: 13873
	private Vector2 guiCenter;

	// Token: 0x04003632 RID: 13874
	private bool moveStick;

	// Token: 0x04003633 RID: 13875
	private bool touchPad;

	// Token: 0x04003634 RID: 13876
	private CrossPlatformInput.VirtualAxis horizontalVirtualAxis;

	// Token: 0x04003635 RID: 13877
	private CrossPlatformInput.VirtualAxis verticalVirtualAxis;

	// Token: 0x04003636 RID: 13878
	private bool useX;

	// Token: 0x04003637 RID: 13879
	private bool useY;

	// Token: 0x04003638 RID: 13880
	private bool getTouchZoneRect;

	// Token: 0x04003639 RID: 13881
	private Vector2 lastTouchPos;

	// Token: 0x0400363A RID: 13882
	private Vector2 touchDelta;

	// Token: 0x0400363B RID: 13883
	private Vector2 touchStart;

	// Token: 0x0400363C RID: 13884
	private float swipeScale;

	// Token: 0x0400363D RID: 13885
	private float sensitivityRelativeX;

	// Token: 0x0400363E RID: 13886
	private float sensitivityRelativeY;

	// Token: 0x02000A12 RID: 2578
	public enum AxisOption
	{
		// Token: 0x04003640 RID: 13888
		Both,
		// Token: 0x04003641 RID: 13889
		OnlyHorizontal,
		// Token: 0x04003642 RID: 13890
		OnlyVertical
	}

	// Token: 0x02000A13 RID: 2579
	public enum ReturnStyleOption
	{
		// Token: 0x04003644 RID: 13892
		Linear,
		// Token: 0x04003645 RID: 13893
		Curved
	}

	// Token: 0x02000A14 RID: 2580
	public enum InputMode
	{
		// Token: 0x04003647 RID: 13895
		Joystick,
		// Token: 0x04003648 RID: 13896
		TouchPadPositional,
		// Token: 0x04003649 RID: 13897
		TouchPadRelativePositional,
		// Token: 0x0400364A RID: 13898
		TouchPadSwipe
	}

	// Token: 0x02000A15 RID: 2581
	public enum SensitivityRelativeTo
	{
		// Token: 0x0400364C RID: 13900
		ZoneSize,
		// Token: 0x0400364D RID: 13901
		Resolution
	}

	// Token: 0x02000A16 RID: 2582
	private class Boundary
	{
		// Token: 0x0400364E RID: 13902
		public Vector2 min = Vector2.zero;

		// Token: 0x0400364F RID: 13903
		public Vector2 max = Vector2.zero;
	}
}
