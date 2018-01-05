using System;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x020008E5 RID: 2277
	public class Benchmark03 : MonoBehaviour
	{
		// Token: 0x06004534 RID: 17716 RVA: 0x001728CC File Offset: 0x00170CCC
		private void Awake()
		{
		}

		// Token: 0x06004535 RID: 17717 RVA: 0x001728D0 File Offset: 0x00170CD0
		private void Start()
		{
			for (int i = 0; i < this.NumberOfNPC; i++)
			{
				if (this.SpawnType == 0)
				{
					TextMeshPro textMeshPro = new GameObject
					{
						transform = 
						{
							position = new Vector3(0f, 0f, 0f)
						}
					}.AddComponent<TextMeshPro>();
					textMeshPro.alignment = TextAlignmentOptions.Center;
					textMeshPro.fontSize = 96f;
					textMeshPro.text = "@";
					textMeshPro.color = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
				}
				else
				{
					TextMesh textMesh = new GameObject
					{
						transform = 
						{
							position = new Vector3(0f, 0f, 0f)
						}
					}.AddComponent<TextMesh>();
					textMesh.GetComponent<Renderer>().sharedMaterial = this.TheFont.material;
					textMesh.font = this.TheFont;
					textMesh.anchor = TextAnchor.MiddleCenter;
					textMesh.fontSize = 96;
					textMesh.color = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
					textMesh.text = "@";
				}
			}
		}

		// Token: 0x04002F1F RID: 12063
		public int SpawnType;

		// Token: 0x04002F20 RID: 12064
		public int NumberOfNPC = 12;

		// Token: 0x04002F21 RID: 12065
		public Font TheFont;
	}
}
