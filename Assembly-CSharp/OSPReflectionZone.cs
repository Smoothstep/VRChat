using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000664 RID: 1636
public class OSPReflectionZone : MonoBehaviour
{
	// Token: 0x170008BA RID: 2234
	// (get) Token: 0x0600376F RID: 14191 RVA: 0x0011B2C8 File Offset: 0x001196C8
	// (set) Token: 0x06003770 RID: 14192 RVA: 0x0011B2D0 File Offset: 0x001196D0
	public Vector3 Dimensions
	{
		get
		{
			return this.dimensions;
		}
		set
		{
			this.dimensions = value;
			this.dimensions.x = Mathf.Clamp(this.dimensions.x, 1f, 200f);
			this.dimensions.y = Mathf.Clamp(this.dimensions.y, 1f, 200f);
			this.dimensions.z = Mathf.Clamp(this.dimensions.z, 1f, 200f);
		}
	}

	// Token: 0x170008BB RID: 2235
	// (get) Token: 0x06003771 RID: 14193 RVA: 0x0011B353 File Offset: 0x00119753
	// (set) Token: 0x06003772 RID: 14194 RVA: 0x0011B35C File Offset: 0x0011975C
	public Vector2 RK01
	{
		get
		{
			return this.rK01;
		}
		set
		{
			this.rK01 = value;
			this.rK01.x = Mathf.Clamp(this.rK01.x, 0f, 0.97f);
			this.rK01.y = Mathf.Clamp(this.rK01.y, 0f, 0.97f);
		}
	}

	// Token: 0x170008BC RID: 2236
	// (get) Token: 0x06003773 RID: 14195 RVA: 0x0011B3BA File Offset: 0x001197BA
	// (set) Token: 0x06003774 RID: 14196 RVA: 0x0011B3C4 File Offset: 0x001197C4
	public Vector2 RK23
	{
		get
		{
			return this.rK23;
		}
		set
		{
			this.rK23 = value;
			this.rK23.x = Mathf.Clamp(this.rK23.x, 0f, 0.95f);
			this.rK23.y = Mathf.Clamp(this.rK23.y, 0f, 0.95f);
		}
	}

	// Token: 0x170008BD RID: 2237
	// (get) Token: 0x06003775 RID: 14197 RVA: 0x0011B422 File Offset: 0x00119822
	// (set) Token: 0x06003776 RID: 14198 RVA: 0x0011B42C File Offset: 0x0011982C
	public Vector2 RK45
	{
		get
		{
			return this.rK45;
		}
		set
		{
			this.rK45 = value;
			this.rK45.x = Mathf.Clamp(this.rK45.x, 0f, 0.95f);
			this.rK45.y = Mathf.Clamp(this.rK45.y, 0f, 0.95f);
		}
	}

	// Token: 0x06003777 RID: 14199 RVA: 0x0011B48A File Offset: 0x0011988A
	private void Start()
	{
	}

	// Token: 0x06003778 RID: 14200 RVA: 0x0011B48C File Offset: 0x0011988C
	private void Update()
	{
	}

	// Token: 0x06003779 RID: 14201 RVA: 0x0011B48E File Offset: 0x0011988E
	private void OnTriggerEnter(Collider other)
	{
		if (this.CheckForAudioListener(other.gameObject))
		{
			this.PushCurrentReflectionValues();
		}
	}

	// Token: 0x0600377A RID: 14202 RVA: 0x0011B4A7 File Offset: 0x001198A7
	private void OnTriggerExit(Collider other)
	{
		if (this.CheckForAudioListener(other.gameObject))
		{
			this.PopCurrentReflectionValues();
		}
	}

	// Token: 0x0600377B RID: 14203 RVA: 0x0011B4C0 File Offset: 0x001198C0
	private bool CheckForAudioListener(GameObject gameObject)
	{
		AudioListener componentInChildren = gameObject.GetComponentInChildren<AudioListener>();
		return componentInChildren != null;
	}

