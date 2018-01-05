using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B15 RID: 2837
public class VRCInputProcessor : MonoBehaviour
{
	// Token: 0x17000C71 RID: 3185
	// (get) Token: 0x0600562A RID: 22058 RVA: 0x001DAC00 File Offset: 0x001D9000
	public virtual bool required
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000C72 RID: 3186
	// (get) Token: 0x0600562B RID: 22059 RVA: 0x001DAC03 File Offset: 0x001D9003
	public virtual bool supported
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000C73 RID: 3187
	// (get) Token: 0x0600562C RID: 22060 RVA: 0x001DAC06 File Offset: 0x001D9006
	public virtual bool platformDefaultEnable
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000C74 RID: 3188
	// (get) Token: 0x0600562D RID: 22061 RVA: 0x001DAC09 File Offset: 0x001D9009
	public virtual bool present
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600562E RID: 22062 RVA: 0x001DAC0C File Offset: 0x001D900C
	public bool AnyKey()
	{
		return this._anyKey;
	}

	// Token: 0x17000C75 RID: 3189
	// (get) Token: 0x0600562F RID: 22063 RVA: 0x001DAC14 File Offset: 0x001D9014
	public virtual string StorageTag
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000C76 RID: 3190
	// (get) Token: 0x06005630 RID: 22064 RVA: 0x001DAC17 File Offset: 0x001D9017
	// (set) Token: 0x06005631 RID: 22065 RVA: 0x001DAC1F File Offset: 0x001D901F
	public bool inputEnabled
	{
		get
		{
			return this._inputEnabled;
		}
		set
		{
			this._inputEnabled = value;
			Storage.Write(this.StorageTag, value);
		}
	}

	// Token: 0x06005632 RID: 22066 RVA: 0x001DAC39 File Offset: 0x001D9039
	public virtual void Awake()
	{
		this.Init();
	}

	// Token: 0x06005633 RID: 22067 RVA: 0x001DAC44 File Offset: 0x001D9044
	protected void Init()
	{
		this._inputEnabled = (bool)Storage.Read(this.StorageTag, typeof(bool), this.platformDefaultEnable);
		if (this.required && !this._inputEnabled)
		{
			this.inputEnabled = true;
		}
	}

	// Token: 0x06005634 RID: 22068 RVA: 0x001DAC99 File Offset: 0x001D9099
	public virtual void Apply()
	{
	}

	// Token: 0x06005635 RID: 22069 RVA: 0x001DAC9B File Offset: 0x001D909B
	public virtual void ConnectInputs(Dictionary<string, VRCInput> inputs)
	{
	}

	// Token: 0x04003CDD RID: 15581
	protected bool _anyKey;

	// Token: 0x04003CDE RID: 15582
	private bool _inputEnabled;
}
