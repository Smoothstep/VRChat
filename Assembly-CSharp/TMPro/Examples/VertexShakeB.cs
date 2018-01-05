using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x0200090A RID: 2314
	public class VertexShakeB : MonoBehaviour
	{
		// Token: 0x060045B8 RID: 17848 RVA: 0x001787EB File Offset: 0x00176BEB
		private void Awake()
		{
			this.m_TextComponent = base.GetComponent<TMP_Text>();
		}

		// Token: 0x060045B9 RID: 17849 RVA: 0x001787F9 File Offset: 0x00176BF9
		private void OnEnable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(new Action<UnityEngine.Object>(this.ON_TEXT_CHANGED));
		}

		// Token: 0x060045BA RID: 17850 RVA: 0x00178811 File Offset: 0x00176C11
		private void OnDisable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(new Action<UnityEngine.Object>(this.ON_TEXT_CHANGED));
		}

		// Token: 0x060045BB RID: 17851 RVA: 0x00178829 File Offset: 0x00176C29
		private void Start()
		{
			base.StartCoroutine(this.AnimateVertexColors());
		}

		// Token: 0x060045BC RID: 17852 RVA: 0x00178838 File Offset: 0x00176C38
		private void ON_TEXT_CHANGED(UnityEngine.Object obj)
		{
			if (this.m_TextComponent)
			{
				this.hasTextChanged = true;
			}
		}

		// Token: 0x060045BD RID: 17853 RVA: 0x00178854 File Offset: 0x00176C54
		private IEnumerator AnimateVertexColors()
		{
			this.m_TextComponent.ForceMeshUpdate();
			TMP_TextInfo textInfo = this.m_TextComponent.textInfo;
			Vector3[][] copyOfVertices = new Vector3[0][];
			this.hasTextChanged = true;
			for (;;)
			{
				if (this.hasTextChanged)
				{
					if (copyOfVertices.Length < textInfo.meshInfo.Length)
					{
						copyOfVertices = new Vector3[textInfo.meshInfo.Length][];
					}
					for (int i = 0; i < textInfo.meshInfo.Length; i++)
					{
						int num = textInfo.meshInfo[i].vertices.Length;
						copyOfVertices[i] = new Vector3[num];
					}
					this.hasTextChanged = false;
				}
				if (textInfo.characterCount == 0)
				{
					yield return new WaitForSeconds(0.25f);
				}
				else
				{
					int lineCount = textInfo.lineCount;
					for (int j = 0; j < lineCount; j++)
					{
						int firstCharacterIndex = textInfo.lineInfo[j].firstCharacterIndex;
						int lastCharacterIndex = textInfo.lineInfo[j].lastCharacterIndex;
						Vector3 b = (textInfo.characterInfo[firstCharacterIndex].bottomLeft + textInfo.characterInfo[lastCharacterIndex].topRight) / 2f;
						Quaternion q = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-0.25f, 0.25f));
						for (int k = firstCharacterIndex; k <= lastCharacterIndex; k++)
						{
							if (textInfo.characterInfo[k].isVisible)
							{
								int materialReferenceIndex = textInfo.characterInfo[k].materialReferenceIndex;
								int vertexIndex = textInfo.characterInfo[k].vertexIndex;
								Vector3[] vertices = textInfo.meshInfo[materialReferenceIndex].vertices;
								Vector3 b2 = (vertices[vertexIndex] + vertices[vertexIndex + 2]) / 2f;
								copyOfVertices[materialReferenceIndex][vertexIndex] = vertices[vertexIndex] - b2;
								copyOfVertices[materialReferenceIndex][vertexIndex + 1] = vertices[vertexIndex + 1] - b2;
								copyOfVertices[materialReferenceIndex][vertexIndex + 2] = vertices[vertexIndex + 2] - b2;
								copyOfVertices[materialReferenceIndex][vertexIndex + 3] = vertices[vertexIndex + 3] - b2;
								float d = UnityEngine.Random.Range(0.95f, 1.05f);
								Matrix4x4 matrix = Matrix4x4.TRS(Vector3.one, Quaternion.identity, Vector3.one * d);
								copyOfVertices[materialReferenceIndex][vertexIndex] = matrix.MultiplyPoint3x4(copyOfVertices[materialReferenceIndex][vertexIndex]);
								copyOfVertices[materialReferenceIndex][vertexIndex + 1] = matrix.MultiplyPoint3x4(copyOfVertices[materialReferenceIndex][vertexIndex + 1]);
								copyOfVertices[materialReferenceIndex][vertexIndex + 2] = matrix.MultiplyPoint3x4(copyOfVertices[materialReferenceIndex][vertexIndex + 2]);
								copyOfVertices[materialReferenceIndex][vertexIndex + 3] = matrix.MultiplyPoint3x4(copyOfVertices[materialReferenceIndex][vertexIndex + 3]);
								copyOfVertices[materialReferenceIndex][vertexIndex] += b2;
								copyOfVertices[materialReferenceIndex][vertexIndex + 1] += b2;
								copyOfVertices[materialReferenceIndex][vertexIndex + 2] += b2;
								copyOfVertices[materialReferenceIndex][vertexIndex + 3] += b2;
								copyOfVertices[materialReferenceIndex][vertexIndex] -= b;
								copyOfVertices[materialReferenceIndex][vertexIndex + 1] -= b;
								copyOfVertices[materialReferenceIndex][vertexIndex + 2] -= b;
								copyOfVertices[materialReferenceIndex][vertexIndex + 3] -= b;
								matrix = Matrix4x4.TRS(Vector3.one, q, Vector3.one);
								copyOfVertices[materialReferenceIndex][vertexIndex] = matrix.MultiplyPoint3x4(copyOfVertices[materialReferenceIndex][vertexIndex]);
								copyOfVertices[materialReferenceIndex][vertexIndex + 1] = matrix.MultiplyPoint3x4(copyOfVertices[materialReferenceIndex][vertexIndex + 1]);
								copyOfVertices[materialReferenceIndex][vertexIndex + 2] = matrix.MultiplyPoint3x4(copyOfVertices[materialReferenceIndex][vertexIndex + 2]);
								copyOfVertices[materialReferenceIndex][vertexIndex + 3] = matrix.MultiplyPoint3x4(copyOfVertices[materialReferenceIndex][vertexIndex + 3]);
								copyOfVertices[materialReferenceIndex][vertexIndex] += b;
								copyOfVertices[materialReferenceIndex][vertexIndex + 1] += b;
								copyOfVertices[materialReferenceIndex][vertexIndex + 2] += b;
								copyOfVertices[materialReferenceIndex][vertexIndex + 3] += b;
							}
						}
					}
					for (int l = 0; l < textInfo.meshInfo.Length; l++)
					{
						textInfo.meshInfo[l].mesh.vertices = copyOfVertices[l];
						this.m_TextComponent.UpdateGeometry(textInfo.meshInfo[l].mesh, l);
					}
					yield return new WaitForSeconds(0.1f);
				}
			}
			yield break;
		}

		// Token: 0x04002FDE RID: 12254
		public float AngleMultiplier = 1f;

		// Token: 0x04002FDF RID: 12255
		public float SpeedMultiplier = 1f;

		// Token: 0x04002FE0 RID: 12256
		public float CurveScale = 1f;

		// Token: 0x04002FE1 RID: 12257
		private TMP_Text m_TextComponent;

		// Token: 0x04002FE2 RID: 12258
		private bool hasTextChanged;
	}
}