	// Token: 0x0600377C RID: 14204 RVA: 0x0011B4E4 File Offset: 0x001198E4
	private void PushCurrentReflectionValues()
	{
		if (OSPManager.sInstance == null)
		{
			Debug.LogWarning(string.Format("OSPReflectionZone-PushCurrentReflectionValues: OSPManager does not exist in scene.", new object[0]));
			return;
		}
		OSPManager.RoomModel t = default(OSPManager.RoomModel);
		t.DimensionX = OSPManager.sInstance.Dimensions.x;
		t.DimensionY = OSPManager.sInstance.Dimensions.y;
		t.DimensionZ = OSPManager.sInstance.Dimensions.z;
		t.Reflection_K0 = OSPManager.sInstance.RK01.x;
		t.Reflection_K1 = OSPManager.sInstance.RK01.y;
		t.Reflection_K2 = OSPManager.sInstance.RK23.x;
		t.Reflection_K3 = OSPManager.sInstance.RK23.y;
		t.Reflection_K4 = OSPManager.sInstance.RK45.x;
		t.Reflection_K5 = OSPManager.sInstance.RK45.y;
		OSPReflectionZone.reflectionList.Push(t);
		this.SetReflectionValues();
	}

	// Token: 0x0600377D RID: 14205 RVA: 0x0011B618 File Offset: 0x00119A18
	private void PopCurrentReflectionValues()
	{
		if (OSPManager.sInstance == null)
		{
			Debug.LogWarning(string.Format("OSPReflectionZone-PopCurrentReflectionValues: OSPManager does not exist in scene.", new object[0]));
			return;
		}
		if (OSPReflectionZone.reflectionList.Count == 0)
		{
			Debug.LogWarning(string.Format("OSPReflectionZone-PopCurrentReflectionValues: reflectionList is empty.", new object[0]));
			return;
		}
		OSPManager.RoomModel roomModel = OSPReflectionZone.reflectionList.Pop();
		this.SetReflectionValues(ref roomModel);
	}

	// Token: 0x0600377E RID: 14206 RVA: 0x0011B684 File Offset: 0x00119A84
	private void SetReflectionValues()
	{
		OSPManager.sInstance.Dimensions = this.Dimensions;
		OSPManager.sInstance.RK01 = this.RK01;
		OSPManager.sInstance.RK23 = this.RK23;
		OSPManager.sInstance.RK45 = this.RK45;
	}

	// Token: 0x0600377F RID: 14207 RVA: 0x0011B6D4 File Offset: 0x00119AD4
	private void SetReflectionValues(ref OSPManager.RoomModel rm)
	{
		OSPManager.sInstance.Dimensions = new Vector3(rm.DimensionX, rm.DimensionY, rm.DimensionZ);
		OSPManager.sInstance.RK01 = new Vector3(rm.Reflection_K0, rm.Reflection_K1);
		OSPManager.sInstance.RK23 = new Vector3(rm.Reflection_K2, rm.Reflection_K3);
		OSPManager.sInstance.RK45 = new Vector3(rm.Reflection_K4, rm.Reflection_K5);
	}

	// Token: 0x04002025 RID: 8229
	[SerializeField]
	private Vector3 dimensions = new Vector3(0f, 0f, 0f);

	// Token: 0x04002026 RID: 8230
	[SerializeField]
	private Vector2 rK01 = new Vector2(0f, 0f);

	// Token: 0x04002027 RID: 8231
	[SerializeField]
	private Vector2 rK23 = new Vector2(0f, 0f);

	// Token: 0x04002028 RID: 8232
	[SerializeField]
	private Vector2 rK45 = new Vector2(0f, 0f);

	// Token: 0x04002029 RID: 8233
	private static Stack<OSPManager.RoomModel> reflectionList = new Stack<OSPManager.RoomModel>();
}
