using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000A8D RID: 2701
public class GearPadGestures : MonoBehaviour
{
	// Token: 0x17000BFD RID: 3069
	// (get) Token: 0x0600515E RID: 20830 RVA: 0x001BE280 File Offset: 0x001BC680
	public static GearPadGestures Instance
	{
		get
		{
			return GearPadGestures.instance;
		}
	}

	// Token: 0x0600515F RID: 20831 RVA: 0x001BE287 File Offset: 0x001BC687
	private void Awake()
	{
		GearPadGestures.instanceCount++;
		if (GearPadGestures.instanceCount > 1)
		{
			Debug.LogWarning("Cannot have more than one GearPadGestures script in a scene");
		}
		if (GearPadGestures.instance == null)
		{
			GearPadGestures.instance = this;
		}
	}

	// Token: 0x06005160 RID: 20832 RVA: 0x001BE2C0 File Offset: 0x001BC6C0
	private void Update()
	{
		if (VRCApplication.AppType == VRCApplication.VRCAppType.GearDemo)
		{
			this.UpdateGesture();
			this.UpdateSwipe();
		}
	}

	// Token: 0x06005161 RID: 20833 RVA: 0x001BE2DC File Offset: 0x001BC6DC
	private void UpdateSwipe()
	{
		if (Input.GetMouseButtonDown(0))
		{
			this.startPos = Input.mousePosition;
			this.currentPos = this.startPos;
		}
		else if (Input.GetMouseButton(0))
		{
			this.currentPos = Input.mousePosition;
		}
		else if (Input.GetMouseButtonUp(0))
		{
			this.justRecognizedGesture = false;
			this.startPos = (this.currentPos = Vector3.zero);
			base.StopCoroutine("CheckForTapAndHold");
		}
		this.swipeDir = this.currentPos - this.startPos;
	}

	// Token: 0x06005162 RID: 20834 RVA: 0x001BE374 File Offset: 0x001BC774
	private void UpdateGesture()
	{
		float num = this.swipeDir.x;
		float num2 = this.swipeDir.y;
		if (this.currentGesture == GearPadGestures.Gesture.TapAndHold)
		{
			if (Mathf.Abs(num) > this.gestureThreshhold || Mathf.Abs(num2) > this.gestureThreshhold)
			{
				this.SetGesture(GearPadGestures.Gesture.TapAndHoldCancel);
			}
			else if (Input.GetMouseButtonUp(0))
			{
				this.SetGesture(GearPadGestures.Gesture.TapAndHoldSelect);
			}
		}
		else
		{
			this.currentGesture = GearPadGestures.Gesture.None;
		}
		if (!this.justRecognizedGesture)
		{
			GearPadGestures.Gesture gesture = GearPadGestures.Gesture.None;
			if (Mathf.Abs(num) > Mathf.Abs(num2))
			{
				num2 = 0f;
			}
			else
			{
				num = 0f;
			}
			if (Input.GetMouseButtonDown(0))
			{
				base.StopCoroutine("CheckForTapAndHold");
				base.StartCoroutine("CheckForTapAndHold", 0.5f);
			}
			if (num > this.gestureThreshhold)
			{
				gesture = GearPadGestures.Gesture.Backward;
			}
			else if (num < -this.gestureThreshhold)
			{
				gesture = GearPadGestures.Gesture.Forward;
			}
			else if (num2 > this.gestureThreshhold)
			{
				gesture = GearPadGestures.Gesture.Up;
			}
			else if (num2 < -this.gestureThreshhold)
			{
				gesture = GearPadGestures.Gesture.Down;
			}
			else if (Input.GetMouseButtonUp(0))
			{
				gesture = GearPadGestures.Gesture.Tap;
			}
			if (gesture != GearPadGestures.Gesture.None)
			{
				this.SetGesture(gesture);
			}
		}
	}

