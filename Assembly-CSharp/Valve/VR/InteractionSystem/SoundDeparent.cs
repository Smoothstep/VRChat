using System;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BC0 RID: 3008
	public class SoundDeparent : MonoBehaviour
	{
		// Token: 0x06005D09 RID: 23817 RVA: 0x00207D38 File Offset: 0x00206138
		private void Awake()
		{
			this.thisAudioSource = base.GetComponent<AudioSource>();
		}

		// Token: 0x06005D0A RID: 23818 RVA: 0x00207D46 File Offset: 0x00206146
		private void Start()
		{
			base.gameObject.transform.parent = null;
			if (this.destroyAfterPlayOnce)
			{
				UnityEngine.Object.Destroy(base.gameObject, this.thisAudioSource.clip.length);
			}
		}

		// Token: 0x040042A1 RID: 17057
		public bool destroyAfterPlayOnce = true;

		// Token: 0x040042A2 RID: 17058
		private AudioSource thisAudioSource;
	}
}
