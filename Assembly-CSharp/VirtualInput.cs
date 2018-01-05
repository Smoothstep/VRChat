using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A1C RID: 2588
public abstract class VirtualInput
{
	// Token: 0x17000BAC RID: 2988
	// (get) Token: 0x06004E39 RID: 20025 RVA: 0x001A22F0 File Offset: 0x001A06F0
	// (set) Token: 0x06004E3A RID: 20026 RVA: 0x001A22F8 File Offset: 0x001A06F8
	public Vector3 virtualMousePosition { get; private set; }

	// Token: 0x06004E3B RID: 20027 RVA: 0x001A2304 File Offset: 0x001A0704
	public void RegisterVirtualAxis(CrossPlatformInput.VirtualAxis axis)
	{
		if (this.virtualAxes.ContainsKey(axis.name))
		{
			Debug.LogError("There is already a virtual axis named " + axis.name + " registered.");
		}
		else
		{
			this.virtualAxes.Add(axis.name, axis);
			if (!axis.matchWithInputManager)
			{
				this.alwaysUseVirtual.Add(axis.name);
			}
		}
	}

	// Token: 0x06004E3C RID: 20028 RVA: 0x001A2374 File Offset: 0x001A0774
	public void RegisterVirtualButton(CrossPlatformInput.VirtualButton button)
	{
		if (this.virtualButtons.ContainsKey(button.name))
		{
			Debug.LogError("There is already a virtual button named " + button.name + " registered.");
		}
		else
		{
			this.virtualButtons.Add(button.name, button);
			if (!button.matchWithInputManager)
			{
				this.alwaysUseVirtual.Add(button.name);
			}
		}
	}

	// Token: 0x06004E3D RID: 20029 RVA: 0x001A23E4 File Offset: 0x001A07E4
	public void UnRegisterVirtualAxis(string name)
	{
		if (this.virtualAxes.ContainsKey(name))
		{
			this.virtualAxes.Remove(name);
		}
	}

	// Token: 0x06004E3E RID: 20030 RVA: 0x001A2404 File Offset: 0x001A0804
	public void UnRegisterVirtualButton(string name)
	{
		if (this.virtualButtons.ContainsKey(name))
		{
			this.virtualButtons.Remove(name);
		}
	}

	// Token: 0x06004E3F RID: 20031 RVA: 0x001A2424 File Offset: 0x001A0824
	public CrossPlatformInput.VirtualAxis VirtualAxisReference(string name)
	{
		return (!this.virtualAxes.ContainsKey(name)) ? null : this.virtualAxes[name];
	}

	// Token: 0x06004E40 RID: 20032 RVA: 0x001A244C File Offset: 0x001A084C
	public void SetVirtualMousePositionX(float f)
	{
		this.virtualMousePosition = new Vector3(f, this.virtualMousePosition.y, this.virtualMousePosition.z);
	}

	// Token: 0x06004E41 RID: 20033 RVA: 0x001A2484 File Offset: 0x001A0884
	public void SetVirtualMousePositionY(float f)
	{
		this.virtualMousePosition = new Vector3(this.virtualMousePosition.x, f, this.virtualMousePosition.z);
	}

	// Token: 0x06004E42 RID: 20034 RVA: 0x001A24BC File Offset: 0x001A08BC
	public void SetVirtualMousePositionZ(float f)
	{
		this.virtualMousePosition = new Vector3(this.virtualMousePosition.x, this.virtualMousePosition.y, f);
	}

	// Token: 0x06004E43 RID: 20035
	public abstract float GetAxis(string name, bool raw);

	// Token: 0x06004E44 RID: 20036
	public abstract bool GetButton(string name, CrossPlatformInput.ButtonAction action);

	// Token: 0x06004E45 RID: 20037
	public abstract Vector3 MousePosition();

	// Token: 0x04003656 RID: 13910
	protected Dictionary<string, CrossPlatformInput.VirtualAxis> virtualAxes = new Dictionary<string, CrossPlatformInput.VirtualAxis>();

	// Token: 0x04003657 RID: 13911
	protected Dictionary<string, CrossPlatformInput.VirtualButton> virtualButtons = new Dictionary<string, CrossPlatformInput.VirtualButton>();

	// Token: 0x04003658 RID: 13912
	protected List<string> alwaysUseVirtual = new List<string>();
}
