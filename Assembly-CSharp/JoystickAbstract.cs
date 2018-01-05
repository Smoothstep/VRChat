using System;
using UnityEngine;

// Token: 0x02000A08 RID: 2568
public abstract class JoystickAbstract : MonoBehaviour
{
	// Token: 0x06004E0A RID: 19978 RVA: 0x001A1B3B File Offset: 0x0019FF3B
	protected virtual void TypeSpecificOnEnable()
	{
	}

	// Token: 0x06004E0B RID: 19979 RVA: 0x001A1B40 File Offset: 0x0019FF40
	protected void OnEnable()
	{
		this.CreateVirtualAxes();
		this.gui = base.GetComponent<GUITexture>();
		if (this.gui != null)
		{
			this.defaultRect = this.gui.GetScreenRect();
			this.gui.pixelInset = this.defaultRect;
			base.transform.localScale = Vector3.zero;
		}
		base.transform.position = new Vector3(0f, 0f, base.transform.position.z);
		this.moveStick = true;
		this.TypeSpecificOnEnable();
		if (JoystickAbstract.enumeratedJoysticks)
		{
			return;
		}
		JoystickAbstract.joysticks = UnityEngine.Object.FindObjectsOfType<JoystickAbstract>();
		JoystickAbstract.enumeratedJoysticks = true;
	}

	// Token: 0x06004E0C RID: 19980 RVA: 0x001A1BF8 File Offset: 0x0019FFF8
	private void CreateVirtualAxes()
	{
		this.useX = (this.axesToUse == JoystickAbstract.AxisOption.Both || this.axesToUse == JoystickAbstract.AxisOption.OnlyHorizontal);
		this.useY = (this.axesToUse == JoystickAbstract.AxisOption.Both || this.axesToUse == JoystickAbstract.AxisOption.OnlyVertical);
		if (this.useX)
		{
			this.horizontalVirtualAxis = new CrossPlatformInput.VirtualAxis(this.horizontalAxisName);
		}
		if (this.useY)
		{
			this.verticalVirtualAxis = new CrossPlatformInput.VirtualAxis(this.verticalAxisName);
		}
	}

	// Token: 0x06004E0D RID: 19981 RVA: 0x001A1C77 File Offset: 0x001A0077
	protected void OnDisable()
	{
		JoystickAbstract.enumeratedJoysticks = false;
		if (this.useX)
		{
			this.horizontalVirtualAxis.Remove();
		}
		if (this.useY)
		{
			this.verticalVirtualAxis.Remove();
		}
	}

	// Token: 0x06004E0E RID: 19982 RVA: 0x001A1CAB File Offset: 0x001A00AB
	protected void ResetJoystick()
	{
		this.lastFingerId = -1;
	}

	// Token: 0x06004E0F RID: 19983 RVA: 0x001A1CB4 File Offset: 0x001A00B4
	protected internal virtual void LatchedFinger(int fingerId)
	{
		if (this.lastFingerId == fingerId)
		{
			this.ResetJoystick();
		}
	}

	// Token: 0x06004E10 RID: 19984 RVA: 0x001A1CC8 File Offset: 0x001A00C8
	protected virtual void TypeSpecificUpdate()
	{
	}

	// Token: 0x06004E11 RID: 19985 RVA: 0x001A1CCA File Offset: 0x001A00CA
	protected virtual void ZeroWhenUnused()
	{
	}

	// Token: 0x06004E12 RID: 19986 RVA: 0x001A1CCC File Offset: 0x001A00CC
	protected virtual void ForEachTouch(Touch touch, Vector2 guiTouchPos)
	{
	}

	// Token: 0x06004E13 RID: 19987 RVA: 0x001A1CCE File Offset: 0x001A00CE
	protected virtual void MoveJoystickGraphic()
	{
	}

