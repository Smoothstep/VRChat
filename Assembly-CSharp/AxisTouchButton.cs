using System;
using UnityEngine;

// Token: 0x020009FD RID: 2557
[RequireComponent(typeof(GUIElement))]
public class AxisTouchButton : MonoBehaviour
{
	// Token: 0x06004DCB RID: 19915 RVA: 0x001A1465 File Offset: 0x0019F865
	private void OnEnable()
	{
		this.axis = (CrossPlatformInput.VirtualAxisReference(this.axisName) ?? new CrossPlatformInput.VirtualAxis(this.axisName));
		this.rect = base.GetComponent<GUIElement>().GetScreenRect();
		this.FindPairedButton();
	}

	// Token: 0x06004DCC RID: 19916 RVA: 0x001A14A4 File Offset: 0x0019F8A4
	private void FindPairedButton()
	{
		AxisTouchButton[] array = UnityEngine.Object.FindObjectsOfType(typeof(AxisTouchButton)) as AxisTouchButton[];
		if (array != null)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].axisName == this.axisName && array[i] != this)
				{
					this.pairedWith = array[i];
					this.axisCentre = (this.axisValue + array[i].axisValue) / 2f;
				}
			}
		}
	}

	// Token: 0x06004DCD RID: 19917 RVA: 0x001A152A File Offset: 0x0019F92A
	private void OnDisable()
	{
		this.axis.Remove();
	}

	// Token: 0x06004DCE RID: 19918 RVA: 0x001A1538 File Offset: 0x0019F938
	private void Update()
	{
		if (this.pairedWith == null)
		{
			this.FindPairedButton();
		}
		this.pressedThisFrame = false;
		for (int i = 0; i < Input.touchCount; i++)
		{
			if (this.rect.Contains(Input.GetTouch(i).position))
			{
				this.axis.Update(Mathf.MoveTowards(this.axis.GetValue, this.axisValue, this.responseSpeed * Time.deltaTime));
				this.pressedThisFrame = true;
			}
		}
	}

	// Token: 0x06004DCF RID: 19919 RVA: 0x001A15CC File Offset: 0x0019F9CC
	private void LateUpdate()
	{
		if (this.pairedWith != null && !this.pressedThisFrame && !this.pairedWith.pressedThisFrame)
		{
			this.axis.Update(Mathf.MoveTowards(this.axis.GetValue, this.axisCentre, this.returnToCentreSpeed * Time.deltaTime * 0.5f));
		}
	}

	// Token: 0x040035BF RID: 13759
	public string axisName = "Horizontal";

	// Token: 0x040035C0 RID: 13760
	public float axisValue = 1f;

	// Token: 0x040035C1 RID: 13761
	public float responseSpeed = 3f;

	// Token: 0x040035C2 RID: 13762
	public float returnToCentreSpeed = 3f;

	// Token: 0x040035C3 RID: 13763
	private AxisTouchButton pairedWith;

	// Token: 0x040035C4 RID: 13764
	private Rect rect;

	// Token: 0x040035C5 RID: 13765
	private CrossPlatformInput.VirtualAxis axis;

	// Token: 0x040035C6 RID: 13766
	private bool pressedThisFrame;

	// Token: 0x040035C7 RID: 13767
	private float axisCentre;
}
