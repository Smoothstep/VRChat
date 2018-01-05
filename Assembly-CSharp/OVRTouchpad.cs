using System;
using UnityEngine;

// Token: 0x020006D4 RID: 1748
public static class OVRTouchpad
{
	// Token: 0x14000047 RID: 71
	// (add) Token: 0x060039D2 RID: 14802 RVA: 0x00123954 File Offset: 0x00121D54
	// (remove) Token: 0x060039D3 RID: 14803 RVA: 0x00123988 File Offset: 0x00121D88
	public static event EventHandler TouchHandler;

	// Token: 0x060039D4 RID: 14804 RVA: 0x001239BC File Offset: 0x00121DBC
	public static void Create()
	{
	}

	// Token: 0x060039D5 RID: 14805 RVA: 0x001239C0 File Offset: 0x00121DC0
	public static void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			OVRTouchpad.moveAmountMouse = Input.mousePosition;
			OVRTouchpad.touchState = OVRTouchpad.TouchState.Down;
		}
		else if (Input.GetMouseButtonUp(0))
		{
			OVRTouchpad.moveAmountMouse -= Input.mousePosition;
			OVRTouchpad.HandleInputMouse(ref OVRTouchpad.moveAmountMouse);
			OVRTouchpad.touchState = OVRTouchpad.TouchState.Init;
		}
	}

	// Token: 0x060039D6 RID: 14806 RVA: 0x00123A1C File Offset: 0x00121E1C
	public static void OnDisable()
	{
	}

	// Token: 0x060039D7 RID: 14807 RVA: 0x00123A20 File Offset: 0x00121E20
	private static void HandleInput(OVRTouchpad.TouchState state, ref Vector2 move)
	{
		if (move.magnitude >= OVRTouchpad.minMovMagnitude && OVRTouchpad.touchState != OVRTouchpad.TouchState.Stationary)
		{
			if (OVRTouchpad.touchState == OVRTouchpad.TouchState.Move)
			{
				move.Normalize();
				if (Mathf.Abs(move.x) > Mathf.Abs(move.y))
				{
					if (move.x > 0f)
					{
					}
				}
				else if (move.y > 0f)
				{
				}
			}
		}
	}

	// Token: 0x060039D8 RID: 14808 RVA: 0x00123AA8 File Offset: 0x00121EA8
	private static void HandleInputMouse(ref Vector3 move)
	{
		if (move.magnitude < OVRTouchpad.minMovMagnitudeMouse)
		{
			if (OVRTouchpad.TouchHandler != null)
			{
				OVRTouchpad.TouchHandler(null, new OVRTouchpad.TouchArgs
				{
					TouchType = OVRTouchpad.TouchEvent.SingleTap
				});
			}
		}
		else
		{
			move.Normalize();
			if (Mathf.Abs(move.x) > Mathf.Abs(move.y))
			{
				if (move.x > 0f)
				{
					if (OVRTouchpad.TouchHandler != null)
					{
						OVRTouchpad.TouchHandler(null, new OVRTouchpad.TouchArgs
						{
							TouchType = OVRTouchpad.TouchEvent.Left
						});
					}
				}
				else if (OVRTouchpad.TouchHandler != null)
				{
					OVRTouchpad.TouchHandler(null, new OVRTouchpad.TouchArgs
					{
						TouchType = OVRTouchpad.TouchEvent.Right
					});
				}
			}
			else if (move.y > 0f)
			{
				if (OVRTouchpad.TouchHandler != null)
				{
					OVRTouchpad.TouchHandler(null, new OVRTouchpad.TouchArgs
					{
						TouchType = OVRTouchpad.TouchEvent.Down
					});
				}
			}
			else if (OVRTouchpad.TouchHandler != null)
			{
				OVRTouchpad.TouchHandler(null, new OVRTouchpad.TouchArgs
				{
					TouchType = OVRTouchpad.TouchEvent.Up
				});
			}
		}
	}

	// Token: 0x040022A8 RID: 8872
	private static OVRTouchpad.TouchState touchState = OVRTouchpad.TouchState.Init;

	// Token: 0x040022A9 RID: 8873
	private static Vector2 moveAmount;

	// Token: 0x040022AA RID: 8874
	private static float minMovMagnitude = 100f;

	// Token: 0x040022AB RID: 8875
	private static Vector3 moveAmountMouse;

	// Token: 0x040022AC RID: 8876
	private static float minMovMagnitudeMouse = 25f;

	// Token: 0x040022AD RID: 8877
	private static OVRTouchpadHelper touchpadHelper = new GameObject("OVRTouchpadHelper").AddComponent<OVRTouchpadHelper>();

	// Token: 0x020006D5 RID: 1749
	public enum TouchEvent
	{
		// Token: 0x040022AF RID: 8879
		SingleTap,
		// Token: 0x040022B0 RID: 8880
		Left,
		// Token: 0x040022B1 RID: 8881
		Right,
		// Token: 0x040022B2 RID: 8882
		Up,
		// Token: 0x040022B3 RID: 8883
		Down
	}

	// Token: 0x020006D6 RID: 1750
	public class TouchArgs : EventArgs
	{
		// Token: 0x040022B4 RID: 8884
		public OVRTouchpad.TouchEvent TouchType;
	}

	// Token: 0x020006D7 RID: 1751
	private enum TouchState
	{
		// Token: 0x040022B6 RID: 8886
		Init,
		// Token: 0x040022B7 RID: 8887
		Down,
		// Token: 0x040022B8 RID: 8888
		Stationary,
		// Token: 0x040022B9 RID: 8889
		Move,
		// Token: 0x040022BA RID: 8890
		Up
	}
}
