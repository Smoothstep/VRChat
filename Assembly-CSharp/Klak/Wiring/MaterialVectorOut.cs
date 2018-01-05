using System;
using UnityEngine;

namespace Klak.Wiring
{
	// Token: 0x02000552 RID: 1362
	[AddComponentMenu("Klak/Wiring/Output/Renderer/Material Vector Out")]
	public class MaterialVectorOut : NodeBase
	{
		// Token: 0x17000721 RID: 1825
		// (set) Token: 0x06002EEF RID: 12015 RVA: 0x000E4544 File Offset: 0x000E2944
		[Inlet]
		public Vector3 vectorInput
		{
			set
			{
				if (!base.enabled || this._target == null || this._propertyID < 0)
				{
					return;
				}
				this._target.material.SetVector(this._propertyID, value);
			}
		}

		// Token: 0x06002EF0 RID: 12016 RVA: 0x000E4596 File Offset: 0x000E2996
		private void OnEnable()
		{
			if (!string.IsNullOrEmpty(this._propertyName))
			{
				this._propertyID = Shader.PropertyToID(this._propertyName);
			}
		}

		// Token: 0x0400194E RID: 6478
		[SerializeField]
		private Renderer _target;

		// Token: 0x0400194F RID: 6479
		[SerializeField]
		private string _propertyName;

		// Token: 0x04001950 RID: 6480
		private int _propertyID = -1;
	}
}
