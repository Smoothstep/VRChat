using System;
using System.Reflection;
using UnityEngine;

namespace Klak.Wiring
{
	// Token: 0x0200054D RID: 1357
	[AddComponentMenu("Klak/Wiring/Output/Generic/Color Out")]
	public class ColorOut : NodeBase
	{
		// Token: 0x1700071D RID: 1821
		// (set) Token: 0x06002EE1 RID: 12001 RVA: 0x000E42FC File Offset: 0x000E26FC
		[Inlet]
		public Color input
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

		// Token: 0x06002EE2 RID: 12002 RVA: 0x000E4349 File Offset: 0x000E2749
		private void OnEnable()
		{
			if (this._target == null || string.IsNullOrEmpty(this._propertyName))
			{
				return;
			}
			this._propertyInfo = this._target.GetType().GetProperty(this._propertyName);
		}

		// Token: 0x04001941 RID: 6465
		[SerializeField]
		private Component _target;

		// Token: 0x04001942 RID: 6466
		[SerializeField]
		private string _propertyName;

		// Token: 0x04001943 RID: 6467
		private PropertyInfo _propertyInfo;
	}
}
