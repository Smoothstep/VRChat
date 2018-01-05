using System;
using System.Runtime.InteropServices;
using UnityEngine;
using Valve.VR;

// Token: 0x02000B92 RID: 2962
public class SteamVR_TrackedController : MonoBehaviour
{
	// Token: 0x14000079 RID: 121
	// (add) Token: 0x06005C0B RID: 23563 RVA: 0x0020232C File Offset: 0x0020072C
	// (remove) Token: 0x06005C0C RID: 23564 RVA: 0x00202364 File Offset: 0x00200764
	public event ClickedEventHandler MenuButtonClicked;

	// Token: 0x1400007A RID: 122
	// (add) Token: 0x06005C0D RID: 23565 RVA: 0x0020239C File Offset: 0x0020079C
	// (remove) Token: 0x06005C0E RID: 23566 RVA: 0x002023D4 File Offset: 0x002007D4
	public event ClickedEventHandler MenuButtonUnclicked;

	// Token: 0x1400007B RID: 123
	// (add) Token: 0x06005C0F RID: 23567 RVA: 0x0020240C File Offset: 0x0020080C
	// (remove) Token: 0x06005C10 RID: 23568 RVA: 0x00202444 File Offset: 0x00200844
	public event ClickedEventHandler TriggerClicked;

	// Token: 0x1400007C RID: 124
	// (add) Token: 0x06005C11 RID: 23569 RVA: 0x0020247C File Offset: 0x0020087C
	// (remove) Token: 0x06005C12 RID: 23570 RVA: 0x002024B4 File Offset: 0x002008B4
	public event ClickedEventHandler TriggerUnclicked;

	// Token: 0x1400007D RID: 125
	// (add) Token: 0x06005C13 RID: 23571 RVA: 0x002024EC File Offset: 0x002008EC
	// (remove) Token: 0x06005C14 RID: 23572 RVA: 0x00202524 File Offset: 0x00200924
	public event ClickedEventHandler SteamClicked;

	// Token: 0x1400007E RID: 126
	// (add) Token: 0x06005C15 RID: 23573 RVA: 0x0020255C File Offset: 0x0020095C
	// (remove) Token: 0x06005C16 RID: 23574 RVA: 0x00202594 File Offset: 0x00200994
	public event ClickedEventHandler PadClicked;

	// Token: 0x1400007F RID: 127
	// (add) Token: 0x06005C17 RID: 23575 RVA: 0x002025CC File Offset: 0x002009CC
	// (remove) Token: 0x06005C18 RID: 23576 RVA: 0x00202604 File Offset: 0x00200A04
	public event ClickedEventHandler PadUnclicked;

	// Token: 0x14000080 RID: 128
	// (add) Token: 0x06005C19 RID: 23577 RVA: 0x0020263C File Offset: 0x00200A3C
	// (remove) Token: 0x06005C1A RID: 23578 RVA: 0x00202674 File Offset: 0x00200A74
	public event ClickedEventHandler PadTouched;

	// Token: 0x14000081 RID: 129
	// (add) Token: 0x06005C1B RID: 23579 RVA: 0x002026AC File Offset: 0x00200AAC
	// (remove) Token: 0x06005C1C RID: 23580 RVA: 0x002026E4 File Offset: 0x00200AE4
	public event ClickedEventHandler PadUntouched;

	// Token: 0x14000082 RID: 130
	// (add) Token: 0x06005C1D RID: 23581 RVA: 0x0020271C File Offset: 0x00200B1C
	// (remove) Token: 0x06005C1E RID: 23582 RVA: 0x00202754 File Offset: 0x00200B54
	public event ClickedEventHandler Gripped;

	// Token: 0x14000083 RID: 131
	// (add) Token: 0x06005C1F RID: 23583 RVA: 0x0020278C File Offset: 0x00200B8C
	// (remove) Token: 0x06005C20 RID: 23584 RVA: 0x002027C4 File Offset: 0x00200BC4
	public event ClickedEventHandler Ungripped;