	// Token: 0x06005163 RID: 20835 RVA: 0x001BE4B4 File Offset: 0x001BC8B4
	private void SetGesture(GearPadGestures.Gesture gesture)
	{
		this.justRecognizedGesture = true;
	}

	// Token: 0x06005164 RID: 20836 RVA: 0x001BE4C0 File Offset: 0x001BC8C0
	private IEnumerator CheckForTapAndHold(float holdTime)
	{
		yield return new WaitForSeconds(holdTime);
		if (!this.justRecognizedGesture)
		{
			this.SetGesture(GearPadGestures.Gesture.TapAndHold);
		}
		yield break;
	}

	// Token: 0x06005165 RID: 20837 RVA: 0x001BE4E2 File Offset: 0x001BC8E2
	public bool GetTap()
	{
		return this.currentGesture == GearPadGestures.Gesture.Tap;
	}

	// Token: 0x06005166 RID: 20838 RVA: 0x001BE4ED File Offset: 0x001BC8ED
	public bool GetForwardGesture()
	{
		return this.currentGesture == GearPadGestures.Gesture.Forward;
	}

	// Token: 0x06005167 RID: 20839 RVA: 0x001BE4F8 File Offset: 0x001BC8F8
	public bool GetBackwardGesture()
	{
		return this.currentGesture == GearPadGestures.Gesture.Backward;
	}

	// Token: 0x06005168 RID: 20840 RVA: 0x001BE503 File Offset: 0x001BC903
	public bool GetUpGesture()
	{
		return this.currentGesture == GearPadGestures.Gesture.Up;
	}

	// Token: 0x06005169 RID: 20841 RVA: 0x001BE50E File Offset: 0x001BC90E
	public bool GetDownGesture()
	{
		return this.currentGesture == GearPadGestures.Gesture.Down;
	}

	// Token: 0x0600516A RID: 20842 RVA: 0x001BE519 File Offset: 0x001BC919
	public bool GetTapAndHold()
	{
		return this.currentGesture == GearPadGestures.Gesture.TapAndHold;
	}

	// Token: 0x0600516B RID: 20843 RVA: 0x001BE524 File Offset: 0x001BC924
	public bool GetTapAndHoldSelect()
	{
		return this.currentGesture == GearPadGestures.Gesture.TapAndHoldSelect;
	}

	// Token: 0x0600516C RID: 20844 RVA: 0x001BE52F File Offset: 0x001BC92F
	public bool GetTapAndHoldCancel()
	{
		return this.currentGesture == GearPadGestures.Gesture.TapAndHoldCancel;
	}

	// Token: 0x040039A0 RID: 14752
	private GearPadGestures.Gesture currentGesture;

	// Token: 0x040039A1 RID: 14753
	private Vector3 startPos;

	// Token: 0x040039A2 RID: 14754
	private Vector3 currentPos;

	// Token: 0x040039A3 RID: 14755
	private Vector3 swipeDir;

	// Token: 0x040039A4 RID: 14756
	public float holdTime = 0.25f;

	// Token: 0x040039A5 RID: 14757
	private float gestureThreshhold = 50f;

	// Token: 0x040039A6 RID: 14758
	private bool justRecognizedGesture;

	// Token: 0x040039A7 RID: 14759
	private static int instanceCount;

	// Token: 0x040039A8 RID: 14760
	private static GearPadGestures instance;

	// Token: 0x02000A8E RID: 2702
	private enum Gesture
	{
		// Token: 0x040039AA RID: 14762
		None,
		// Token: 0x040039AB RID: 14763
		Tap,
		// Token: 0x040039AC RID: 14764
		TapAndHold,
		// Token: 0x040039AD RID: 14765
		TapAndHoldSelect,
		// Token: 0x040039AE RID: 14766
		TapAndHoldCancel,
		// Token: 0x040039AF RID: 14767
		Forward,
		// Token: 0x040039B0 RID: 14768
		Backward,
		// Token: 0x040039B1 RID: 14769
		Up,
		// Token: 0x040039B2 RID: 14770
		Down
	}
}
