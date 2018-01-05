using System;
using UnityEngine;

namespace Klak.Wiring
{
	// Token: 0x02000533 RID: 1331
	[AddComponentMenu("Klak/Wiring/Animation/Float Animation")]
	public class FloatAnimation : NodeBase
	{
		// Token: 0x1700070C RID: 1804
		// (set) Token: 0x06002E92 RID: 11922 RVA: 0x000E32EF File Offset: 0x000E16EF
		[Inlet]
		public float timeScale
		{
			set
			{
				this._timeScale = value;
			}
		}

		// Token: 0x06002E93 RID: 11923 RVA: 0x000E32F8 File Offset: 0x000E16F8
		[Inlet]
		public void Play()
		{
			this._time = 0f;
			this._isPlaying = true;
		}

		// Token: 0x06002E94 RID: 11924 RVA: 0x000E330C File Offset: 0x000E170C
		[Inlet]
		public void Stop()
		{
			this._isPlaying = false;
		}

		// Token: 0x06002E95 RID: 11925 RVA: 0x000E3315 File Offset: 0x000E1715
		[Inlet]
		public void TogglePause()
		{
			this._isPlaying = !this._isPlaying;
		}

		// Token: 0x06002E96 RID: 11926 RVA: 0x000E3326 File Offset: 0x000E1726
		private void Start()
		{
			this._isPlaying = this._playOnStart;
			this._timeScale = 1f;
		}

		// Token: 0x06002E97 RID: 11927 RVA: 0x000E3340 File Offset: 0x000E1740
		private void Update()
		{
			if (this._isPlaying)
			{
				this._time += Time.deltaTime * this._speed * this._timeScale;
				this._floatEvent.Invoke(this._curve.Evaluate(this._time));
			}
		}

		// Token: 0x040018B7 RID: 6327
		[SerializeField]
		private AnimationCurve _curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x040018B8 RID: 6328
		[SerializeField]
		private float _speed = 1f;

		// Token: 0x040018B9 RID: 6329
		[SerializeField]
		private bool _playOnStart = true;

		// Token: 0x040018BA RID: 6330
		[SerializeField]
		[Outlet]
		private NodeBase.FloatEvent _floatEvent = new NodeBase.FloatEvent();

		// Token: 0x040018BB RID: 6331
		private float _time;

		// Token: 0x040018BC RID: 6332
		private float _timeScale;

		// Token: 0x040018BD RID: 6333
		private bool _isPlaying;
	}
}