	// Token: 0x06005C21 RID: 23585 RVA: 0x002027FC File Offset: 0x00200BFC
	protected virtual void Start()
	{
		if (base.GetComponent<SteamVR_TrackedObject>() == null)
		{
			base.gameObject.AddComponent<SteamVR_TrackedObject>();
		}
		if (this.controllerIndex != 0u)
		{
			base.GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)this.controllerIndex;
			if (base.GetComponent<SteamVR_RenderModel>() != null)
			{
				base.GetComponent<SteamVR_RenderModel>().index = (SteamVR_TrackedObject.EIndex)this.controllerIndex;
			}
		}
		else
		{
			this.controllerIndex = (uint)base.GetComponent<SteamVR_TrackedObject>().index;
		}
	}

	// Token: 0x06005C22 RID: 23586 RVA: 0x0020287A File Offset: 0x00200C7A
	public void SetDeviceIndex(int index)
	{
		this.controllerIndex = (uint)index;
	}

	// Token: 0x06005C23 RID: 23587 RVA: 0x00202883 File Offset: 0x00200C83
	public virtual void OnTriggerClicked(ClickedEventArgs e)
	{
		if (this.TriggerClicked != null)
		{
			this.TriggerClicked(this, e);
		}
	}

	// Token: 0x06005C24 RID: 23588 RVA: 0x0020289D File Offset: 0x00200C9D
	public virtual void OnTriggerUnclicked(ClickedEventArgs e)
	{
		if (this.TriggerUnclicked != null)
		{
			this.TriggerUnclicked(this, e);
		}
	}

	// Token: 0x06005C25 RID: 23589 RVA: 0x002028B7 File Offset: 0x00200CB7
	public virtual void OnMenuClicked(ClickedEventArgs e)
	{
		if (this.MenuButtonClicked != null)
		{
			this.MenuButtonClicked(this, e);
		}
	}

	// Token: 0x06005C26 RID: 23590 RVA: 0x002028D1 File Offset: 0x00200CD1
	public virtual void OnMenuUnclicked(ClickedEventArgs e)
	{
		if (this.MenuButtonUnclicked != null)
		{
			this.MenuButtonUnclicked(this, e);
		}
	}

	// Token: 0x06005C27 RID: 23591 RVA: 0x002028EB File Offset: 0x00200CEB
	public virtual void OnSteamClicked(ClickedEventArgs e)
	{
		if (this.SteamClicked != null)
		{
			this.SteamClicked(this, e);
		}
	}

	// Token: 0x06005C28 RID: 23592 RVA: 0x00202905 File Offset: 0x00200D05
	public virtual void OnPadClicked(ClickedEventArgs e)
	{
		if (this.PadClicked != null)
		{
			this.PadClicked(this, e);
		}
	}

	// Token: 0x06005C29 RID: 23593 RVA: 0x0020291F File Offset: 0x00200D1F
	public virtual void OnPadUnclicked(ClickedEventArgs e)
	{
		if (this.PadUnclicked != null)
		{
			this.PadUnclicked(this, e);
		}
	}

	// Token: 0x06005C2A RID: 23594 RVA: 0x00202939 File Offset: 0x00200D39
	public virtual void OnPadTouched(ClickedEventArgs e)
	{
		if (this.PadTouched != null)
		{
			this.PadTouched(this, e);
		}
	}

	// Token: 0x06005C2B RID: 23595 RVA: 0x00202953 File Offset: 0x00200D53
	public virtual void OnPadUntouched(ClickedEventArgs e)
	{
		if (this.PadUntouched != null)
		{
			this.PadUntouched(this, e);
		}
	}

	// Token: 0x06005C2C RID: 23596 RVA: 0x0020296D File Offset: 0x00200D6D
	public virtual void OnGripped(ClickedEventArgs e)
	{
		if (this.Gripped != null)
		{
			this.Gripped(this, e);
		}
	}

	// Token: 0x06005C2D RID: 23597 RVA: 0x00202987 File Offset: 0x00200D87
	public virtual void OnUngripped(ClickedEventArgs e)
	{
		if (this.Ungripped != null)
		{
			this.Ungripped(this, e);
		}
	}

	// Token: 0x06005C2E RID: 23598 RVA: 0x002029A4 File Offset: 0x00200DA4
	protected virtual void Update()
	{
		CVRSystem system = OpenVR.System;
		if (system != null && system.GetControllerState(this.controllerIndex, ref this.controllerState, (uint)Marshal.SizeOf(typeof(VRControllerState_t))))
		{
			ulong num = this.controllerState.ulButtonPressed & 8589934592UL;
			if (num > 0UL && !this.triggerPressed)
			{
				this.triggerPressed = true;
				ClickedEventArgs e;
				e.controllerIndex = this.controllerIndex;
				e.flags = (uint)this.controllerState.ulButtonPressed;
				e.padX = this.controllerState.rAxis0.x;
				e.padY = this.controllerState.rAxis0.y;
				this.OnTriggerClicked(e);
			}
			else if (num == 0UL && this.triggerPressed)
			{
				this.triggerPressed = false;
				ClickedEventArgs e2;
				e2.controllerIndex = this.controllerIndex;
				e2.flags = (uint)this.controllerState.ulButtonPressed;
				e2.padX = this.controllerState.rAxis0.x;
				e2.padY = this.controllerState.rAxis0.y;
				this.OnTriggerUnclicked(e2);
			}
			ulong num2 = this.controllerState.ulButtonPressed & 4UL;
			if (num2 > 0UL && !this.gripped)
			{
				this.gripped = true;
				ClickedEventArgs e3;
				e3.controllerIndex = this.controllerIndex;
				e3.flags = (uint)this.controllerState.ulButtonPressed;
				e3.padX = this.controllerState.rAxis0.x;
				e3.padY = this.controllerState.rAxis0.y;
				this.OnGripped(e3);
			}
			else if (num2 == 0UL && this.gripped)
			{
				this.gripped = false;
				ClickedEventArgs e4;
				e4.controllerIndex = this.controllerIndex;
				e4.flags = (uint)this.controllerState.ulButtonPressed;
				e4.padX = this.controllerState.rAxis0.x;
				e4.padY = this.controllerState.rAxis0.y;
				this.OnUngripped(e4);
			}
			ulong num3 = this.controllerState.ulButtonPressed & 4294967296UL;
			if (num3 > 0UL && !this.padPressed)
			{
				this.padPressed = true;
				ClickedEventArgs e5;
				e5.controllerIndex = this.controllerIndex;
				e5.flags = (uint)this.controllerState.ulButtonPressed;
				e5.padX = this.controllerState.rAxis0.x;
				e5.padY = this.controllerState.rAxis0.y;
				this.OnPadClicked(e5);
			}
			else if (num3 == 0UL && this.padPressed)
			{
				this.padPressed = false;
				ClickedEventArgs e6;
				e6.controllerIndex = this.controllerIndex;
				e6.flags = (uint)this.controllerState.ulButtonPressed;
				e6.padX = this.controllerState.rAxis0.x;
				e6.padY = this.controllerState.rAxis0.y;
				this.OnPadUnclicked(e6);
			}
			ulong num4 = this.controllerState.ulButtonPressed & 2UL;
			if (num4 > 0UL && !this.menuPressed)
			{
				this.menuPressed = true;
				ClickedEventArgs e7;
				e7.controllerIndex = this.controllerIndex;
				e7.flags = (uint)this.controllerState.ulButtonPressed;
				e7.padX = this.controllerState.rAxis0.x;
				e7.padY = this.controllerState.rAxis0.y;
				this.OnMenuClicked(e7);
			}
			else if (num4 == 0UL && this.menuPressed)
			{
				this.menuPressed = false;
				ClickedEventArgs e8;
				e8.controllerIndex = this.controllerIndex;
				e8.flags = (uint)this.controllerState.ulButtonPressed;
				e8.padX = this.controllerState.rAxis0.x;
				e8.padY = this.controllerState.rAxis0.y;
				this.OnMenuUnclicked(e8);
			}
			num3 = (this.controllerState.ulButtonTouched & 4294967296UL);
			if (num3 > 0UL && !this.padTouched)
			{
				this.padTouched = true;
				ClickedEventArgs e9;
				e9.controllerIndex = this.controllerIndex;
				e9.flags = (uint)this.controllerState.ulButtonPressed;
				e9.padX = this.controllerState.rAxis0.x;
				e9.padY = this.controllerState.rAxis0.y;
				this.OnPadTouched(e9);
			}
			else if (num3 == 0UL && this.padTouched)
			{
				this.padTouched = false;
				ClickedEventArgs e10;
				e10.controllerIndex = this.controllerIndex;
				e10.flags = (uint)this.controllerState.ulButtonPressed;
				e10.padX = this.controllerState.rAxis0.x;
				e10.padY = this.controllerState.rAxis0.y;
				this.OnPadUntouched(e10);
			}
		}
	}

	// Token: 0x040041A1 RID: 16801
	public uint controllerIndex;

	// Token: 0x040041A2 RID: 16802
	public VRControllerState_t controllerState;

	// Token: 0x040041A3 RID: 16803
	public bool triggerPressed;

	// Token: 0x040041A4 RID: 16804
	public bool steamPressed;

	// Token: 0x040041A5 RID: 16805
	public bool menuPressed;

	// Token: 0x040041A6 RID: 16806
	public bool padPressed;

	// Token: 0x040041A7 RID: 16807
	public bool padTouched;

	// Token: 0x040041A8 RID: 16808
	public bool gripped;
}
