using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x02000909 RID: 2313
	public class VertexShakeA : MonoBehaviour
	{
		// Token: 0x060045B1 RID: 17841 RVA: 0x00178167 File Offset: 0x00176567
		private void Awake()
		{
			this.m_TextComponent = base.GetComponent<TMP_Text>();
		}

		// Token: 0x060045B2 RID: 17842 RVA: 0x00178175 File Offset: 0x00176575
		private void OnEnable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(new Action<UnityEngine.Object>(this.ON_TEXT_CHANGED));
		}

		// Token: 0x060045B3 RID: 17843 RVA: 0x0017818D File Offset: 0x0017658D
		private void OnDisable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(new Action<UnityEngine.Object>(this.ON_TEXT_CHANGED));
		}

		// Token: 0x060045B4 RID: 17844 RVA: 0x001781A5 File Offset: 0x001765A5
		private void Start()
		{
			base.StartCoroutine(this.AnimateVertexColors());
		}

		// Token: 0x060045B5 RID: 17845 RVA: 0x001781B4 File Offset: 0x001765B4
		private void ON_TEXT_CHANGED(UnityEngine.Object obj)
		{
			if (this.m_TextComponent)
			{
				this.hasTextChanged = true;
			}
		}

		// Token: 0x060045B6 RID: 17846 RVA: 0x001781D0 File Offset: 0x001765D0
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
						Quaternion q = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-0.25f, 0.25f) * this.RotationMultiplier);
						for (int k = firstCharacterIndex; k <= lastCharacterIndex; k++)
						{
							if (textInfo.characterInfo[k].isVisible)
							{
								int materialReferenceIndex = textInfo.characterInfo[k].materialReferenceIndex;
								int vertexIndex = textInfo.characterInfo[k].vertexIndex;
								Vector3[] vertices = textInfo.meshInfo[materialReferenceIndex].vertices;
								copyOfVertices[materialReferenceIndex][vertexIndex] = vertices[vertexIndex] - b;
								copyOfVertices[materialReferenceIndex][vertexIndex + 1] = vertices[vertexIndex + 1] - b;
								copyOfVertices[materialReferenceIndex][vertexIndex + 2] = vertices[vertexIndex + 2] - b;
								copyOfVertices[materialReferenceIndex][vertexIndex + 3] = vertices[vertexIndex + 3] - b;
								float d = UnityEngine.Random.Range(0.995f - 0.001f * this.ScaleMultiplier, 1.005f + 0.001f * this.ScaleMultiplier);
								Matrix4x4 matrix = Matrix4x4.TRS(Vector3.one, q, Vector3.one * d);
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

		// Token: 0x04002FD8 RID: 12248
		public float AngleMultiplier = 1f;

		// Token: 0x04002FD9 RID: 12249
		public float SpeedMultiplier = 1f;

		// Token: 0x04002FDA RID: 12250
		public float ScaleMultiplier = 1f;

		// Token: 0x04002FDB RID: 12251
		public float RotationMultiplier = 1f;

		// Token: 0x04002FDC RID: 12252
		private TMP_Text m_TextComponent;

		// Token: 0x04002FDD RID: 12253
		private bool hasTextChanged;
	}
}
