using System;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x020008E6 RID: 2278
	public class Benchmark04 : MonoBehaviour
	{
		// Token: 0x06004537 RID: 17719 RVA: 0x00172A20 File Offset: 0x00170E20
		private void Start()
		{
			this.m_Transform = base.transform;
			float num = 0f;
			float num2 = (float)(Screen.height / 2);
			Camera.main.orthographicSize = num2;
			float num3 = num2;
			float num4 = (float)Screen.width / (float)Screen.height;
			for (int i = this.MinPointSize; i <= this.MaxPointSize; i += this.Steps)
			{
				if (this.SpawnType == 0)
				{
					GameObject gameObject = new GameObject("Text - " + i + " Pts");
					if (num > num3 * 2f)
					{
						return;
					}
					gameObject.transform.position = this.m_Transform.position + new Vector3(num4 * -num3 * 0.975f, num3 * 0.975f - num, 0f);
					TextMeshPro textMeshPro = gameObject.AddComponent<TextMeshPro>();
					textMeshPro.rectTransform.pivot = new Vector2(0f, 0.5f);
					textMeshPro.enableWordWrapping = false;
					textMeshPro.extraPadding = true;
					textMeshPro.isOrthographic = true;
					textMeshPro.fontSize = (float)i;
					textMeshPro.text = i + " pts - Lorem ipsum dolor sit...";
					textMeshPro.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
					num += (float)i;
				}
			}
		}

		// Token: 0x04002F22 RID: 12066
		public int SpawnType;

		// Token: 0x04002F23 RID: 12067
		public int MinPointSize = 12;

		// Token: 0x04002F24 RID: 12068
		public int MaxPointSize = 64;

		// Token: 0x04002F25 RID: 12069
		public int Steps = 4;

		// Token: 0x04002F26 RID: 12070
		private Transform m_Transform;
	}
}
