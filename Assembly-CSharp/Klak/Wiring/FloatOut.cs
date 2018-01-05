using System;
using System.Reflection;
using UnityEngine;

namespace Klak.Wiring
{
	// Token: 0x0200054F RID: 1359
	[AddComponentMenu("Klak/Wiring/Output/Generic/Float Out")]
	public class FloatOut : NodeBase
	{
		// Token: 0x1700071E RID: 1822
		// (set) Token: 0x06002EE6 RID: 12006 RVA: 0x000E43A8 File Offset: 0x000E27A8
		[Inlet]
		public float input
		{
			set
			{
				if (!base.enabled || this._target == null || this._propertyInfo == null)
				{
					return;
				}
				this._propertyInfo.SetValue(this._target, value, null);
			}
		}

		// Token: 0x06002EE7 RID: 12007 RVA: 0x000E43F5 File Offset: 0x000E27F5
		private void OnEnable()
		{
			if (this._target == null || string.IsNullOrEmpty(this._propertyName))
			{
				return;
			}
			this._propertyInfo = this._target.GetType().GetProperty(this._propertyName);
		}

		// Token: 0x04001945 RID: 6469
		[SerializeField]
		private Component _target;

		// Token: 0x04001946 RID: 6470
		[SerializeField]
		private string _propertyName;

		// Token: 0x04001947 RID: 6471
		private PropertyInfo _propertyInfo;
	}
}
