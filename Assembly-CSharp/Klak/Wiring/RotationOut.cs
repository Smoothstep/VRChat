using System;
using System.Reflection;
using UnityEngine;

namespace Klak.Wiring
{
	// Token: 0x02000554 RID: 1364
	[AddComponentMenu("Klak/Wiring/Output/Generic/Rotation Out")]
	public class RotationOut : NodeBase
	{
		// Token: 0x17000723 RID: 1827
		// (set) Token: 0x06002EF6 RID: 12022 RVA: 0x000E461C File Offset: 0x000E2A1C
		[Inlet]
		public Quaternion input
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

		// Token: 0x06002EF7 RID: 12023 RVA: 0x000E4669 File Offset: 0x000E2A69
		private void OnEnable()
		{
			if (this._target == null || string.IsNullOrEmpty(this._propertyName))
			{
				return;
			}
			this._propertyInfo = this._target.GetType().GetProperty(this._propertyName);
		}

		// Token: 0x04001953 RID: 6483
		[SerializeField]
		private Component _target;

		// Token: 0x04001954 RID: 6484
		[SerializeField]
		private string _propertyName;

		// Token: 0x04001955 RID: 6485
		private PropertyInfo _propertyInfo;
	}
}
