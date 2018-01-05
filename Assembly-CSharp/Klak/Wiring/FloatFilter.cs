using System;
using Klak.Math;
using UnityEngine;

namespace Klak.Wiring
{
	// Token: 0x02000538 RID: 1336
	[AddComponentMenu("Klak/Wiring/Filter/Float Filter")]
	public class FloatFilter : NodeBase
	{
		// Token: 0x17000710 RID: 1808
		// (set) Token: 0x06002EA1 RID: 11937 RVA: 0x000E3540 File Offset: 0x000E1940
		[Inlet]
		public float input
		{
			set
			{
				if (!base.enabled)
				{
					return;
				}
				this._inputValue = value;
				if (this._interpolator.enabled)
				{
					this._floatValue.targetValue = this.EvalResponse();
				}
				else
				{
					this._outputEvent.Invoke(this.EvalResponse());
				}
			}
		}

		// Token: 0x06002EA2 RID: 11938 RVA: 0x000E3597 File Offset: 0x000E1997
		private float EvalResponse()
		{
			return this._responseCurve.Evaluate(this._inputValue) * this._amplitude + this._bias;
		}

		// Token: 0x06002EA3 RID: 11939 RVA: 0x000E35B8 File Offset: 0x000E19B8
		private void Start()
		{
			this._floatValue = new FloatInterpolator(0f, this._interpolator);
		}

		// Token: 0x06002EA4 RID: 11940 RVA: 0x000E35D0 File Offset: 0x000E19D0
		private void Update()
		{
			if (this._interpolator.enabled)
			{
				this._outputEvent.Invoke(this._floatValue.Step());
			}
		}

		// Token: 0x040018CA RID: 6346
		[SerializeField]
		private AnimationCurve _responseCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x040018CB RID: 6347
		[SerializeField]
		private FloatInterpolator.Config _interpolator = FloatInterpolator.Config.Direct;

		// Token: 0x040018CC RID: 6348
		[SerializeField]
		private float _amplitude = 1f;

		// Token: 0x040018CD RID: 6349
		[SerializeField]
		private float _bias;

		// Token: 0x040018CE RID: 6350
		[SerializeField]
		[Outlet]
		private NodeBase.FloatEvent _outputEvent = new NodeBase.FloatEvent();

		// Token: 0x040018CF RID: 6351
		private float _inputValue;

		// Token: 0x040018D0 RID: 6352
		private FloatInterpolator _floatValue;
	}
}
