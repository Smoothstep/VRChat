using System;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x020008E4 RID: 2276
	public class Benchmark02 : MonoBehaviour
	{
		// Token: 0x06004532 RID: 17714 RVA: 0x001725F4 File Offset: 0x001709F4
		private void Start()
		{
			for (int i = 0; i < this.NumberOfNPC; i++)
			{
				if (this.SpawnType == 0)
				{
					GameObject gameObject = new GameObject();
					gameObject.transform.position = new Vector3(UnityEngine.Random.Range(-95f, 95f), 0.25f, UnityEngine.Random.Range(-95f, 95f));
					TextMeshPro textMeshPro = gameObject.AddComponent<TextMeshPro>();
					textMeshPro.autoSizeTextContainer = true;
					textMeshPro.rectTransform.pivot = new Vector2(0.5f, 0f);
					textMeshPro.alignment = TextAlignmentOptions.Bottom;
					textMeshPro.fontSize = 96f;
					textMeshPro.color = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
					textMeshPro.text = "!";
					this.floatingText_Script = gameObject.AddComponent<TextMeshProFloatingText>();
					this.floatingText_Script.SpawnType = 0;
				}
				else if (this.SpawnType == 1)
				{
					GameObject gameObject2 = new GameObject();
					gameObject2.transform.position = new Vector3(UnityEngine.Random.Range(-95f, 95f), 0.25f, UnityEngine.Random.Range(-95f, 95f));
					TextMesh textMesh = gameObject2.AddComponent<TextMesh>();
					textMesh.font = (Resources.Load("Fonts/ARIAL", typeof(Font)) as Font);
					textMesh.GetComponent<Renderer>().sharedMaterial = textMesh.font.material;
					textMesh.anchor = TextAnchor.LowerCenter;
					textMesh.fontSize = 96;
					textMesh.color = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
					textMesh.text = "!";
					this.floatingText_Script = gameObject2.AddComponent<TextMeshProFloatingText>();
					this.floatingText_Script.SpawnType = 1;
				}
				else if (this.SpawnType == 2)
				{
					GameObject gameObject3 = new GameObject();
					Canvas canvas = gameObject3.AddComponent<Canvas>();
					canvas.worldCamera = Camera.main;
					gameObject3.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
					gameObject3.transform.position = new Vector3(UnityEngine.Random.Range(-95f, 95f), 5f, UnityEngine.Random.Range(-95f, 95f));
					TextMeshProUGUI textMeshProUGUI = new GameObject().AddComponent<TextMeshProUGUI>();
					textMeshProUGUI.rectTransform.SetParent(gameObject3.transform, false);
					textMeshProUGUI.color = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
					textMeshProUGUI.alignment = TextAlignmentOptions.Bottom;
					textMeshProUGUI.fontSize = 96f;
					textMeshProUGUI.text = "!";
					this.floatingText_Script = gameObject3.AddComponent<TextMeshProFloatingText>();
					this.floatingText_Script.SpawnType = 0;
				}
			}
		}

		// Token: 0x04002F1C RID: 12060
		public int SpawnType;

		// Token: 0x04002F1D RID: 12061
		public int NumberOfNPC = 12;

		// Token: 0x04002F1E RID: 12062
		private TextMeshProFloatingText floatingText_Script;
	}
}
