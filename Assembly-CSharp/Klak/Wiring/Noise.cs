using System;
using Klak.Math;
using UnityEngine;

namespace Klak.Wiring
{
	// Token: 0x02000549 RID: 1353
	[AddComponentMenu("Klak/Wiring/Input/Noise")]
	public class Noise : NodeBase
	{
		// Token: 0x06002ED3 RID: 11987 RVA: 0x000E413D File Offset: 0x000E253D
		private void InvokeEvent(float noise)
		{
			this._outputEvent.Invoke((noise + this._bias) * this._amplitude);
		}

		// Token: 0x06002ED4 RID: 11988 RVA: 0x000E4159 File Offset: 0x000E2559
		private void Start()
		{
			this._time = UnityEngine.Random.Range(-10000f, 0f);
		}

		// Token: 0x06002ED5 RID: 11989 RVA: 0x000E4170 File Offset: 0x000E2570
		private void Update()
		{
			this._time += Time.deltaTime * this._frequency;
			if (this._octaves > 1)
			{
				this.InvokeEvent(Perlin.Fbm(this._time, this._octaves));
			}
			else
			{
				this.InvokeEvent(Perlin.Noise(this._time));
			}
		}

		// Token: 0x04001935 RID: 6453
		[SerializeField]
		private float _frequency = 1f;

		// Token: 0x04001936 RID: 6454
		[SerializeField]
		[Range(1f, 8f)]
		private int _octaves = 1;

		// Token: 0x04001937 RID: 6455
		[SerializeField]
		private float _bias;

		// Token: 0x04001938 RID: 6456
		[SerializeField]
		private float _amplitude = 1f;

		// Token: 0x04001939 RID: 6457
		[SerializeField]
		[Outlet]
		private NodeBase.FloatEvent _outputEvent = new NodeBase.FloatEvent();

		// Token: 0x0400193A RID: 6458
		private float _time;
	}
}
