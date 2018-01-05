using System;
using System.Collections;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BC9 RID: 3017
	[Serializable]
	public class AfterTimer_Component : MonoBehaviour
	{
		// Token: 0x06005D65 RID: 23909 RVA: 0x00209598 File Offset: 0x00207998
		public void Init(float _time, Action _callback, bool earlydestroy)
		{
			this.triggerTime = _time;
			this.callback = _callback;
			this.triggerOnEarlyDestroy = earlydestroy;
			this.timerActive = true;
			base.StartCoroutine(this.Wait());
		}

		// Token: 0x06005D66 RID: 23910 RVA: 0x002095C4 File Offset: 0x002079C4
		private IEnumerator Wait()
		{
			yield return new WaitForSeconds(this.triggerTime);
			this.timerActive = false;
			this.callback();
			UnityEngine.Object.Destroy(this);
			yield break;
		}

		// Token: 0x06005D67 RID: 23911 RVA: 0x002095DF File Offset: 0x002079DF
		private void OnDestroy()
		{
			if (this.timerActive)
			{
				base.StopCoroutine(this.Wait());
				this.timerActive = false;
				if (this.triggerOnEarlyDestroy)
				{
					this.callback();
				}
			}
		}

		// Token: 0x040042D4 RID: 17108
		private Action callback;

		// Token: 0x040042D5 RID: 17109
		private float triggerTime;

		// Token: 0x040042D6 RID: 17110
		private bool timerActive;

		// Token: 0x040042D7 RID: 17111
		private bool triggerOnEarlyDestroy;
	}
}
