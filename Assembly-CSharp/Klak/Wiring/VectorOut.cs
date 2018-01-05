using System;
using System.Reflection;
using UnityEngine;

namespace Klak.Wiring
{
	// Token: 0x02000557 RID: 1367
	[AddComponentMenu("Klak/Wiring/Output/Generic/Vector Out")]
	public class VectorOut : NodeBase
	{
		// Token: 0x17000730 RID: 1840
		// (set) Token: 0x06002F09 RID: 12041 RVA: 0x000E4960 File Offset: 0x000E2D60
		[Inlet]
		public Vector3 input
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

		// Token: 0x06002F0A RID: 12042 RVA: 0x000E49AD File Offset: 0x000E2DAD
		private void OnEnable()
		{
			if (this._target == null || string.IsNullOrEmpty(this._propertyName))
			{
				return;
			}
			this._propertyInfo = this._target.GetType().GetProperty(this._propertyName);
		}

		// Token: 0x0400195B RID: 6491
		[SerializeField]
		private Component _target;

		// Token: 0x0400195C RID: 6492
		[SerializeField]
		private string _propertyName;

		// Token: 0x0400195D RID: 6493
		private PropertyInfo _propertyInfo;
	}
}
