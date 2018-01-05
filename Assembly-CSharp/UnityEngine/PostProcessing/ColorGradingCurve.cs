using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000833 RID: 2099
	[Serializable]
	public sealed class ColorGradingCurve
	{
		// Token: 0x06004154 RID: 16724 RVA: 0x00149AFC File Offset: 0x00147EFC
		public ColorGradingCurve(AnimationCurve curve, float zeroValue, bool loop, Vector2 bounds)
		{
			this.curve = curve;
			this.m_ZeroValue = zeroValue;
			this.m_Loop = loop;
			this.m_Range = bounds.magnitude;
		}

		// Token: 0x06004155 RID: 16725 RVA: 0x00149B28 File Offset: 0x00147F28
		public void Cache()
		{
			if (!this.m_Loop)
			{
				return;
			}
			int length = this.curve.length;
			if (length < 2)
			{
				return;
			}
			if (this.m_InternalLoopingCurve == null)
			{
				this.m_InternalLoopingCurve = new AnimationCurve();
			}
			Keyframe key = this.curve[length - 1];
			key.time -= this.m_Range;
			Keyframe key2 = this.curve[0];
			key2.time += this.m_Range;
			this.m_InternalLoopingCurve.keys = this.curve.keys;
			this.m_InternalLoopingCurve.AddKey(key);
			this.m_InternalLoopingCurve.AddKey(key2);
		}

		// Token: 0x06004156 RID: 16726 RVA: 0x00149BE0 File Offset: 0x00147FE0
		public float Evaluate(float t)
		{
			if (this.curve.length == 0)
			{
				return this.m_ZeroValue;
			}
			if (!this.m_Loop || this.curve.length == 1)
			{
				return this.curve.Evaluate(t);
			}
			return this.m_InternalLoopingCurve.Evaluate(t);
		}

		// Token: 0x04002A62 RID: 10850
		public AnimationCurve curve;

		// Token: 0x04002A63 RID: 10851
		[SerializeField]
		private bool m_Loop;

		// Token: 0x04002A64 RID: 10852
		[SerializeField]
		private float m_ZeroValue;

		// Token: 0x04002A65 RID: 10853
		[SerializeField]
		private float m_Range;

		// Token: 0x04002A66 RID: 10854
		private AnimationCurve m_InternalLoopingCurve;
	}
}
