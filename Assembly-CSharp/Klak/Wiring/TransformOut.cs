using System;
using UnityEngine;

namespace Klak.Wiring
{
	// Token: 0x02000556 RID: 1366
	[AddComponentMenu("Klak/Wiring/Output/Component/Transform Out")]
	public class TransformOut : NodeBase
	{
		// Token: 0x1700072C RID: 1836
		// (set) Token: 0x06002F02 RID: 12034 RVA: 0x000E4758 File Offset: 0x000E2B58
		[Inlet]
		public Vector3 position
		{
			set
			{
				if (!base.enabled || this._targetTransform == null)
				{
					return;
				}
				this._targetTransform.localPosition = ((!this._addToOriginal) ? value : (this._originalPosition + value));
			}
		}

		// Token: 0x1700072D RID: 1837
		// (set) Token: 0x06002F03 RID: 12035 RVA: 0x000E47AC File Offset: 0x000E2BAC
		[Inlet]
		public Quaternion rotation
		{
			set
			{
				if (!base.enabled || this._targetTransform == null)
				{
					return;
				}
				this._targetTransform.localRotation = ((!this._addToOriginal) ? value : (this._originalRotation * value));
			}
		}

		// Token: 0x1700072E RID: 1838
		// (set) Token: 0x06002F04 RID: 12036 RVA: 0x000E4800 File Offset: 0x000E2C00
		[Inlet]
		public Vector3 scale
		{
			set
			{
				if (!base.enabled || this._targetTransform == null)
				{
					return;
				}
				this._targetTransform.localScale = ((!this._addToOriginal) ? value : (this._originalScale + value));
			}
		}

		// Token: 0x1700072F RID: 1839
		// (set) Token: 0x06002F05 RID: 12037 RVA: 0x000E4854 File Offset: 0x000E2C54
		[Inlet]
		public float uniformScale
		{
			set
			{
				if (!base.enabled || this._targetTransform == null)
				{
					return;
				}
				Vector3 vector = Vector3.one * value;
				if (this._addToOriginal)
				{
					vector += this._originalScale;
				}
				this._targetTransform.localScale = vector;
			}
		}

		// Token: 0x06002F06 RID: 12038 RVA: 0x000E48B0 File Offset: 0x000E2CB0
		private void OnEnable()
		{
			if (this._targetTransform != null)
			{
				this._originalPosition = this._targetTransform.localPosition;
				this._originalRotation = this._targetTransform.localRotation;
				this._originalScale = this._targetTransform.localScale;
			}
		}

		// Token: 0x06002F07 RID: 12039 RVA: 0x000E4904 File Offset: 0x000E2D04
		private void OnDisable()
		{
			if (this._targetTransform != null)
			{
				this._targetTransform.localPosition = this._originalPosition;
				this._targetTransform.localRotation = this._originalRotation;
				this._targetTransform.localScale = this._originalScale;
			}
		}

		// Token: 0x04001956 RID: 6486
		[SerializeField]
		private Transform _targetTransform;

		// Token: 0x04001957 RID: 6487
		[SerializeField]
		private bool _addToOriginal = true;

		// Token: 0x04001958 RID: 6488
		private Vector3 _originalPosition;

		// Token: 0x04001959 RID: 6489
		private Quaternion _originalRotation;

		// Token: 0x0400195A RID: 6490
		private Vector3 _originalScale;
	}
}
