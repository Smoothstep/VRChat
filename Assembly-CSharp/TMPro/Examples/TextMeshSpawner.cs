using System;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x02000905 RID: 2309
	public class TextMeshSpawner : MonoBehaviour
	{
		// Token: 0x060045A3 RID: 17827 RVA: 0x001776B8 File Offset: 0x00175AB8
		private void Awake()
		{
		}

		// Token: 0x060045A4 RID: 17828 RVA: 0x001776BC File Offset: 0x00175ABC
		private void Start()
		{
			for (int i = 0; i < this.NumberOfNPC; i++)
			{
				if (this.SpawnType == 0)
				{
					GameObject gameObject = new GameObject();
					gameObject.transform.position = new Vector3(UnityEngine.Random.Range(-95f, 95f), 0.5f, UnityEngine.Random.Range(-95f, 95f));
					TextMeshPro textMeshPro = gameObject.AddComponent<TextMeshPro>();
					textMeshPro.fontSize = 96f;
					textMeshPro.text = "!";
					textMeshPro.color = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
					this.floatingText_Script = gameObject.AddComponent<TextMeshProFloatingText>();
					this.floatingText_Script.SpawnType = 0;
				}
				else
				{
					GameObject gameObject2 = new GameObject();
					gameObject2.transform.position = new Vector3(UnityEngine.Random.Range(-95f, 95f), 0.5f, UnityEngine.Random.Range(-95f, 95f));
					TextMesh textMesh = gameObject2.AddComponent<TextMesh>();
					textMesh.GetComponent<Renderer>().sharedMaterial = this.TheFont.material;
					textMesh.font = this.TheFont;
					textMesh.anchor = TextAnchor.LowerCenter;
					textMesh.fontSize = 96;
					textMesh.color = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
					textMesh.text = "!";
					this.floatingText_Script = gameObject2.AddComponent<TextMeshProFloatingText>();
					this.floatingText_Script.SpawnType = 1;
				}
			}
		}

		// Token: 0x04002FCB RID: 12235
		public int SpawnType;

		// Token: 0x04002FCC RID: 12236
		public int NumberOfNPC = 12;

		// Token: 0x04002FCD RID: 12237
		public Font TheFont;

		// Token: 0x04002FCE RID: 12238
		private TextMeshProFloatingText floatingText_Script;
	}
}
