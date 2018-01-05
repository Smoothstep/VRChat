using System;
using UnityEngine;

namespace Klak.Wiring
{
	// Token: 0x02000551 RID: 1361
	[AddComponentMenu("Klak/Wiring/Output/Renderer/Material Float Out")]
	public class MaterialFloatOut : NodeBase
	{
		// Token: 0x17000720 RID: 1824
		// (set) Token: 0x06002EEC RID: 12012 RVA: 0x000E44C4 File Offset: 0x000E28C4
		[Inlet]
		public float floatInput
		{
			set
			{
				if (!base.enabled || this._target == null || this._propertyID < 0)
				{
					return;
				}
				this._target.material.SetFloat(this._propertyID, value);
			}
		}

		// Token: 0x06002EED RID: 12013 RVA: 0x000E4511 File Offset: 0x000E2911
		private void OnEnable()
		{
			if (!string.IsNullOrEmpty(this._propertyName))
			{
				this._propertyID = Shader.PropertyToID(this._propertyName);
			}
		}

		// Token: 0x0400194B RID: 6475
		[SerializeField]
		private Renderer _target;

		// Token: 0x0400194C RID: 6476
		[SerializeField]
		private string _propertyName;

		// Token: 0x0400194D RID: 6477
		private int _propertyID = -1;
	}
}
