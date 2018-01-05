using System;
using UnityEngine;

namespace Klak.Wiring
{
	// Token: 0x02000550 RID: 1360
	[AddComponentMenu("Klak/Wiring/Output/Renderer/Material Color Out")]
	public class MaterialColorOut : NodeBase
	{
		// Token: 0x1700071F RID: 1823
		// (set) Token: 0x06002EE9 RID: 12009 RVA: 0x000E4444 File Offset: 0x000E2844
		[Inlet]
		public Color colorInput
		{
			set
			{
				if (!base.enabled || this._target == null || this._propertyID < 0)
				{
					return;
				}
				this._target.material.SetColor(this._propertyID, value);
			}
		}

		// Token: 0x06002EEA RID: 12010 RVA: 0x000E4491 File Offset: 0x000E2891
		private void OnEnable()
		{
			if (!string.IsNullOrEmpty(this._propertyName))
			{
				this._propertyID = Shader.PropertyToID(this._propertyName);
			}
		}

		// Token: 0x04001948 RID: 6472
		[SerializeField]
		private Renderer _target;

		// Token: 0x04001949 RID: 6473
		[SerializeField]
		private string _propertyName;

		// Token: 0x0400194A RID: 6474
		private int _propertyID = -1;
	}
}
