using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets.SceneUtils
{
	// Token: 0x02000A29 RID: 2601
	public class SlowMoButton : MonoBehaviour
	{
		// Token: 0x06004E6B RID: 20075 RVA: 0x001A480D File Offset: 0x001A2C0D
		private void Start()
		{
			this.m_SlowMo = false;
		}

		// Token: 0x06004E6C RID: 20076 RVA: 0x001A4816 File Offset: 0x001A2C16
		private void OnDestroy()
		{
			Time.timeScale = 1f;
		}

		// Token: 0x06004E6D RID: 20077 RVA: 0x001A4824 File Offset: 0x001A2C24
		public void ChangeSpeed()
		{
			this.m_SlowMo = !this.m_SlowMo;
			Image image = this.button.targetGraphic as Image;
			if (image != null)
			{
				image.sprite = ((!this.m_SlowMo) ? this.FullSpeedTex : this.SlowSpeedTex);
			}
			this.button.targetGraphic = image;
			Time.timeScale = ((!this.m_SlowMo) ? this.fullSpeed : this.slowSpeed);
		}

		// Token: 0x0400368A RID: 13962
		public Sprite FullSpeedTex;

		// Token: 0x0400368B RID: 13963
		public Sprite SlowSpeedTex;

		// Token: 0x0400368C RID: 13964
		public float fullSpeed = 1f;

		// Token: 0x0400368D RID: 13965
		public float slowSpeed = 0.3f;

		// Token: 0x0400368E RID: 13966
		public Button button;

		// Token: 0x0400368F RID: 13967
		private bool m_SlowMo;
	}
}