	// Token: 0x06004E14 RID: 19988 RVA: 0x001A1CD0 File Offset: 0x001A00D0
	public void Update()
	{
		this.ZeroWhenUnused();
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
				Vector2 guiTouchPos = touch.position - this.guiTouchOffset;
				this.ForEachTouch(touch, guiTouchPos);
			}
		}
		this.MoveJoystickGraphic();
		float x = this.position.x;
		float y = this.position.y;
		this.DeadZoneAndNormaliseAxes(ref x, ref y);
		this.AdjustAxesIfInverted(ref x, ref y);
		this.UpdateVirtualAxes(x, y);
	}

	// Token: 0x06004E15 RID: 19989 RVA: 0x001A1D6C File Offset: 0x001A016C
	private void DeadZoneAndNormaliseAxes(ref float modifiedX, ref float modifiedY)
	{
		float num = Mathf.Abs(modifiedX);
		float num2 = Mathf.Abs(modifiedY);
		if (num < this.deadZone.x)
		{
			modifiedX = 0f;
		}
		else if (this.normalize)
		{
			modifiedX = Mathf.Sign(modifiedX) * (num - this.deadZone.x) / (1f - this.deadZone.x);
		}
		if (num2 < this.deadZone.y)
		{
			modifiedY = 0f;
		}
		else if (this.normalize)
		{
			modifiedY = Mathf.Sign(modifiedY) * (num2 - this.deadZone.y) / (1f - this.deadZone.y);
		}
	}

	// Token: 0x06004E16 RID: 19990 RVA: 0x001A1E2B File Offset: 0x001A022B
	private void AdjustAxesIfInverted(ref float modifiedX, ref float modifiedY)
	{
		modifiedX *= (float)((!this.invertX) ? 1 : -1);
		modifiedY *= (float)((!this.invertY) ? 1 : -1);
	}

	// Token: 0x06004E17 RID: 19991 RVA: 0x001A1E5D File Offset: 0x001A025D
	private void UpdateVirtualAxes(float modifiedX, float modifiedY)
	{
		if (this.useX)
		{
			this.horizontalVirtualAxis.Update(modifiedX);
		}
		if (this.useY)
		{
			this.verticalVirtualAxis.Update(modifiedY);
		}
	}

	// Token: 0x040035DE RID: 13790
	public Vector2 deadZone = Vector2.zero;

	// Token: 0x040035DF RID: 13791
	public bool normalize;

	// Token: 0x040035E0 RID: 13792
	public Vector2 autoReturnSpeed = new Vector2(4f, 4f);

	// Token: 0x040035E1 RID: 13793
	public string horizontalAxisName = "Horizontal";

	// Token: 0x040035E2 RID: 13794
	public string verticalAxisName = "Vertical";

	// Token: 0x040035E3 RID: 13795
	public JoystickAbstract.AxisOption axesToUse;

	// Token: 0x040035E4 RID: 13796
	public bool invertX;

	// Token: 0x040035E5 RID: 13797
	public bool invertY;

	// Token: 0x040035E6 RID: 13798
	public GUITexture touchZone;

	// Token: 0x040035E7 RID: 13799
	public float touchZonePadding;

	// Token: 0x040035E8 RID: 13800
	public JoystickAbstract.ReturnStyleOption autoReturnStyle = JoystickAbstract.ReturnStyleOption.Curved;

	// Token: 0x040035E9 RID: 13801
	public float sensitivity = 1f;

	// Token: 0x040035EA RID: 13802
	public float interpolateTime = 2f;

	// Token: 0x040035EB RID: 13803
	public Vector2 startPosition = Vector2.zero;

	// Token: 0x040035EC RID: 13804
	protected static JoystickAbstract[] joysticks;

	// Token: 0x040035ED RID: 13805
	protected static bool enumeratedJoysticks;

	// Token: 0x040035EE RID: 13806
	protected Rect touchZoneRect;

	// Token: 0x040035EF RID: 13807
	protected Vector2 position;

	// Token: 0x040035F0 RID: 13808
	protected int lastFingerId = -1;

	// Token: 0x040035F1 RID: 13809
	protected GUITexture gui;

	// Token: 0x040035F2 RID: 13810
	protected Rect defaultRect;

	// Token: 0x040035F3 RID: 13811
	protected Rect guiBoundary = default(Rect);

	// Token: 0x040035F4 RID: 13812
	protected Vector2 guiTouchOffset;

	// Token: 0x040035F5 RID: 13813
	protected Vector2 guiCenter;

	// Token: 0x040035F6 RID: 13814
	protected bool moveStick;

	// Token: 0x040035F7 RID: 13815
	protected bool touchPad;

	// Token: 0x040035F8 RID: 13816
	protected CrossPlatformInput.VirtualAxis horizontalVirtualAxis;

	// Token: 0x040035F9 RID: 13817
	protected CrossPlatformInput.VirtualAxis verticalVirtualAxis;

	// Token: 0x040035FA RID: 13818
	protected bool useX;

	// Token: 0x040035FB RID: 13819
	protected bool useY;

	// Token: 0x040035FC RID: 13820
	protected bool getTouchZoneRect;

	// Token: 0x040035FD RID: 13821
	protected Vector2 lastTouchPos;

	// Token: 0x040035FE RID: 13822
	protected Vector2 touchDelta;

	// Token: 0x040035FF RID: 13823
	protected Vector2 touchStart;

	// Token: 0x04003600 RID: 13824
	protected float swipeScale;

	// Token: 0x02000A09 RID: 2569
	public enum AxisOption
	{
		// Token: 0x04003602 RID: 13826
		Both,
		// Token: 0x04003603 RID: 13827
		OnlyHorizontal,
		// Token: 0x04003604 RID: 13828
		OnlyVertical
	}

	// Token: 0x02000A0A RID: 2570
	public enum ReturnStyleOption
	{
		// Token: 0x04003606 RID: 13830
		Linear,
		// Token: 0x04003607 RID: 13831
		Curved
	}
}
