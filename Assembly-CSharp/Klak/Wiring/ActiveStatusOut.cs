using System;
using System.Reflection;
using UnityEngine;

namespace Klak.Wiring
{
	// Token: 0x0200054B RID: 1355
	[AddComponentMenu("Klak/Wiring/Output/Component/Active Status Out")]
	public class ActiveStatusOut : NodeBase
	{
		// Token: 0x06002ED9 RID: 11993 RVA: 0x000E41F7 File Offset: 0x000E25F7
		[Inlet]
		public void Enable()
		{
			if (base.enabled)
			{
				this.SetActive(true);
			}
		}

		// Token: 0x06002EDA RID: 11994 RVA: 0x000E420B File Offset: 0x000E260B
		[Inlet]
		public void Disable()
		{
			if (base.enabled)
			{
				this.SetActive(false);
			}
		}

		// Token: 0x06002EDB RID: 11995 RVA: 0x000E421F File Offset: 0x000E261F
		private void OnEnable()
		{
			if (this._targetComponent != null)
			{
				this._propertyInfo = this._targetComponent.GetType().GetProperty("enabled");
			}
		}

		// Token: 0x06002EDC RID: 11996 RVA: 0x000E4250 File Offset: 0x000E2650
		private void SetActive(bool flag)
		{
			if (this._targetComponent != null && this._propertyInfo != null)
			{
				this._propertyInfo.SetValue(this._targetComponent, flag, null);
			}
			if (this._targetGameObject != null)
			{
				this._targetGameObject.SetActive(flag);
			}
		}

		// Token: 0x0400193C RID: 6460
		[SerializeField]
		private Component _targetComponent;

		// Token: 0x0400193D RID: 6461
		[SerializeField]
		private GameObject _targetGameObject;

		// Token: 0x0400193E RID: 6462
		private PropertyInfo _propertyInfo;
	}
}
